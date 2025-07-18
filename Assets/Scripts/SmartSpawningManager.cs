using UnityEngine;

public class SmartSpawningManager : MonoBehaviour
{
    public SmartGhostSpawner smartSpawner;
    public GhostManager ghostManager;
    public GameObject ghostPrefab;
    public int ghostCount = 5;
    public float minDistance = 1.5f;
    public float maxDistance = 6f;
    
    void Start()
    {
        if (smartSpawner == null)
            smartSpawner = FindObjectOfType<SmartGhostSpawner>();
        if (ghostManager == null)
            ghostManager = GetComponent<GhostManager>();
    }
    
    public void StartSmartSpawning()
    {
        Debug.Log("Starting Smart Ghost Spawning System");
        
        ClearOldGhosts();
        SpawnSmartGhosts();
    }
    
    void ClearOldGhosts()
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in ghosts)
        {
            if (ghost.name.Contains("Prefab"))
                Destroy(ghost);
        }
    }
    
    void SpawnSmartGhosts()
    {
        Camera arCamera = Camera.main;
        Vector3 playerPos = arCamera.transform.position;
        
        for (int i = 0; i < ghostCount; i++)
        {
            Vector3 spawnPos = GetSmartSpawnPosition(playerPos);
            CreateSmartGhost(spawnPos, i);
        }
        
        Debug.Log("Smart spawning completed: " + ghostCount + " ghosts");
    }
    
    Vector3 GetSmartSpawnPosition(Vector3 playerPos)
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float distance = Random.Range(minDistance, maxDistance);
        
        Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        Vector3 spawnPos = playerPos + direction * distance;
        spawnPos.y += Random.Range(0.5f, 2f);
        
        return spawnPos;
    }
    
    void CreateSmartGhost(Vector3 position, int index)
    {
        if (ghostPrefab != null)
        {
            GameObject newGhost = Instantiate(ghostPrefab, position, Quaternion.identity);
            newGhost.name = "SmartGhost_" + index;
            
            // Automatically upgrade with VFX system
            GhostVFXUpgrader vfxUpgrader = FindObjectOfType<GhostVFXUpgrader>();
            if (vfxUpgrader != null)
            {
                vfxUpgrader.UpgradeNewGhost(newGhost);
            }
            
            Debug.Log("Smart ghost with VFX created at: " + position);
        }
    }
}