using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnhancedGhostManager : MonoBehaviour
{
    [Header("Ghost Spawning")]
    public GameObject ghostPrefab;
    public int ghostCount = 5;
    public float spawnRadius = 5f;
    public float spawnHeightMin = 0.5f;
    public float spawnHeightMax = 3f;
    
    [Header("UI References")]
    public TextMeshProUGUI ghostCounterText;
    
    [Header("Occlusion Integration")]
    public GhostOcclusionIntegrator occlusionIntegrator;
    
    private List<GameObject> spawnedGhosts = new List<GameObject>();
    private int ghostsFound = 0;
    private Camera playerCamera;
    
    private void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
        
        if (occlusionIntegrator == null)
            occlusionIntegrator = GetComponent<GhostOcclusionIntegrator>();
        
        UpdateGhostCounter();
    }
    
    public void SpawnAllGhosts()
    {
        if (ghostPrefab == null)
        {
            Debug.LogError("Ghost prefab is not assigned!");
            return;
        }
        
        ClearAllGhosts();
        Vector3 playerPosition = playerCamera.transform.position;
        
        for (int i = 0; i < ghostCount; i++)
        {
            Vector3 spawnPosition = GenerateSpawnPosition(playerPosition);
            GameObject newGhost = SpawnGhost(spawnPosition, i);
            
            if (newGhost != null)
            {
                spawnedGhosts.Add(newGhost);
                
                if (occlusionIntegrator != null)
                {
                    occlusionIntegrator.OnGhostSpawned(newGhost);
                }
            }
        }
        
        Debug.Log($"Spawned {spawnedGhosts.Count} ghosts with occlusion support");
        UpdateGhostCounter();
    }
    
    private Vector3 GenerateSpawnPosition(Vector3 playerPosition)
    {
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0;
        randomDirection = randomDirection.normalized;
        
        Vector3 spawnPosition = playerPosition + (randomDirection * Random.Range(2f, spawnRadius));
        spawnPosition.y = playerPosition.y + Random.Range(spawnHeightMin, spawnHeightMax);
        
        return spawnPosition;
    }
    
    private GameObject SpawnGhost(Vector3 position, int ghostIndex)
    {
        GameObject ghost = Instantiate(ghostPrefab, position, Quaternion.identity);
        ghost.name = $"Ghost_{ghostIndex}";
        
        if (!ghost.CompareTag("Ghost"))
        {
            ghost.tag = "Ghost";
        }
        
        return ghost;
    }
    
    public void GhostFound(GameObject ghost)
    {
        if (spawnedGhosts.Contains(ghost))
        {
            spawnedGhosts.Remove(ghost);
            ghostsFound++;
            
            if (occlusionIntegrator != null)
            {
                occlusionIntegrator.OnGhostDestroyed(ghost);
            }
            
            UpdateGhostCounter();
            Debug.Log($"Ghost found! Total found: {ghostsFound}");
        }
    }
    
    private void UpdateGhostCounter()
    {
        if (ghostCounterText != null)
        {
            ghostCounterText.text = $"{ghostsFound}";
        }
    }
    
    public void ClearAllGhosts()
    {
        foreach (GameObject ghost in spawnedGhosts)
        {
            if (ghost != null)
            {
                if (occlusionIntegrator != null)
                {
                    occlusionIntegrator.OnGhostDestroyed(ghost);
                }
                DestroyImmediate(ghost);
            }
        }
        
        spawnedGhosts.Clear();
        ghostsFound = 0;
        UpdateGhostCounter();
    }
}