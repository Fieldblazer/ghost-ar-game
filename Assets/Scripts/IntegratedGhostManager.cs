using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntegratedGhostManager : MonoBehaviour
{
    [Header("Ghost Settings")]
    public GameObject ghostPrefab;
    public int ghostCount = 5;
    
    [Header("UI References")]
    public TextMeshProUGUI ghostCounterText;
    
    [Header("Smart Spawning")]
    public SmartGhostSpawner smartSpawner;
    public bool useSmartSpawning = true;
    
    [Header("Integration")]
    public GhostManagerOcclusionBridge occlusionBridge;
    
    private List<GameObject> spawnedGhosts = new List<GameObject>();
    private int ghostsFound = 0;
    private Camera playerCamera;
    
    private void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
        
        if (smartSpawner == null)
            smartSpawner = FindObjectOfType<SmartGhostSpawner>();
        
        if (occlusionBridge == null)
            occlusionBridge = GetComponent<GhostManagerOcclusionBridge>();
        
        UpdateGhostCounter();
        Debug.Log("Integrated GhostManager ready with smart spawning");
    }
    
    public void SpawnAllGhosts()
    {
        Debug.Log("Spawning ghosts with smart positioning...");
        
        ClearAllGhosts();
        
        List<Vector3> spawnPositions = GetSpawnPositions();
        
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            GameObject newGhost = CreateGhost(spawnPositions[i], i);
            if (newGhost != null)
            {
                spawnedGhosts.Add(newGhost);
                
                if (occlusionBridge != null)
                {
                    occlusionBridge.OnGhostSpawned(newGhost);
                }
            }
        }
        
        Debug.Log($"Spawned {spawnedGhosts.Count} smart ghosts");
        UpdateGhostCounter();
    }
    
    private List<Vector3> GetSpawnPositions()
    {
        if (useSmartSpawning && smartSpawner != null)
        {
            smartSpawner.totalGhosts = ghostCount;
            return smartSpawner.GenerateSpawnPositions();
        }
        
        return GetFallbackPositions();
    }
    
    private List<Vector3> GetFallbackPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        Vector3 playerPos = playerCamera.transform.position;
        
        for (int i = 0; i < ghostCount; i++)
        {
            Vector3 randomDir = Random.insideUnitSphere;
            randomDir.y = 0;
            randomDir = randomDir.normalized;
            
            Vector3 pos = playerPos + randomDir * Random.Range(2f, 5f);
            pos.y = playerPos.y + Random.Range(0.5f, 3f);
            positions.Add(pos);
        }
        
        return positions;
    }
    
    private GameObject CreateGhost(Vector3 position, int index)
    {
        if (ghostPrefab == null)
        {
            Debug.LogError("Ghost prefab not assigned!");
            return null;
        }
        
        GameObject ghost = Instantiate(ghostPrefab, position, Quaternion.identity);
        ghost.name = $"SmartGhost_{index}";
        
        if (!ghost.CompareTag("Ghost"))
            ghost.tag = "Ghost";
        
        EnhancedGhostTapHandler tapHandler = ghost.GetComponent<EnhancedGhostTapHandler>();
        if (tapHandler == null)
        {
            GhostTapHandler oldHandler = ghost.GetComponent<GhostTapHandler>();
            if (oldHandler != null)
                DestroyImmediate(oldHandler);
            
            ghost.AddComponent<EnhancedGhostTapHandler>();
        }
        
        if (ghost.GetComponent<AudioSource>() == null)
            ghost.AddComponent<AudioSource>();
        
        return ghost;
    }
    
    public void GhostFound(GameObject ghost)
    {
        if (spawnedGhosts.Contains(ghost))
        {
            spawnedGhosts.Remove(ghost);
            ghostsFound++;
            
            if (occlusionBridge != null)
            {
                occlusionBridge.OnGhostDestroyed(ghost);
            }
            
            UpdateGhostCounter();
            Debug.Log($"Smart ghost found! Total: {ghostsFound}");
            
            if (spawnedGhosts.Count == 0)
            {
                Debug.Log("All smart ghosts found!");
            }
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
                if (occlusionBridge != null)
                {
                    occlusionBridge.OnGhostDestroyed(ghost);
                }
                DestroyImmediate(ghost);
            }
        }
        
        spawnedGhosts.Clear();
        ghostsFound = 0;
        UpdateGhostCounter();
    }
    
    [ContextMenu("Test Smart Spawning")]
    public void TestSmartSpawning()
    {
        SpawnAllGhosts();
    }
}