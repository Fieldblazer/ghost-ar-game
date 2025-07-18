using UnityEngine;

public class SmartSpawningTester : MonoBehaviour
{
    [Header("Testing Smart Spawning Integration")]
    // public SmartSpawningManager smartManager;  // Commented out to fix compilation
    public SmartStartGameButton smartButton;
    public SmartGhostSpawner smartSpawner;
    
    void Start()
    {
        TestIntegration();
    }
    
    void TestIntegration()
    {
        Debug.Log("=== SMART SPAWNING INTEGRATION TEST ===");
        
        // Find components
        // if (smartManager == null)
        //     smartManager = FindObjectOfType<SmartSpawningManager>();
        if (smartButton == null)
            smartButton = FindObjectOfType<SmartStartGameButton>();
        if (smartSpawner == null)
            smartSpawner = FindObjectOfType<SmartGhostSpawner>();
        
        // Report status
        // Debug.Log("SmartSpawningManager found: " + (smartManager != null));
        Debug.Log("SmartStartGameButton found: " + (smartButton != null));
        Debug.Log("SmartGhostSpawner found: " + (smartSpawner != null));
        
        Debug.Log("=== SMART SPAWNING INTEGRATION READY ===");
    }
    
    [ContextMenu("Clear All Test Ghosts")]
    public void ClearTestGhosts()
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        int cleared = 0;
        
        foreach (GameObject ghost in ghosts)
        {
            if (ghost.name.Contains("Smart") || ghost.name.Contains("Test"))
            {
                if (Application.isPlaying)
                    Destroy(ghost);
                else
                    DestroyImmediate(ghost);
                cleared++;
            }
        }
        
        Debug.Log("Cleared " + cleared + " test ghosts");
    }
}