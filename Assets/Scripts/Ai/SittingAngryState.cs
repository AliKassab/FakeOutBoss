// SittingState.cs
using System;
using Unity.VisualScripting;
using UnityEngine;

public class SittingAngryState : IAiState
{
    private AiBrain aiBrain;
    private float timer;
    private WeightedRandom weightedRandom = new();

    public void Enter(AiBrain aiBrain)
    {        

        this.aiBrain = aiBrain;
        timer = UnityEngine.Random.Range(aiBrain.Data.MinSittingDelay, aiBrain.Data.MaxSittingDelay);

        float suspicionRatio = StatsManager.Instance.SuspicionRatio;

        weightedRandom.items.Add(new WeightedItem(new StandingState(),() => aiBrain.Data.StandingStateWeight * suspicionRatio));
        weightedRandom.items.Add(new WeightedItem(new SittingState(), () => aiBrain.Data.SittingStateWeight * (1 - suspicionRatio)));

        aiBrain.ChangeAnimation("SittingAngry");
    }

    public void Exit() { }

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            aiBrain.ChangeState(weightedRandom.GetRandomState());
    }
}