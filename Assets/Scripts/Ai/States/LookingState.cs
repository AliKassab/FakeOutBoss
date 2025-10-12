// LookingState.cs
using Unity.VisualScripting;
using UnityEngine;

public class LookingState : IAiState
{
    private AiBrain aiBrain;
    private float timer;

    public void Enter(AiBrain aiBrain)
    {
        this.aiBrain = aiBrain;
        this.timer = aiBrain.Data.LookingDuration;
        aiBrain.ChangeAnimation("Looking");
        GameData.Instance.IsAILooking = true;
    }

    public void Exit()
        => GameData.Instance.IsAILooking = false;

    public void Update()
    {
        timer -= Time.unscaledDeltaTime;

        // Exit conditions
        if (timer <= 0)
            GameData.Instance.IsSpotted = true;
        else if(!GameData.Instance.IsPlaying)
            aiBrain.ChangeState(new WalkOnPathState());
    }
}