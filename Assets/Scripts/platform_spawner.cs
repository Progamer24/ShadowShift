using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] private GameObject[] platformPrefabs; // Array of platform prefabs
    [SerializeField] private Transform player; // Reference to the player
    [SerializeField] private float spawnDistance = 15f; // Distance ahead of player to spawn platforms
    [SerializeField] private float recycleDistance = 5f; // Distance behind player to recycle platforms
    [SerializeField] private int initialPoolSize = 10; // Initial number of platforms to pool
    [SerializeField] private float minPlatformSpacing = 2f; // Minimum space between platforms
    [SerializeField] private float maxPlatformSpacing = 5f; // Maximum space between platforms
    [SerializeField] private float unstableChance = 0.5f; // Chance to spawn an unstable platform

    private List<GameObject> platformPool = new List<GameObject>(); // Pool of platform objects
    private float lastSpawnZ; // Z-position of the last spawned platform

    private void Awake()
    {
        InitializePool();
        SpawnInitialPlatforms();
    }

    private void Update()
    {
        // Spawn new platforms as the player moves forward
        if (player.position.z + spawnDistance > lastSpawnZ)
        {
            SpawnPlatform();
        }

        // Recycle platforms that are behind the player
        RecyclePlatforms();
    }

    /// <summary>
    /// Initializes the platform object pool.
    /// </summary>
    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject platform = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)]);
            platform.SetActive(false);
            platformPool.Add(platform);
        }
    }

    /// <summary>
    /// Spawns the initial set of platforms.
    /// </summary>
    private void SpawnInitialPlatforms()
    {
        float currentZ = player.position.z;

        for (int i = 0; i < initialPoolSize; i++)
        {
            SpawnPlatform();
            currentZ += Random.Range(minPlatformSpacing, maxPlatformSpacing);
        }
    }

    /// <summary>
    /// Spawns a new platform from the pool.
    /// </summary>
    private void SpawnPlatform()
    {
        // Find an inactive platform in the pool
        GameObject platform = GetInactivePlatform();
        if (platform == null)
        {
            // If no inactive platforms are available, create a new one
            platform = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)]);
            platformPool.Add(platform);
        }

        // Position the platform
        float platformSpacing = Random.Range(minPlatformSpacing, maxPlatformSpacing);
        lastSpawnZ += platformSpacing;
        platform.transform.position = new Vector3(Random.Range(-3f, 3f), 0, lastSpawnZ);

        // Set platform type (stable or unstable)
        bool isUnstable = Random.value < unstableChance;
        platform.GetComponent<Platform>().SetUnstable(isUnstable);

        // Activate the platform
        platform.SetActive(true);
    }

    /// <summary>
    /// Recycles platforms that are behind the player.
    /// </summary>
    private void RecyclePlatforms()
    {
        for (int i = platformPool.Count - 1; i >= 0; i--)
        {
            GameObject platform = platformPool[i];
            if (platform.activeInHierarchy && platform.transform.position.z < player.position.z - recycleDistance)
            {
                platform.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Returns an inactive platform from the pool.
    /// </summary>
    private GameObject GetInactivePlatform()
    {
        foreach (GameObject platform in platformPool)
        {
            if (!platform.activeInHierarchy)
            {
                return platform;
            }
        }
        return null;
    }
}