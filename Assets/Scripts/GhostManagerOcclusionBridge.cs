using UnityEngine;
using System.Collections.Generic;

public class GhostManagerOcclusionBridge : MonoBehaviour
{
    [Header("References")]
    public GhostOcclusionIntegrator occlusionIntegrator;
    public GameObject enhancedGhostPrefab;
    
    [Header("Settings")]
    public bool enableOcclusion = true;
    
    // Track registered ghosts to avoid duplicates
    private HashSet<GameObject> registeredGhosts = new HashSet<GameObject>();
    
    private void Start()
    {
        if (occlusionIntegrator == null)
            occlusionIntegrator = GetComponent<GhostOcclusionIntegrator>();
        
        // Register any existing ghosts at start
        if (enableOcclusion)
        {
            RegisterExistingGhosts();
        }
    }
    
    /// <summary>
    /// Register existing ghosts in scene (called once at start)
    /// </summary>
    public void RegisterExistingGhosts()
    {
        if (!enableOcclusion || occlusionIntegrator == null) return;
        
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        
        foreach (GameObject ghost in ghosts)
        {
            RegisterSingleGhost(ghost);
        }
        
        Debug.Log($"Registered {registeredGhosts.Count} existing ghosts for occlusion");
    }
    
    /// <summary>
    /// Register a single ghost (called when new ghosts are spawned)
    /// </summary>
    public void RegisterSingleGhost(GameObject ghost)
    {
        if (!enableOcclusion || occlusionIntegrator == null || ghost == null) return;
        
        // Only register if not already registered
        if (registeredGhosts.Contains(ghost)) return;
        
        // Upgrade ghost if needed
        UpgradeGhostIfNeeded(ghost);
        
        // Register with occlusion system
        occlusionIntegrator.OnGhostSpawned(ghost);
        registeredGhosts.Add(ghost);
        
        Debug.Log($"Registered new ghost: {ghost.name}");
    }
    
    /// <summary>
    /// Unregister a ghost (called when ghost is destroyed)
    /// </summary>
    public void UnregisterSingleGhost(GameObject ghost)
    {
        if (!enableOcclusion || occlusionIntegrator == null || ghost == null) return;
        
        if (registeredGhosts.Contains(ghost))
        {
            occlusionIntegrator.OnGhostDestroyed(ghost);
            registeredGhosts.Remove(ghost);
            Debug.Log($"Unregistered ghost: {ghost.name}");
        }
    }
    
    /// <summary>
    /// Upgrades existing ghosts to use enhanced tap handler
    /// </summary>
    private void UpgradeGhostIfNeeded(GameObject ghost)
    {
        EnhancedGhostTapHandler enhancedHandler = ghost.GetComponent<EnhancedGhostTapHandler>();
        if (enhancedHandler != null)
            return; // Already upgraded
        
        GhostTapHandler oldHandler = ghost.GetComponent<GhostTapHandler>();
        if (oldHandler != null)
        {
            DestroyImmediate(oldHandler);
            ghost.AddComponent<EnhancedGhostTapHandler>();
            
            if (ghost.GetComponent<AudioSource>() == null)
                ghost.AddComponent<AudioSource>();
            
            Debug.Log($"Upgraded ghost {ghost.name} to enhanced tap handler");
        }
    }
    
    /// <summary>
    /// Called by GhostManager when spawning new ghosts
    /// </summary>
    public void OnGhostSpawned(GameObject ghost)
    {
        RegisterSingleGhost(ghost);
    }
    
    /// <summary>
    /// Called by Enhanced tap handler when ghost is destroyed
    /// </summary>
    public void OnGhostDestroyed(GameObject ghost)
    {
        UnregisterSingleGhost(ghost);
    }
    
    /// <summary>
    /// Clear all registered ghosts
    /// </summary>
    public void ClearAllRegistrations()
    {
        registeredGhosts.Clear();
        Debug.Log("Cleared all ghost registrations");
    }
    
    /// <summary>
    /// Refresh occlusion for all registered ghosts
    /// </summary>
    public void RefreshOcclusion()
    {
        if (occlusionIntegrator != null && enableOcclusion)
        {
            occlusionIntegrator.RefreshAllGhosts();
        }
    }
    
    /// <summary>
    /// Toggle occlusion system on/off
    /// </summary>
    public void ToggleOcclusion()
    {
        enableOcclusion = !enableOcclusion;
        
        if (enableOcclusion)
        {
            RegisterExistingGhosts();
            Debug.Log("Occlusion enabled");
        }
        else
        {
            ClearAllRegistrations();
            Debug.Log("Occlusion disabled");
        }
    }
}