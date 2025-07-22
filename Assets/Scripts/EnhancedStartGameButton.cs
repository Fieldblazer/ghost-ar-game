using UnityEngine;
using UnityEngine.UI;

public class EnhancedStartGameButton : MonoBehaviour
{
    private Button startButton;
    private GhostManager ghostManager;
    private StopGameButton stopGameButton;
    
    void Start()
    {
        // Get components
        startButton = GetComponent<Button>();
        ghostManager = FindAnyObjectByType<GhostManager>();
        stopGameButton = FindAnyObjectByType<StopGameButton>();
        
        // Add click listener
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
    }
    
    public void StartGame()
    {
        // Start ghost spawning
        if (ghostManager != null)
        {
            ghostManager.SpawnAllGhosts();
        }
        
        // Hide start button
        gameObject.SetActive(false);
        
        // Show stop button
        if (stopGameButton != null)
        {
            stopGameButton.ShowStopButton();
        }
        
        Debug.Log("Game started - ghosts spawned, stop button shown");
    }
}