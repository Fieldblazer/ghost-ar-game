using UnityEngine;
using UnityEngine.UI;

public class StopGameButton : MonoBehaviour
{
    private Button stopButton;
    private GhostManager ghostManager;
    private GameObject startButton;
    
    void Start()
    {
        // Get components
        stopButton = GetComponent<Button>();
        ghostManager = FindAnyObjectByType<GhostManager>();
        startButton = GameObject.Find("StartGameButton");
        
        // Add click listener
        if (stopButton != null)
        {
            stopButton.onClick.AddListener(StopGame);
        }
        
        // Initially hide the stop button
        gameObject.SetActive(false);
    }
    
    public void StopGame()
    {
        // Clear all spawned ghosts
        ClearAllGhosts();
        
        // Reset ghost counter
        if (ghostManager != null)
        {
            ghostManager.ResetGhostCount();
        }
        
        // Show start button, hide stop button
        if (startButton != null)
        {
            startButton.SetActive(true);
        }
        gameObject.SetActive(false);
        
        Debug.Log("Game stopped - returned to main menu");
    }
    
    private void ClearAllGhosts()
    {
        // Find and destroy all ghosts in the scene
        GameObject[] allGhosts = GameObject.FindGameObjectsWithTag("Ghost");
        
        foreach (GameObject ghost in allGhosts)
        {
            if (ghost.name.Contains("Clone")) // Only destroy spawned ghosts, not prefabs
            {
                Destroy(ghost);
            }
        }
        
        Debug.Log($"Cleared {allGhosts.Length} ghosts from scene");
    }
    
    public void ShowStopButton()
    {
        gameObject.SetActive(true);
    }
}