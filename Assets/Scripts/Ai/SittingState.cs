// SittingState.cs
using UnityEngine;

public class SittingState : IAiState
{
    private AiBrain aiBrain;
    private float timer;
    private WeightedRandom weightedRandom = new();
    public void Enter(AiBrain aiBrain)
    {
        this.aiBrain = aiBrain;
        timer = Random.Range(aiBrain.Data.MinSittingDelay, aiBrain.Data.MaxSittingDelay);

        float suspicionRatio = StatsManager.Instance.SuspicionRatio;

        weightedRandom.items.Add(new WeightedItem(new StandingState(), suspicionRatio));
        weightedRandom.items.Add(new WeightedItem(new SittingAngryState(), 1 - suspicionRatio));

        aiBrain.ChangeAnimation("Sitting");
    }

    public void Exit() { }

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            aiBrain.ChangeState(weightedRandom.GetRandomState());
    }
}