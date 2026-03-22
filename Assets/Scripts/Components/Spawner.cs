using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform dropPoint;
    public GameObject[] junkPrefabs;
    public int itemsPerSpawn = 5;

    [Header("Debug")]
    public KeyCode spawnKey = KeyCode.J;

    private void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            SpawnJunkPile();
        }
    }

    private void SpawnJunkPile()
    {
        if (dropPoint == null || junkPrefabs.Length == 0)
        {
            Debug.LogWarning("JunkSpawner: Missing dropPoint or junkPrefabs.");
            return;
        }

        for (int i = 0; i < itemsPerSpawn; i++)
        {
            GameObject prefab = junkPrefabs[Random.Range(0, junkPrefabs.Length)];

            // Slight random offset so items don't overlap perfectly
            Vector3 offset = new Vector3(
                Random.Range(-0.2f, 0.2f),
                Random.Range(0.1f, 0.3f),
                Random.Range(-0.2f, 0.2f)
            );

            Instantiate(prefab, dropPoint.position + offset, Random.rotation);
        }
    }
}
