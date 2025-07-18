using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    private bool collected = false;
    private GhostManager ghostManager;

    void Start()
    {
        ghostManager = FindObjectOfType<GhostManager>();
    }

    public void Collect()
    {
        if (collected || !GameModeManager.Instance.isSeeking)
            return;

        collected = true;
        ghostManager.GhostFound();
        Destroy(gameObject);
    }
}
