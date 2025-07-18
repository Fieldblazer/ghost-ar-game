using UnityEngine;

public class GameCompletionManager : MonoBehaviour
{
    [Header("Game Completion Settings")]
    public int totalGhosts = 5;
    public bool autoCompleteWhenDone = true;
    
    private int ghostsFound = 0;
    private bool gameInProgress = false;
    
    void Start()
    {
        Debug.Log("GameCompletionManager initialized");
    }
    
    public void StartTracking(int ghostCount)
    {
        totalGhosts = ghostCount;
        ghostsFound = 0;
        gameInProgress = true;
        
        Debug.Log($"Started tracking game completion: {ghostsFound}/{totalGhosts} ghosts");
    }
    
    public void OnGhostFound()
    {
        if (!gameInProgress) return;
        
        ghostsFound++;
        Debug.Log($"Ghost found! Progress: {ghostsFound}/{totalGhosts}");
        
        if (autoCompleteWhenDone && ghostsFound >= totalGhosts)
        {
            CompleteGame();
        }
    }
    
    void CompleteGame()
    {
        gameInProgress = false;
        
        Debug.Log("🎉 ALL GHOSTS FOUND! Game completed!");
        
        // Show start button, hide stop button
        GameObject startButton = GameObject.Find("StartGameButton");
        GameObject stopButton = GameObject.Find("StopGameButton");
        
        if (startButton != null)
            startButton.SetActive(true);
        if (stopButton != null)
            stopButton.SetActive(false);
        
        // Could add completion effects here
        ShowCompletionMessage();
    }
    
    void ShowCompletionMessage()
    {
        // Simple debug message for now - could be enhanced with UI popup
        Debug.Log("Congratulations! You found all the ghosts!");
    }
    
    public void StopTracking()
    {
        gameInProgress = false;
        ghostsFound = 0;
        Debug.Log("Stopped tracking game completion");
    }
    
    // Public getters for UI updates
    public int GetGhostsFound() { return ghostsFound; }
    public int GetTotalGhosts() { return totalGhosts; }
    public float GetProgress() { return totalGhosts > 0 ? (float)ghostsFound / totalGhosts : 0f; }
    public bool IsGameInProgress() { return gameInProgress; }
}