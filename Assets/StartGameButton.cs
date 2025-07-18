using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] private Button startButton; // <- Needs this
    [SerializeField] private GameObject ghostManagerObject; // <- And this

    private GhostManager ghostManager;

    void Awake()
    {
        ghostManager = ghostManagerObject.GetComponent<GhostManager>();
        startButton.onClick.AddListener(OnStartGame);
    }

    void OnStartGame()
    {
        ghostManager.SpawnAllGhosts();
        gameObject.SetActive(false); // hides the start button after click
    }
}
