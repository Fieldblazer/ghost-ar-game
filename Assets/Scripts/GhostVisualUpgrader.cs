using UnityEngine;

public class GhostVisualUpgrader : MonoBehaviour
{
    [Header("Visual Upgrade Settings")]
    public bool autoUpgradeOnStart = true;
    public Color defaultGhostColor = new Color(0.8f, 0.9f, 1f, 0.6f);
    public bool enableGlowEffect = true;
    public bool enableFloatingAnimation = true;
    
    [Header("Ghost Varieties")]
    public Color[] ghostColorVariants = {
        new Color(0.8f, 0.9f, 1f, 0.6f),    // Pale blue
        new Color(1f, 0.8f, 0.9f, 0.6f),    // Pale pink
        new Color(0.9f, 1f, 0.8f, 0.6f),    // Pale green
        new Color(1f, 1f, 0.8f, 0.6f),      // Pale yellow
        new Color(0.9f, 0.8f, 1f, 0.6f)     // Pale purple
    };
    
    void Start()
    {
        if (autoUpgradeOnStart)
        {
            UpgradeAllGhostVisuals();
        }
    }
    
    [ContextMenu("Upgrade All Ghost Visuals")]
    public void UpgradeAllGhostVisuals()
    {
        Debug.Log("=== UPGRADING GHOST VISUALS ===");
        
        GameObject[] allGhosts = GameObject.FindGameObjectsWithTag("Ghost");
        int upgradedCount = 0;
        
        foreach (GameObject ghost in allGhosts)
        {
            if (UpgradeGhostVisual(ghost))
            {
                upgradedCount++;
            }
        }
        
        Debug.Log($"Visual upgrade complete: {upgradedCount}/{allGhosts.Length} ghosts upgraded");
    }
    
    bool UpgradeGhostVisual(GameObject ghost)
    {
        if (ghost == null) return false;
        
        // Check if already has visual upgrade
        if (ghost.GetComponentInChildren<GhostVisualUpgrade>() != null)
        {
            Debug.Log($"Ghost {ghost.name} already has visual upgrade");
            return false;
        }
        
        Debug.Log($"Upgrading visual for ghost: {ghost.name}");
        
        // Find the visual child object (usually named GhostVisual)
        Transform visualChild = ghost.transform.Find("GhostVisual");
        if (visualChild == null)
        {
            // If no child, apply to the ghost itself
            visualChild = ghost.transform;
        }
        
        // Add visual upgrade component
        GhostVisualUpgrade visualUpgrade = visualChild.gameObject.AddComponent<GhostVisualUpgrade>();
        
        // Configure with random color variant
        Color chosenColor = ghostColorVariants[Random.Range(0, ghostColorVariants.Length)];
        visualUpgrade.ghostColor = chosenColor;
        visualUpgrade.enableGlow = enableGlowEffect;
        visualUpgrade.enableFloating = enableFloatingAnimation;
        
        // Add some randomization to animation
        visualUpgrade.floatSpeed = Random.Range(0.8f, 1.2f);
        visualUpgrade.floatHeight = Random.Range(0.1f, 0.3f);
        
        return true;
    }
    
    public void UpgradeNewGhost(GameObject ghost)
    {
        if (UpgradeGhostVisual(ghost))
        {
            Debug.Log($"New ghost visual upgraded: {ghost.name}");
        }
    }
    
    [ContextMenu("Remove Visual Upgrades")]
    public void RemoveAllVisualUpgrades()
    {
        GhostVisualUpgrade[] upgrades = FindObjectsOfType<GhostVisualUpgrade>();
        int removedCount = 0;
        
        foreach (GhostVisualUpgrade upgrade in upgrades)
        {
            DestroyImmediate(upgrade);
            removedCount++;
        }
        
        Debug.Log($"Removed {removedCount} visual upgrades");
    }
    
    [ContextMenu("Test Visual Variety")]
    public void TestVisualVariety()
    {
        Debug.Log("=== TESTING GHOST VISUAL VARIETY ===");
        
        for (int i = 0; i < ghostColorVariants.Length; i++)
        {
            Color color = ghostColorVariants[i];
            Debug.Log($"Color variant {i}: R={color.r:F1}, G={color.g:F1}, B={color.b:F1}, A={color.a:F1}");
        }
    }
}