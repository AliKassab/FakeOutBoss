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
        // Unscaled on purpose: the spot timer is real-time pressure even while the
        // key-challenge slow-mo (timeScale 0.05) is active.
        timer -= Time.unscaledDeltaTime;

        // Player stopped slacking (working again) or passed the challenge: the boss
        // saw nothing worth catching, so walk back to the desk (which then sits).
        if (!GameData.Instance.IsPlaying)
        {
            aiBrain.ChangeState(new WalkOnPathState(aiBrain.pathfindingPath.originNode));
            return;
        }

        // Time ran out while the player was still slacking: caught. Flag it and let
        // KeyChallengeManager resolve the fail. Stay put for this frame; the
        // IsGameActive guard in AiBrain halts the boss once the game ends.
        if (timer <= 0)
            GameData.Instance.IsSpotted = true;
    }
}