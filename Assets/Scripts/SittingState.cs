// SittingState.cs
using Unity.VisualScripting;
using UnityEngine;

public class SittingState : IAiState
{
    private AiBrain aiBrain;
    private float timer;

    public void Enter(AiBrain aiBrain)
    {
        this.aiBrain = aiBrain;
        timer = Random.Range(aiBrain.MinSittingDelay, aiBrain.MaxSittingDelay);
        aiBrain.ChangeAnimation("Sitting");
    }

    public void Exit() { }

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            aiBrain.ChangeState(new StandingState());
    }
}