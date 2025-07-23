// IState.cs
public interface IAiState
{
    void Enter(AiBrain aiBrain);
    void Exit();
    void Update();
}