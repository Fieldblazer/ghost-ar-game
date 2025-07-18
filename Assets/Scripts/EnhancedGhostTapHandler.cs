using UnityEngine;

public class EnhancedGhostTapHandler : MonoBehaviour
{
    [Header("Ghost Settings")]
    public bool isVisible = true;
    public bool canBeTapped = true;
    
    [Header("Visual Effects")]
    public GameObject disappearEffect;
    public AudioClip foundSound;
    
    private GhostManagerOcclusionBridge occlusionBridge;
    private AudioSource audioSource;
    private Renderer[] renderers;
    private Collider[] colliders;
    
    private void Start()
    {
        // Cache components
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();
        audioSource = GetComponent<AudioSource>();
        
        // Find the occlusion bridge
        occlusionBridge = FindObjectOfType<GhostManagerOcclusionBridge>();
        
        // Make sure the ghost is tagged properly
        if (!CompareTag("Ghost"))
        {
            tag = "Ghost";
            Debug.Log($"Auto-tagged {gameObject.name} as Ghost");
        }
        
        // Register this ghost with the occlusion system
        if (occlusionBridge != null)
        {
            occlusionBridge.RegisterSingleGhost(gameObject);
        }
    }
    
    private void OnMouseDown()
    {
        if (canBeTapped && isVisible)
        {
            OnGhostTapped();
        }
    }
    
    /// <summary>
    /// Called when the ghost is tapped
    /// </summary>
    public void OnGhostTapped()
    {
        if (!canBeTapped || !isVisible) return;
        
        // Play sound effect
        if (audioSource != null && foundSound != null)
        {
            audioSource.PlayOneShot(foundSound);
        }
        
        // Spawn disappear effect
        if (disappearEffect != null)
        {
            Instantiate(disappearEffect, transform.position, transform.rotation);
        }
        
        // Unregister from occlusion system FIRST
        if (occlusionBridge != null)
        {
            occlusionBridge.UnregisterSingleGhost(gameObject);
        }
        
        // Notify the GhostManager
        GhostManager ghostManager = FindObjectOfType<GhostManager>();
        if (ghostManager != null)
        {
            ghostManager.SendMessage("GhostFound", gameObject, SendMessageOptions.DontRequireReceiver);
        }
        
        // Destroy the ghost
        Destroy(gameObject);
    }
    
    /// <summary>
    /// Set ghost visibility (called by occlusion system)
    /// </summary>
    public void SetVisibility(bool visible)
    {
        isVisible = visible;
        canBeTapped = visible;
        
        // Update renderers
        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
                renderer.enabled = visible;
        }
        
        // Update colliders (so hidden ghosts can't be tapped)
        foreach (Collider collider in colliders)
        {
            if (collider != null)
                collider.enabled = visible;
        }
    }
    
    /// <summary>
    /// Get the current visibility state
    /// </summary>
    public bool IsVisible()
    {
        return isVisible;
    }
    
    /// <summary>
    /// Force refresh visibility (useful for debugging)
    /// </summary>
    public void RefreshVisibility()
    {
        SetVisibility(isVisible);
    }
    
    private void OnDestroy()
    {
        // Safety unregister from occlusion system
        if (occlusionBridge != null)
        {
            occlusionBridge.UnregisterSingleGhost(gameObject);
        }
    }
}