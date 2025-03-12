// LookingState.cs
using Unity.VisualScripting;

public class LookingState : IAiState
{
    private AiBrain aiBrain;

    public void Enter(AiBrain aiBrain)
    {
        this.aiBrain = aiBrain;
        aiBrain.ChangeAnimation("Looking");
        GameData.Instance.IsAILooking = true;
    }

    public void Exit() => GameData.Instance.IsAILooking = false;

    public void Update()
    {
        if (!GameData.Instance.IsPlaying)
            aiBrain.ChangeState(new WalkingBackToOriginState(aiBrain.CurrentPath));
    }
}