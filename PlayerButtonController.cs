using UnityEngine;

public class PlayerButtonController : MonoBehaviour
{
    public float raycastRange = 2f; // Range within which the player can press the button
    public LayerMask buttonLayer; // Layer mask to filter raycast hits

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Cast a ray from the player's position forward
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            Debug.Log("Attempting raycast...");

            if (Physics.Raycast(ray, out hit, raycastRange, buttonLayer))
            {
                Debug.Log("Raycast hit: " + hit.collider.name);

                // Check if we hit the wave start button
                WaveManager waveManager = hit.collider.GetComponent<WaveManager>();
                if (waveManager != null)
                {
                    Debug.Log("Starting wave...");
                    waveManager.StartWave();
                }
                else
                {
                    Debug.Log("Hit object is not the wave start button.");
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything within range.");
            }
        }
    }
}
