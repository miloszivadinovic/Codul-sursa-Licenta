using System.Collections;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Reference to the Enemy prefab
    public Transform spawnPoint;   // The point where the enemies will spawn
    public float spawnInterval = 5f; // Time interval between spawns
    public Transform player; // Reference to the player transform

    void Start()
    {
        // Start the coroutine to spawn enemies at regular intervals
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Wait for the specified interval
            yield return new WaitForSeconds(spawnInterval);

            // Spawn an enemy at the spawn point
            GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            // Get the FollowPlayerAI component and set the player as the target
            FollowPlayerAI followPlayerAI = spawnedEnemy.GetComponent<FollowPlayerAI>();

            if (followPlayerAI != null)
            {
                followPlayerAI.player = player;
            }
        }
    }
}
