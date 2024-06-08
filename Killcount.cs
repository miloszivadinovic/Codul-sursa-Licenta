using UnityEngine;
using TMPro;

public class KillCount : MonoBehaviour
{
    // Static variable to store the kill count
    public static int EnemyKillCount { get; private set; }
    public TextMeshProUGUI killsText;

    private static KillCount instance;

    void Start()
    {
        // Set the singleton instance
        instance = this;

        // Initialization of kill count to the value saved in PlayerPrefs, default is 0
        EnemyKillCount = PlayerPrefs.GetInt("EnemyKillCount", 0);

        ResetKillCount();

        // Update the UI text with the initial kill count
        UpdateKillCountText();
    }

    public static void IncreaseKillCount()
    {
        // Increase the kill count
        EnemyKillCount++;

        // Update the UI text
        UpdateKillCountText();

        // Saves kill count to PlayerPrefs
        PlayerPrefs.SetInt("EnemyKillCount", EnemyKillCount);

    }

    // Update the UI text with the current kill count
    private static void UpdateKillCountText()
    {
        // Check if the attached Text component exists
        if (instance != null && instance.killsText != null)
        {
            // Update the text with the current kill count
            instance.killsText.text = " " + EnemyKillCount;
        }
    }

    public static void ResetKillCount()
    {
        EnemyKillCount = 0;
    }
}
