// StandingState.cs
using Unity.VisualScripting;
using UnityEngine;

public class StandingState : IAiState
{
    private AiBrain aiBrain;
    private float timer;

    public void Enter(AiBrain aiBrain)
    {
        this.aiBrain = aiBrain;
        timer = Random.Range(aiBrain.Data.MinStandDelay, aiBrain.Data.MaxStandDelay);
        aiBrain.ChangeAnimation("Standing");
    }

    public void Exit() { }

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            aiBrain.ChangeState(new WalkingToWaypointState());
    }
}