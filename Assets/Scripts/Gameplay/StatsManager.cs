using System;
using UnityEngine;

public class StatsManager : SingletonMO<StatsManager>
{
    public event Action OnOverworkPressureMax;
    public event Action OnSuspicionMax;

    [SerializeField] private float overworkPressure = 0f;
    [SerializeField] private float suspicion = 0f;
    [SerializeField] private float maxValue = 100f;

    [SerializeField, Range(0, 100)] private float overworkIncreaseRate = 5f;
    [SerializeField, Range(0, 100)] private float suspicionIncreaseRate = 5f;
    [SerializeField, Range(0,100)] private float decayRate = 2f;

    public float OverworkPressureRatio => overworkPressure / maxValue;
    public float SuspicionRatio => suspicion / maxValue;

    private void Update()
    {
        bool isPlaying = GameData.Instance.IsPlaying;

        overworkPressure = Mathf.Clamp(overworkPressure + (isPlaying ? -decayRate : overworkIncreaseRate) * Time.deltaTime, 0, maxValue);
        suspicion = Mathf.Clamp(suspicion + (isPlaying ? suspicionIncreaseRate : -decayRate) * Time.deltaTime, 0, maxValue);

        if (overworkPressure >= maxValue) OnOverworkPressureMax?.Invoke();
        if (suspicion >= maxValue) OnSuspicionMax?.Invoke();
    }

}
