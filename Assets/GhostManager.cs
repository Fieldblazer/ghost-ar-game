using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GhostManager : MonoBehaviour
{
    public TextMeshProUGUI ghostCounterText;
    public GameObject ghostPrefab;
    public int numberOfGhostsToSpawn = 5;
    public float spawnRadius = 3f;

    private int totalGhosts = 0;
    private int foundGhosts = 0;

    void Start()
    {
        UpdateCounter();
    }

    public void SpawnAllGhosts()
    {
        for (int i = 0; i < numberOfGhostsToSpawn; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);
            totalGhosts++;
        }

        UpdateCounter();
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Random position around the player in XZ plane
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection.y = 0; // Keep ghosts on ground level
        Vector3 playerPosition = Camera.main.transform.position;
        return playerPosition + randomDirection;
    }

    public void GhostFound()
    {
        foundGhosts++;
        UpdateCounter();
    }

    void UpdateCounter()
    {
        ghostCounterText.text = $"Ghosts Found: {foundGhosts} / {totalGhosts}";
    }
}
