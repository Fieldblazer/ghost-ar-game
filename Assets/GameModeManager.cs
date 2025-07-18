using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; }

    public bool isSeeking = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartSeeking()
    {
        isSeeking = true;
        Debug.Log("Seeker mode started!");
    }
}
