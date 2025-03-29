using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TMP_Text timerText;
    [SerializeField] private float timeToDisplay = 60f;
    private bool isRunning;

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

    private void EventManagerOnTimerUpdate(float value) => timeToDisplay += value;

    void Update()
    {
        if (!isRunning) return;
        if (timeToDisplay <= 0f) return;
        timeToDisplay -= Time.deltaTime;

        TimeSpan timeSpan = TimeSpan.FromSeconds(timeToDisplay);
        timerText.text = timeSpan.ToString(@"mm\:ss\:ff");
    }
}
