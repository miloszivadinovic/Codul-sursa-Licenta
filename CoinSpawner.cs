using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab; // The coin prefab to spawn
    public Collider pavementCollider; // Reference to the pavement's box collider
    private GameObject currentCoin; // To keep track of the spawned coin

    void Start()
    {
        SpawnCoin();
    }

    void SpawnCoin()
    {
        if (currentCoin != null)
        {
            Destroy(currentCoin); // Ensure there's no existing coin
        }

        // Get the bounds of the pavement's box collider
        Bounds pavementBounds = pavementCollider.bounds;

        // Generate a random position within the pavement's bounds
        Vector3 randomPosition = new Vector3(
            Random.Range(pavementBounds.min.x, pavementBounds.max.x),
            pavementBounds.max.y + (coinPrefab.transform.localScale.y / 2), // Set the Y position to the pavement's surface
            Random.Range(pavementBounds.min.z, pavementBounds.max.z)
        );

        // Perform a raycast downward to find the exact position on the pavement surface
        if (Physics.Raycast(randomPosition, Vector3.down, out RaycastHit hit, 2f))
        {
            // Adjust the position to be exactly on the pavement surface
            randomPosition.y = hit.point.y + (coinPrefab.transform.localScale.y / 2);

            // Instantiate the coin at the calculated position
            currentCoin = Instantiate(coinPrefab, randomPosition, Quaternion.identity);
        }
    }

    // Call this method when you want to spawn a new coin (e.g., when the player collects the current one)
    public void RespawnCoin()
    {
        SpawnCoin();
    }
}
