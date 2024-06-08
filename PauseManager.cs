using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject weapon1;
    public SimplePlayerMovement playerMovementScript;
    public Beretta berettaScript;

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle pause state
            if (isPaused)
            {
                // Unpause the game
                ResumeGame();
            }
            else
            {
                // Pause the game
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        // Set time scale to 0 to pause the game
        Time.timeScale = 0f;
        isPaused = true;
        weapon1.SetActive(false);
        playerMovementScript.enabled = false;
        berettaScript.enabled = false; // Disable the Beretta script

        // Optionally, you can display a pause menu or do other UI-related actions here
    }

    void ResumeGame()
    {
        // Set time scale back to 1 to resume the game
        Time.timeScale = 1f;
        isPaused = false;
        weapon1.SetActive(true);
        playerMovementScript.enabled = true;
        berettaScript.enabled = true; // Enable the Beretta script

        // Optionally, you can hide the pause menu or revert UI changes here
    }
}
