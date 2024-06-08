using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestructionManager : MonoBehaviour
{
    public float breakForceThreshold = 5f;

    private List<Transform> wallChunks;
    private List<Rigidbody> chunkRigidbodies;

    void Start()
    {
        // Initialize lists
        wallChunks = new List<Transform>();
        chunkRigidbodies = new List<Rigidbody>();

        // Get all child chunks and add a Rigidbody to each
        foreach (Transform child in transform)
        {
            wallChunks.Add(child);
            Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true; // Initially, set Rigidbody to kinematic
            rb.mass = 0.1f;
            chunkRigidbodies.Add(rb);
        }
    }

    // Call this method to break a specific chunk
    public void BreakChunk(Transform chunk)
    {
        if (chunk != null && chunkRigidbodies.Contains(chunk.GetComponent<Rigidbody>()))
        {
            Rigidbody rb = chunk.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(Vector3.down * breakForceThreshold, ForceMode.Impulse); // Apply downward force
        }
    }
}
