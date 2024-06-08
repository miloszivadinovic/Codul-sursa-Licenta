using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject[] portals; // Array of portals
    public float[] waveDurations = { 60f, 100f, 100f, 100f, 250f }; // Duration for each wave
    public int totalWaves = 5; // Total number of waves

    private int currentWave = 0;
    private bool waveInProgress = false;

    void Start()
    {
        DisableAllPortals();
    }

    void DisableAllPortals()
    {
        foreach (GameObject portal in portals)
        {
            portal.SetActive(false);
        }
    }

    public void StartWave()
    {
        if (!waveInProgress && currentWave < totalWaves)
        {
            StartCoroutine(WaveRoutine());
        }
    }

    IEnumerator WaveRoutine()
    {
        waveInProgress = true;

        // Enable the appropriate number of portals for the current wave
        for (int i = 0; i <= currentWave; i++)
        {
            portals[i].SetActive(true);
        }

        // Wait for the wave duration
        yield return new WaitForSeconds(waveDurations[currentWave]);

        // Disable all portals after the duration
        foreach (GameObject portal in portals)
        {
            portal.SetActive(false);
        }

        currentWave++;
        waveInProgress = false;
    }
}
