using UnityEngine;
using TMPro;

public class GhostCounterManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI ghostCounterText;
    
    private int totalGhosts = 5;
    private int ghostsFound = 0;
    
    void Start()
    {
        // Find the ghost counter text if not assigned
        if (ghostCounterText == null)
        {
            ghostCounterText = GameObject.Find("GhostsFound_Count")?.GetComponent<TextMeshProUGUI>();
        }
        
        UpdateCounter();
    }
    
    public void GhostFound()
    {
        ghostsFound++;
        UpdateCounter();
        
        // Check if game is complete
        if (ghostsFound >= totalGhosts)
        {
            GameComplete();
        }
        
        Debug.Log($"Ghost found! Progress: {ghostsFound}/{totalGhosts}");
    }
    
    public void ResetCounter()
    {
        ghostsFound = 0;
        UpdateCounter();
        Debug.Log("Ghost counter reset");
    }
    
    public void SetTotalGhosts(int total)
    {
        totalGhosts = total;
        UpdateCounter();
    }
    
    private void UpdateCounter()
    {
        if (ghostCounterText != null)
        {
            ghostCounterText.text = $"{ghostsFound}/{totalGhosts}";
        }
    }
    
    private void GameComplete()
    {
        Debug.Log("🎉 All ghosts found! Game complete!");
        
        // Find and activate stop button to end game
        StopGameButton stopButton = FindAnyObjectByType<StopGameButton>();
        if (stopButton != null)
        {
            // Wait a moment then stop game
            Invoke("CompleteGame", 2f);
        }
    }
    
    private void CompleteGame()
    {
        StopGameButton stopButton = FindAnyObjectByType<StopGameButton>();
        if (stopButton != null)
        {
            stopButton.StopGame();
        }
    }
}