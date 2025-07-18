using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Game Control")]
    public Button startButton;
    public Button stopButton;
    public bool gameInProgress = false;
    
    [Header("References")]
    public SmartSpawningManager spawningManager;
    public GhostManager ghostManager;
    
    void Start()
    {
        SetupButtons();
        SetInitialState();
    }
    
    void SetupButtons()
    {
        if (startButton == null)
        {
            GameObject startObj = GameObject.Find("StartGameButton");
            if (startObj != null)
                startButton = startObj.GetComponent<Button>();
        }
        
        if (spawningManager == null)
            spawningManager = FindObjectOfType<SmartSpawningManager>();
        if (ghostManager == null)
            ghostManager = FindObjectOfType<GhostManager>();
        
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(StartHunt);
        }
        
        if (stopButton != null)
        {
            stopButton.onClick.RemoveAllListeners();
            stopButton.onClick.AddListener(StopHunt);
        }
    }
    
    void SetInitialState()
    {
        gameInProgress = false;
        
        if (startButton != null)
            startButton.gameObject.SetActive(true);
        if (stopButton != null)
            stopButton.gameObject.SetActive(false);
    }
    
    public void StartHunt()
    {
        Debug.Log("=== STARTING GHOST HUNT ===");
        
        gameInProgress = true;
        
        if (startButton != null)
            startButton.gameObject.SetActive(false);
        if (stopButton != null)
            stopButton.gameObject.SetActive(true);
        
        if (spawningManager != null)
        {
            spawningManager.StartSmartSpawning();
        }
    }
    
    public void StopHunt()
    {
        Debug.Log("=== STOPPING GHOST HUNT ===");
        
        gameInProgress = false;
        
        ClearAllGhosts();
        
        if (startButton != null)
            startButton.gameObject.SetActive(true);
        if (stopButton != null)
            stopButton.gameObject.SetActive(false);
    }
    
    void ClearAllGhosts()
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in ghosts)
        {
            if (ghost.name.Contains("Smart") || ghost.name.Contains("Prefab"))
            {
                Destroy(ghost);
            }
        }
        Debug.Log("Cleared all ghosts from scene");
    }
    
    public void OnGhostFound()
    {
        Debug.Log("Ghost found!");
        
        // Check if all ghosts are found
        GameObject[] remainingGhosts = GameObject.FindGameObjectsWithTag("Ghost");
        int activeGhosts = 0;
        
        foreach (GameObject ghost in remainingGhosts)
        {
            if (ghost.activeInHierarchy)
                activeGhosts++;
        }
        
        if (activeGhosts <= 1) // This ghost will be destroyed soon
        {
            CompletedHunt();
        }
    }
    
    void CompletedHunt()
    {
        Debug.Log("🎉 ALL GHOSTS FOUND! Hunt completed!");
        
        gameInProgress = false;
        
        if (startButton != null)
            startButton.gameObject.SetActive(true);
        if (stopButton != null)
            stopButton.gameObject.SetActive(false);
    }
}