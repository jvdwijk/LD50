using System.Collections;
using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] 
    private float timeOfSecond = 1;

    [SerializeField] 
    private GameObject gameOverPrefab;

    private Coroutine timer;

    private float timeSurvived = 0;
    
    public float TimeSurvived => timeSurvived;

    public event Action<float> OnGameFinish, OnTimeChanged;

    private void Start()
    {
        StartTimer();
    }

    private IEnumerator TimerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeOfSecond);
            timeSurvived += timeOfSecond;
            OnTimeChanged?.Invoke(timeSurvived);
        }
    }

    public void ResetTimeSurvived()
    {
        timeSurvived = 0;
    }

    public void StartTimer()
    {
        timer = StartCoroutine("TimerRoutine");
    }

    public void StopTimer()
    {
        if(timer != null) StopCoroutine(timer);
        
        gameOverPrefab.SetActive(true);
        OnGameFinish?.Invoke(timeSurvived);
    }
}