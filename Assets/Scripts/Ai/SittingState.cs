// SittingState.cs
using Unity.VisualScripting;
using UnityEngine;

public class SittingState : IAiState
{
    private AiBrain aiBrain;
    private float timer;
    private WeightedRandom weightedRandom = new();
    public void Enter(AiBrain aiBrain)
    {
        this.aiBrain = aiBrain;
        timer = UnityEngine.Random.Range(aiBrain.Data.MinSittingDelay, aiBrain.Data.MaxSittingDelay);
        weightedRandom.items.Add(new WeightedItem(new StandingState(), aiBrain.Data.StandingStateWeight));
        weightedRandom.items.Add(new WeightedItem(new SittingAngryState(), aiBrain.Data.AngryStateWeight));
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