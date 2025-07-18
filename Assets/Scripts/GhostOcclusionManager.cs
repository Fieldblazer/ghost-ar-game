using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GhostOcclusionManager : MonoBehaviour
{
    [Header("Occlusion Settings")]
    public float occlusionCheckInterval = 0.1f;
    public float occlusionTolerance = 0.5f;
    
    [Header("AR Components")]
    public ARPlaneManager planeManager;
    public Camera arCamera;
    
    private List<GameObject> activeGhosts = new List<GameObject>();
    private List<ARPlane> detectedPlanes = new List<ARPlane>();
    private float lastOcclusionCheck = 0f;
    
    private void Start()
    {
        if (arCamera == null)
            arCamera = Camera.main;
        
        if (planeManager == null)
            planeManager = FindObjectOfType<ARPlaneManager>();
            
        if (planeManager != null)
        {
            planeManager.planesChanged += OnPlanesChanged;
        }
        
        RefreshGhostList();
    }
    
    private void Update()
    {
        if (Time.time - lastOcclusionCheck >= occlusionCheckInterval)
        {
            CheckGhostOcclusion();
            lastOcclusionCheck = Time.time;
        }
    }
    
    public void RegisterGhost(GameObject ghost)
    {
        if (!activeGhosts.Contains(ghost))
        {
            activeGhosts.Add(ghost);
        }
    }
    
    public void UnregisterGhost(GameObject ghost)
    {
        if (activeGhosts.Contains(ghost))
        {
            activeGhosts.Remove(ghost);
        }
    }
    
    public void RefreshGhostList()
    {
        activeGhosts.Clear();
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        
        foreach (GameObject ghost in ghosts)
        {
            RegisterGhost(ghost);
        }
    }
    
    private void CheckGhostOcclusion()
    {
        if (arCamera == null || activeGhosts.Count == 0)
            return;
            
        Vector3 cameraPosition = arCamera.transform.position;
        
        foreach (GameObject ghost in activeGhosts)
        {
            if (ghost == null) continue;
            
            bool isOccluded = IsGhostOccluded(cameraPosition, ghost.transform.position);
            SetGhostVisibility(ghost, !isOccluded);
        }
    }
    
    private bool IsGhostOccluded(Vector3 cameraPos, Vector3 ghostPos)
    {
        Vector3 direction = ghostPos - cameraPos;
        float distance = direction.magnitude;
        
        Ray ray = new Ray(cameraPos, direction.normalized);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, distance - occlusionTolerance))
        {
            if (!hit.collider.CompareTag("Ghost"))
            {
                return true;
            }
        }
        
        return false;
    }
    
    private void SetGhostVisibility(GameObject ghost, bool visible)
    {
        Renderer[] renderers = ghost.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = visible;
        }
        
        Collider[] colliders = ghost.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = visible;
        }
    }
    
    private void OnPlanesChanged(ARPlanesChangedEventArgs eventArgs)
    {
        foreach (ARPlane addedPlane in eventArgs.added)
        {
            if (!detectedPlanes.Contains(addedPlane))
            {
                detectedPlanes.Add(addedPlane);
            }
        }
        
        foreach (ARPlane removedPlane in eventArgs.removed)
        {
            if (detectedPlanes.Contains(removedPlane))
            {
                detectedPlanes.Remove(removedPlane);
            }
        }
    }
    
    private void OnDestroy()
    {
        if (planeManager != null)
        {
            planeManager.planesChanged -= OnPlanesChanged;
        }
    }
}