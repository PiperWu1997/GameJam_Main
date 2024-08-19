using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject flyPrefab;
    public GameObject mothPrefab;
    public GameObject phantomPrefab;
    public Transform lamp;
    public float spawnRadius = 0.5f;
    public float spawnInterval = 5f; // Time between spawns
    public float minPrefabsInScene = 3f; // Minimum number of prefabs in the scene initially
    public float midPrefabsInScene = 10f; // Number of prefabs between 10s and 30s
    public float maxPrefabsInScene = 15f; // Maximum number of prefabs after 30s

    private float timeSinceLastSpawn;
    private float elapsedTime;
    private List<GameObject> prefabsInScene = new List<GameObject>();

    void Start()
    {
        timeSinceLastSpawn = 0f;
        elapsedTime = 0f;
        InitializePrefabs();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            MaintainPrefabCount();
            timeSinceLastSpawn = 0f;
        }
    }

    void InitializePrefabs()
    {
        // Initial spawn to reach minimum prefabs in scene
        for (int i = 0; i < minPrefabsInScene; i++)
        {
            SpawnPrefab();
        }
    }

    void MaintainPrefabCount()
    {
        int currentPrefabCount = GetCurrentPrefabCount();
        int requiredCount = GetRequiredPrefabCount();

        if (currentPrefabCount < requiredCount)
        {
            int prefabsToSpawn = requiredCount - currentPrefabCount;
            for (int i = 0; i < prefabsToSpawn; i++)
            {
                SpawnPrefab();
            }
        }
    }

    void SpawnPrefab()
    {
        GameObject prefab =new []{ flyPrefab, mothPrefab, phantomPrefab }[Random.Range(0, 3)] ;
        // GameObject prefab =phantomPrefab;

        Vector3 spawnPosition;
        do
        {
            float randomAngle = Random.Range(0f, 360f);
            float randomDistance = Random.Range(spawnRadius, 10f); // Adjust max distance if needed
            Vector3 offset = new Vector3(Mathf.Cos(randomAngle) * randomDistance, Mathf.Sin(randomAngle) * randomDistance, 0f);
            spawnPosition = lamp.position + offset;
        } while (Vector3.Distance(spawnPosition, lamp.position) <= spawnRadius);

        GameObject spawnedPrefab = Instantiate(prefab, spawnPosition, Quaternion.identity);
        prefabsInScene.Add(spawnedPrefab);
    }

    int GetCurrentPrefabCount()
    {
        // Remove any null references from the list
        prefabsInScene.RemoveAll(prefab => prefab == null);
        return prefabsInScene.Count;
    }

    int GetRequiredPrefabCount()
    {
        if (elapsedTime <= 10f)
        {
            return Mathf.RoundToInt(minPrefabsInScene);
        }
        else if (elapsedTime <= 30f)
        {
            return Mathf.RoundToInt(midPrefabsInScene);
        }
        else
        {
            return Mathf.RoundToInt(maxPrefabsInScene);
        }
    }
}
