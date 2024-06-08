using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public static float SurvivalTime { get; private set; } // Static variable for survival time

    public GameObject rainGameObject;
    public GameObject DustGameObject;
    public GameObject rainAudioSource;

    public GameObject DemonTankSpawn;
    public GameObject DemonTankAnimation;

    public GameObject portal1;
    public GameObject portal2;
    public GameObject portal3;

    void Update()
    {
        SurvivalTime += Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.Floor(SurvivalTime).ToString();
        }

        if (SurvivalTime >= 10)
        {
            portal1.SetActive(true);
        }

        if (SurvivalTime >= 20f)
        {
            rainGameObject.SetActive(true);
            rainAudioSource.SetActive(true);
        }

        if (SurvivalTime >= 40)
        {
            portal2.SetActive(true);
        }



        if (SurvivalTime >= 100f)
        {
            DemonTankSpawn.SetActive(true);
            DemonTankAnimation.SetActive(false);
            DustGameObject.SetActive(true);
            portal3.SetActive(true);
        }

        

       
    }

    public void EndGame()
    {
        // Save the survival time to PlayerPrefs for persistence
        PlayerPrefs.Save();
    }

    public static void ResetSurvivalTime()
    {
        SurvivalTime = 0f;
    }
}
