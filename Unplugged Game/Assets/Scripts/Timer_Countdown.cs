using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime = 300f;

    private bool isTimerRunning = true;

    void Update()
    {
        if (!isTimerRunning) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0;
            isTimerRunning = false;
            OnTimerEnd();
        }

        float displayTime = Mathf.Max(0, remainingTime);
        int minutes = Mathf.FloorToInt(displayTime / 60f);
        int seconds = Mathf.FloorToInt(displayTime % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnTimerEnd()
    {
        Debug.Log("Time ran out! Nexus AI Wins!.");

    }
}