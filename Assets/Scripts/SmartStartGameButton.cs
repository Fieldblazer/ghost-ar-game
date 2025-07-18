using UnityEngine;
using UnityEngine.UI;

public class SmartStartGameButton : MonoBehaviour
{
    [Header("Smart Spawning Integration")]
    public GhostManager originalGhostManager;
    
    private Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
        
        if (originalGhostManager == null)
            originalGhostManager = FindObjectOfType<GhostManager>();
        
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(StartSmartGhostHunt);
        }
        
        Debug.Log("SmartStartGameButton configured");
    }
    
    public void StartSmartGhostHunt()
    {
        Debug.Log("=== SMART GHOST HUNT STARTED ===");
        
        // Hide the start button
        if (button != null)
            button.gameObject.SetActive(false);
        
        // Show the stop button
        GameObject stopButtonObj = GameObject.Find("StopGameButton");
        if (stopButtonObj != null)
        {
            stopButtonObj.SetActive(true);
            Debug.Log("Stop button activated");
        }
        
        // Try to find smart spawning manager
        Object smartManagerObj = FindObjectOfType(System.Type.GetType("SmartSpawningManager"));
        if (smartManagerObj != null)
        {
            Component smartManager = smartManagerObj as Component;
            if (smartManager != null)
            {
                // Use reflection to call the method
                smartManager.SendMessage("StartSmartSpawning", SendMessageOptions.DontRequireReceiver);
                Debug.Log("Smart spawning system activated");
            }
        }
        else
        {
            Debug.LogWarning("SmartSpawningManager not found, using original system");
            
            // Fallback to original system
            if (originalGhostManager != null)
            {
                originalGhostManager.SendMessage("SpawnAllGhosts", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
    
    [ContextMenu("Test Smart Hunt")]
    public void TestSmartHunt()
    {
        if (Application.isPlaying)
            StartSmartGhostHunt();
    }
}