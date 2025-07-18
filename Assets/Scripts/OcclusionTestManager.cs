using UnityEngine;

public class OcclusionTestManager : MonoBehaviour
{
    [Header("Test Settings")]
    public GameObject testGhostPrefab;
    public int numberOfTestGhosts = 3;
    public float spawnRadius = 4f;
    
    [Header("References")]
    public GhostOcclusionIntegrator occlusionIntegrator;
    
    private void Start()
    {
        if (occlusionIntegrator == null)
            occlusionIntegrator = FindObjectOfType<GhostOcclusionIntegrator>();
    }
    
    [ContextMenu("Spawn Test Ghosts")]
    public void SpawnTestGhosts()
    {
        if (testGhostPrefab == null)
        {
            Debug.LogError("Test ghost prefab not assigned!");
            return;
        }
        
        Camera cam = Camera.main;
        Vector3 playerPos = cam.transform.position;
        
        for (int i = 0; i < numberOfTestGhosts; i++)
        {
            Vector3 spawnPos = playerPos + Random.insideUnitSphere * spawnRadius;
            spawnPos.y = playerPos.y + Random.Range(0.5f, 2f);
            
            GameObject ghost = Instantiate(testGhostPrefab, spawnPos, Quaternion.identity);
            ghost.name = $"TestGhost_{i}";
            
            if (occlusionIntegrator != null)
            {
                occlusionIntegrator.OnGhostSpawned(ghost);
            }
        }
        
        Debug.Log($"Spawned {numberOfTestGhosts} test ghosts for occlusion testing");
    }
    
    [ContextMenu("Clear Test Ghosts")]
    public void ClearTestGhosts()
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in ghosts)
        {
            if (occlusionIntegrator != null)
            {
                occlusionIntegrator.OnGhostDestroyed(ghost);
            }
            DestroyImmediate(ghost);
        }
        Debug.Log("Cleared all test ghosts");
    }
    
    [ContextMenu("Refresh Occlusion")]
    public void RefreshOcclusion()
    {
        if (occlusionIntegrator != null)
        {
            occlusionIntegrator.RefreshAllGhosts();
            Debug.Log("Refreshed ghost occlusion");
        }
    }
}