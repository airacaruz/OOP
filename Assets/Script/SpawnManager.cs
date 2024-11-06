using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] prefabs;
    private GameObject[] spawnedObjects;
    private Vector3[] originalPositions;
    public float spawnInterval = 0.5f;
    public int numberOfSpawns = 3;

    private Coroutine spawnCoroutine; // Reference to manage the coroutine
    private GameManager gameManager;  // Reference to GameManager

    void Awake()
    {
        // Initialize arrays without starting spawning automatically
        spawnedObjects = new GameObject[prefabs.Length];
        originalPositions = new Vector3[prefabs.Length];

        // Store original positions of each prefab
        for (int i = 0; i < prefabs.Length; i++)
        {
            originalPositions[i] = prefabs[i].transform.position;
        }

        // Find the GameManager in the scene
        gameManager = FindObjectOfType<GameManager>();
    }

    public void StartSpawning()
    {
        // Start the spawn coroutine
        spawnCoroutine = StartCoroutine(SpawnObjects());
    }

    public void StopSpawning()
    {
        // Stop the spawn coroutine if it's running
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        // Optionally, destroy all spawned objects here if needed
        foreach (var obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            // If the game is over, stop spawning
            if (gameManager != null && !gameManager.isGameActive)
            {
                yield break; // Exit the coroutine if the game is not active
            }

            for (int i = 0; i < numberOfSpawns; i++)
            {
                for (int j = 0; j < prefabs.Length; j++)
                {
                    spawnedObjects[j] = Instantiate(prefabs[j], originalPositions[j], Quaternion.identity);
                }
                yield return new WaitForSeconds(spawnInterval);
            }

            while (spawnedObjects[spawnedObjects.Length - 1] != null)
            {
                yield return null;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
