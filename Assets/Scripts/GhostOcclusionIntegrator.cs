using UnityEngine;

public class GhostOcclusionIntegrator : MonoBehaviour
{
    [Header("References")]
    public GhostOcclusionManager occlusionManager;
    
    private void Start()
    {
        // Find the occlusion manager if not assigned
        if (occlusionManager == null)
        {
            occlusionManager = FindObjectOfType<GhostOcclusionManager>();
        }
        
        if (occlusionManager == null)
        {
            Debug.LogError("GhostOcclusionManager not found! Please add it to the scene.");
        }
    }
    
    /// <summary>
    /// Call this method when a ghost is spawned
    /// </summary>
    public void OnGhostSpawned(GameObject ghost)
    {
        if (occlusionManager != null)
        {
            occlusionManager.RegisterGhost(ghost);
            Debug.Log($"Registered ghost {ghost.name} for occlusion");
        }
    }
    
    /// <summary>
    /// Call this method when a ghost is destroyed/found
    /// </summary>
    public void OnGhostDestroyed(GameObject ghost)
    {
        if (occlusionManager != null)
        {
            occlusionManager.UnregisterGhost(ghost);
            Debug.Log($"Unregistered ghost {ghost.name} from occlusion");
        }
    }
    
    /// <summary>
    /// Call this to refresh all ghosts in the scene
    /// </summary>
    public void RefreshAllGhosts()
    {
        if (occlusionManager != null)
        {
            occlusionManager.RefreshGhostList();
            Debug.Log("Refreshed all ghosts for occlusion system");
        }
    }
}