using UnityEngine;

public class VFXSystemTester : MonoBehaviour
{
    [Header("VFX System Testing")]
    public GhostVFXUpgrader vfxUpgrader;
    
    [Header("Test Settings")]
    public int testGhostCount = 3;
    public float testSpawnRadius = 3f;
    
    void Start()
    {
        if (vfxUpgrader == null)
            vfxUpgrader = FindObjectOfType<GhostVFXUpgrader>();
        
        Debug.Log("VFX System Tester initialized");
        ValidateVFXSystem();
    }
    
    void ValidateVFXSystem()
    {
        Debug.Log("=== VFX SYSTEM VALIDATION ===");
        
        // Check core components
        Debug.Log("VFXUpgrader found: " + (vfxUpgrader != null));
        
        // Check existing ghosts
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        Debug.Log("Total ghosts in scene: " + ghosts.Length);
        
        int vfxEnabledGhosts = 0;
        foreach (GameObject ghost in ghosts)
        {
            if (ghost.GetComponent<GhostVFX>() != null)
                vfxEnabledGhosts++;
        }
        
        Debug.Log("Ghosts with VFX: " + vfxEnabledGhosts + "/" + ghosts.Length);
        Debug.Log("=== VFX SYSTEM READY ===");
    }
    
    [ContextMenu("Test VFX Integration")]
    public void TestVFXIntegration()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("VFX testing requires Play mode");
            return;
        }
        
        Debug.Log("=== TESTING VFX INTEGRATION ===");
        
        // Test: Upgrade existing ghosts
        if (vfxUpgrader != null)
        {
            vfxUpgrader.UpgradeAllGhosts();
        }
        
        // Test: Test VFX on random ghost
        Invoke("TestRandomGhostVFX", 1f);
    }
    
    void TestRandomGhostVFX()
    {
        Debug.Log("Testing VFX on random ghost...");
        
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        if (ghosts.Length > 0)
        {
            int randomIndex = Random.Range(0, ghosts.Length);
            GameObject testGhost = ghosts[randomIndex];
            
            GhostVFX vfx = testGhost.GetComponent<GhostVFX>();
            if (vfx != null)
            {
                Debug.Log("Triggering VFX on: " + testGhost.name);
                vfx.PlayFoundEffect();
            }
            else
            {
                Debug.LogWarning("Test ghost has no VFX component!");
            }
        }
    }
    
    [ContextMenu("Manual VFX Test")]
    public void ManualVFXTest()
    {
        TestVFXIntegration();
    }
    
    [ContextMenu("Validate VFX System")]
    public void ValidateSystem()
    {
        ValidateVFXSystem();
    }
}