using UnityEngine;

public class BeetleSpawner : MonoBehaviour
{
    public GameObject beetlePrefab;
    public Camera mainCamera;
    public float spawnDistanceFromScreenEdge = 1f;  // Distance from the screen edge where beetles should spawn
    public float minSpawnInterval = 5f;  // Minimum time between spawns
    public float maxSpawnInterval = 10f;  // Maximum time between spawns
    public int maxBeetlesOnScreen = 3;  // Maximum number of beetles allowed on screen

    private float spawnTimer;
    private float currentSpawnInterval;

    void Start()
    {
        // Initialize the spawn interval randomly between the min and max values
        currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= currentSpawnInterval)
        {
            if (CountBeetlesOnScreen() < maxBeetlesOnScreen)
            {
                int beetlesToSpawn = Random.Range(1, 3);  // Spawn 1 or 2 beetles
                for (int i = 0; i < beetlesToSpawn; i++)
                {
                    SpawnBeetle();
                }
            }

            spawnTimer = 0f;
            currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);  // Randomize the next spawn interval
        }
    }

    void SpawnBeetle()
    {
        // Get the screen bounds
        Vector3 screenBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));

        float screenLeft = screenBottomLeft.x*2;
        float screenRight = screenTopRight.x*2;
        float screenBottom = screenBottomLeft.y;
        float screenTop = screenTopRight.y;

        Vector3 spawnPosition = Vector3.zero;  // Initialize spawnPosition to a default value

        // Generate a spawn position outside the screen bounds
        bool isSpawned = false;

        while (!isSpawned)
        {
            float spawnX = 0f;
            float spawnY = 0f;

            // Randomly decide to spawn on the left, right, top, or bottom of the screen
            int edge = Random.Range(0, 4);
            switch (edge)
            {
                case 0: // Left
                    spawnX = screenLeft - spawnDistanceFromScreenEdge;
                    spawnY = Random.Range(screenBottom, screenTop);
                    break;
                case 1: // Right
                    spawnX = screenRight + spawnDistanceFromScreenEdge;
                    spawnY = Random.Range(screenBottom, screenTop);
                    break;
                case 2: // Top
                    spawnX = Random.Range(screenLeft, screenRight);
                    spawnY = screenTop + spawnDistanceFromScreenEdge;
                    break;
                case 3: // Bottom
                    spawnX = Random.Range(screenLeft, screenRight);
                    spawnY = screenBottom - spawnDistanceFromScreenEdge;
                    break;
            }

            spawnPosition = new Vector3(spawnX, spawnY, 0f);

            // Ensure the spawn position is outside the screen bounds
            if (IsOutsideScreenBounds(spawnPosition, screenLeft, screenRight, screenBottom, screenTop))
            {
                isSpawned = true;
            }
        }

        Instantiate(beetlePrefab, spawnPosition, Quaternion.identity);
    }

    bool IsOutsideScreenBounds(Vector3 position, float left, float right, float bottom, float top)
    {
        return position.x < left || position.x > right || position.y < bottom || position.y > top;
    }

    int CountBeetlesOnScreen()
    {
        return GameObject.FindGameObjectsWithTag("Beetle").Length;
    }
}
