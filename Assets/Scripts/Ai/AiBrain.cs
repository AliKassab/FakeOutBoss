// AiBrain.cs
using UnityEngine;

public partial class AiBrain : MonoBehaviour
{
    [SerializeField] public CharacterData characterData;
    public PathfindingAlgorithm pathfindingAlgorithm;
    public IPathfindingStrategy pathfindingStrategy;
    [SerializeField] public PathfindingPath pathfindingPath;
    public Vector3 target;
    private Animator animator;
    private IAiState currentState;
    public CharacterData Data => characterData;
    private void Start()
    {
        animator = GetComponent<Animator>();
        Random.InitState(System.Environment.TickCount);
        pathfindingStrategy = PathfindingAlgorithms.Instance.Strategies[pathfindingAlgorithm];

        ChangeState(new SittingState());

    }

    private void Update()
    {
        //if (!GameData.Instance.IsGameActive) return;
        currentState?.Update();
    }

    public void ChangeState(IAiState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter(this);
    }

    public void LookTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.forward = new Vector3(direction.x, 0, direction.z);
    }

    public void ChangeAnimation(string animationName) => animator.Play(animationName);
}
