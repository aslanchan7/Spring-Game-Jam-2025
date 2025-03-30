using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TMP_Text timerText;
    public float TimeToDisplay = 60f;
    private bool isRunning;
    public enum TimerFormat { Minutes_Seconds, Seconds_Milli }
    [SerializeField] private TimerFormat format;

    void Awake()
    {
        timerText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        TimerEventManager.TimerStart += EventManagerOnTimerStart;
        TimerEventManager.TimerStop += EventManagerOnTimerStop;
        TimerEventManager.TimerUpdate += EventManagerOnTimerUpdate;
    }

    private void OnDisable()
    {
        TimerEventManager.TimerStart -= EventManagerOnTimerStart;
        TimerEventManager.TimerStop -= EventManagerOnTimerStop;
        TimerEventManager.TimerUpdate -= EventManagerOnTimerUpdate;
    }


    private void EventManagerOnTimerStart() => isRunning = true;

    private void EventManagerOnTimerStop() => isRunning = false;

    private void EventManagerOnTimerUpdate(float value) => TimeToDisplay += value;

    void Update()
    {
        if (!isRunning) return;
        if (TimeToDisplay <= 0f) return;
        TimeToDisplay -= Time.deltaTime;

        TimeSpan timeSpan = TimeSpan.FromSeconds(TimeToDisplay);

        if (format == TimerFormat.Minutes_Seconds)
        {
            timerText.text = timeSpan.ToString(@"mm\:ss");
        }
        else if (format == TimerFormat.Seconds_Milli)
        {
            timerText.text = timeSpan.ToString(@"ss\:ff");
        }
    }
}
