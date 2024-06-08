using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverSceneController : MonoBehaviour
{
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI survivalTimeText;

    void Start()
    {
        // Gets the killcount from playerprefs
        int enemyKillCount = PlayerPrefs.GetInt("EnemyKillCount", 0);

        // Displays nr of kills in UI
        killCountText.text = " " + enemyKillCount;

        // Gets the survival time from playerprefs
        float survivalTime = TimerManager.SurvivalTime;
        survivalTimeText.text = "Time Survived: " + Mathf.Floor(survivalTime).ToString() + " seconds";
    }
}
