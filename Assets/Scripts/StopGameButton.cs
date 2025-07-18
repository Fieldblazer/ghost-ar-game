using UnityEngine;
using UnityEngine.UI;

public class StopGameButton : MonoBehaviour
{
    private Button stopButton;
    private Button startButton;
    
    void Start()
    {
        stopButton = GetComponent<Button>();
        
        // Find the start button
        GameObject startObj = GameObject.Find("StartGameButton");
        if (startObj != null)
            startButton = startObj.GetComponent<Button>();
        
        if (stopButton != null)
        {
            stopButton.onClick.AddListener(StopGhostHunt);
        }
        
        Debug.Log("Stop button initialized");
    }
    
    public void StopGhostHunt()
    {
        Debug.Log("=== STOPPING GHOST HUNT ===");
        
        // Clear all spawned ghosts
        ClearAllGhosts();
        
        // Hide stop button, show start button
        if (stopButton != null)
            stopButton.gameObject.SetActive(false);
        
        if (startButton != null)
            startButton.gameObject.SetActive(true);
        
        Debug.Log("Ghost hunt stopped - returned to menu");
    }
    
    void ClearAllGhosts()
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        int clearedCount = 0;
        
        foreach (GameObject ghost in ghosts)
        {
            // Only clear spawned ghosts, not prefabs
            if (ghost.name.Contains("Smart") || ghost.name.Contains("Prefab") || ghost.name.Contains("Test"))
            {
                Destroy(ghost);
                clearedCount++;
            }
        }
        
        Debug.Log($"Cleared {clearedCount} ghosts from scene");
    }
}