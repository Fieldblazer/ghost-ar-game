using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SmartGhostSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public int totalGhosts = 5;
    public float spawnRadius = 5f;
    public float minHeight = 0.5f;
    public float maxHeight = 3f;
    
    [Header("AR References")]
    public ARPlaneManager planeManager;
    public Camera arCamera;
    
    [Header("Smart Spawning")]
    public bool useContextualSpawning = true;
    public bool useSurfaceSpawning = true;
    public bool useRaycastValidation = true;
    
    private List<ARPlane> detectedPlanes = new List<ARPlane>();
    
    public enum SpawnMethod
    {
        Contextual,
        Surface,
        Raycast,
        Random
    }
    
    /// <summary>
    /// Generate spawn positions using smart hierarchy
    /// </summary>
    public List<Vector3> GenerateSpawnPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        UpdatePlanes();
        
        for (int i = 0; i < totalGhosts; i++)
        {
            Vector3 pos = GetSmartSpawnPosition();
            positions.Add(pos);
        }
        
        return positions;
    }
    
    /// <summary>
    /// Try spawning methods in priority order
    /// </summary>
    private Vector3 GetSmartSpawnPosition()
    {
        Vector3 playerPos = arCamera.transform.position;
        
        // 1. Try Contextual (behind furniture, corners)
        if (useContextualSpawning)
        {
            Vector3 contextPos = TryContextualSpawn();
            if (contextPos != Vector3.zero)
                return contextPos;
        }
        
        // 2. Try Surface-based (on AR planes)
        if (useSurfaceSpawning && detectedPlanes.Count > 0)
        {
            Vector3 surfacePos = TrySurfaceSpawn();
            if (surfacePos != Vector3.zero)
                return surfacePos;
        }
        
        // 3. Try Raycast validation
        if (useRaycastValidation)
        {
            Vector3 raycastPos = TryRaycastSpawn();
            if (raycastPos != Vector3.zero)
                return raycastPos;
        }
        
        // 4. Fallback to random
        return GetRandomPosition();
    }
    
    private Vector3 TryContextualSpawn()
    {
        Vector3 playerPos = arCamera.transform.position;
        Vector3 playerForward = arCamera.transform.forward;
        
        // Try to spawn behind player or to the sides
        for (int i = 0; i < 5; i++)
        {
            float angle = Random.Range(90f, 270f); // Behind and sides
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 pos = playerPos + direction * Random.Range(2f, spawnRadius);
            pos.y = playerPos.y + Random.Range(minHeight, maxHeight);
            
            return pos;
        }
        
        return Vector3.zero;
    }
    
    private Vector3 TrySurfaceSpawn()
    {
        if (detectedPlanes.Count == 0) return Vector3.zero;
        
        ARPlane plane = detectedPlanes[Random.Range(0, detectedPlanes.Count)];
        Vector2 randomPoint = Random.insideUnitCircle * 0.5f;
        
        Vector3 localPoint = new Vector3(
            randomPoint.x * plane.size.x,
            0,
            randomPoint.y * plane.size.y
        );
        
        Vector3 worldPoint = plane.transform.TransformPoint(localPoint);
        return worldPoint + plane.normal * Random.Range(0.2f, 1.5f);
    }
    
    private Vector3 TryRaycastSpawn()
    {
        Vector3 playerPos = arCamera.transform.position;
        
        for (int i = 0; i < 5; i++)
        {
            Vector3 randomDir = Random.insideUnitSphere;
            randomDir.y = 0;
            randomDir = randomDir.normalized;
            
            Vector3 testPos = playerPos + randomDir * Random.Range(2f, spawnRadius);
            testPos.y = playerPos.y + 3f; // Start high
            
            if (Physics.Raycast(testPos, Vector3.down, out RaycastHit hit, 5f))
            {
                return hit.point + Vector3.up * Random.Range(0.5f, 2f);
            }
        }
        
        return Vector3.zero;
    }
    
    private Vector3 GetRandomPosition()
    {
        Vector3 playerPos = arCamera.transform.position;
        Vector3 randomDir = Random.insideUnitSphere;
        randomDir.y = 0;
        randomDir = randomDir.normalized;
        
        Vector3 pos = playerPos + randomDir * Random.Range(2f, spawnRadius);
        pos.y = playerPos.y + Random.Range(minHeight, maxHeight);
        
        return pos;
    }
    
    private void UpdatePlanes()
    {
        detectedPlanes.Clear();
        if (planeManager != null)
        {
            foreach (var plane in planeManager.trackables)
            {
                if (plane.gameObject.activeInHierarchy)
                {
                    detectedPlanes.Add(plane);
                }
            }
        }
    }
    
    private void Start()
    {
        if (arCamera == null)
            arCamera = Camera.main;
        
        if (planeManager == null)
            planeManager = FindObjectOfType<ARPlaneManager>();
    }
}