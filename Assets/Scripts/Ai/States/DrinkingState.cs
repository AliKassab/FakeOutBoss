// SittingState.cs
using UnityEngine;

public class DrinkingState : IAiState
{
    private AiBrain aiBrain;
    private float timer;

    public void Enter(AiBrain aiBrain)
    {
        this.aiBrain = aiBrain;
        timer = Random.Range(aiBrain.Data.MinDrinkingDelay, aiBrain.Data.MaxDrinkingDelay);
        aiBrain.ChangeAnimation("Drinking");
    }

    public void Exit() { }

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            aiBrain.ChangeState(new WalkingBackToOriginState(aiBrain.CurrentPath));
    }
}