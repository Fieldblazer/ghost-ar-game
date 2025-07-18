using UnityEngine;

public class SmartSpawningHelper : MonoBehaviour
{
    [Header("Configuration")]
    public SmartGhostSpawner smartSpawner;
    public GhostManager ghostManager;
    public GameObject ghostPrefab;
    public int ghostCount = 5;
    
    private void Start()
    {
        if (smartSpawner == null)
            smartSpawner = FindObjectOfType<SmartGhostSpawner>();
        if (ghostManager == null)
            ghostManager = FindObjectOfType<GhostManager>();
    }
    
    public void StartSmartSpawning()
    {
        Debug.Log("Starting smart ghost spawning...");
        
        if (smartSpawner != null && ghostPrefab != null)
        {
            for (int i = 0; i < ghostCount; i++)
            {
                Vector3 spawnPos = GetSmartSpawnPosition();
                Instantiate(ghostPrefab, spawnPos, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning("SmartGhostSpawner or ghost prefab not found!");
        }
    }
    
    private Vector3 GetSmartSpawnPosition()
    {
        Camera arCamera = Camera.main;
        Vector3 playerPos = arCamera.transform.position;
        
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(2f, 6f);
        
        Vector3 direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad), 
            0, 
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );
        
        Vector3 spawnPos = playerPos + direction * distance;
        spawnPos.y += Random.Range(0.5f, 2.0f);
        
        return spawnPos;
    }
}