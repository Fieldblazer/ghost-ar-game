using UnityEngine;

public class GhostVFXUpgrader : MonoBehaviour
{
    [Header("VFX Upgrade Configuration")]
    public bool autoUpgradeOnStart = true;
    public bool upgradeExistingGhosts = true;
    public AudioClip defaultTapSound;
    
    [Header("VFX Settings")]
    public float tapScaleMultiplier = 1.4f;
    public float animationDuration = 0.4f;
    public Color foundColor = Color.yellow;
    
    void Start()
    {
        if (autoUpgradeOnStart)
        {
            UpgradeAllGhosts();
        }
    }
    
    [ContextMenu("Upgrade All Ghosts with VFX")]
    public void UpgradeAllGhosts()
    {
        Debug.Log("=== UPGRADING GHOSTS WITH VFX SYSTEM ===");
        
        GameObject[] allGhosts = GameObject.FindGameObjectsWithTag("Ghost");
        int upgradedCount = 0;
        
        foreach (GameObject ghost in allGhosts)
        {
            if (UpgradeGhost(ghost))
            {
                upgradedCount++;
            }
        }
        
        Debug.Log($"VFX Upgrade complete: {upgradedCount}/{allGhosts.Length} ghosts upgraded");
    }
    
    bool UpgradeGhost(GameObject ghost)
    {
        if (ghost == null) return false;
        
        // Check if already has VFX components
        if (ghost.GetComponent<GhostVFX>() != null)
        {
            Debug.Log($"Ghost {ghost.name} already has VFX system");
            return false;
        }
        
        Debug.Log($"Upgrading ghost: {ghost.name}");
        
        // Add VFX component
        GhostVFX vfx = ghost.AddComponent<GhostVFX>();
        vfx.tapScaleMultiplier = tapScaleMultiplier;
        vfx.animationDuration = animationDuration;
        vfx.foundColor = foundColor;
        
        // Add enhanced tap handler
        EnhancedGhostTapHandlerVFX tapHandler = ghost.AddComponent<EnhancedGhostTapHandlerVFX>();
        tapHandler.ghostVFX = vfx;
        tapHandler.tapSound = defaultTapSound;
        
        // Remove old tap handler if present
        EnhancedGhostTapHandler oldHandler = ghost.GetComponent<EnhancedGhostTapHandler>();
        if (oldHandler != null)
        {
            Debug.Log($"Removing old tap handler from {ghost.name}");
            DestroyImmediate(oldHandler);
        }
        
        // Ensure audio source
        AudioSource audioSource = ghost.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = ghost.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f; // 3D audio
        }
        tapHandler.audioSource = audioSource;
        
        // Ensure collider for interaction
        SphereCollider collider = ghost.GetComponent<SphereCollider>();
        if (collider == null)
        {
            collider = ghost.AddComponent<SphereCollider>();
            collider.radius = 0.5f;
            collider.isTrigger = true;
        }
        
        return true;
    }
    
    public void UpgradeNewGhost(GameObject ghost)
    {
        if (UpgradeGhost(ghost))
        {
            Debug.Log($"New ghost upgraded with VFX: {ghost.name}");
        }
    }
    
    // Called when smart spawning creates new ghosts
    void OnGhostSpawned(GameObject ghost)
    {
        if (ghost != null)
        {
            UpgradeNewGhost(ghost);
        }
    }
    
    [ContextMenu("Remove VFX from All Ghosts")]
    public void RemoveVFXFromAllGhosts()
    {
        GameObject[] allGhosts = GameObject.FindGameObjectsWithTag("Ghost");
        int removedCount = 0;
        
        foreach (GameObject ghost in allGhosts)
        {
            GhostVFX vfx = ghost.GetComponent<GhostVFX>();
            if (vfx != null)
            {
                DestroyImmediate(vfx);
                removedCount++;
            }
            
            EnhancedGhostTapHandlerVFX vfxHandler = ghost.GetComponent<EnhancedGhostTapHandlerVFX>();
            if (vfxHandler != null)
            {
                DestroyImmediate(vfxHandler);
            }
        }
        
        Debug.Log($"VFX removed from {removedCount} ghosts");
    }
    
    [ContextMenu("Test VFX on Random Ghost")]
    public void TestVFXOnRandomGhost()
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        if (ghosts.Length > 0)
        {
            int randomIndex = Random.Range(0, ghosts.Length);
            GameObject testGhost = ghosts[randomIndex];
            
            GhostVFX vfx = testGhost.GetComponent<GhostVFX>();
            if (vfx != null)
            {
                Debug.Log($"Testing VFX on: {testGhost.name}");
                vfx.PlayFoundEffect();
            }
            else
            {
                Debug.LogWarning($"Ghost {testGhost.name} has no VFX component");
            }
        }
        else
        {
            Debug.LogWarning("No ghosts found to test VFX");
        }
    }
}