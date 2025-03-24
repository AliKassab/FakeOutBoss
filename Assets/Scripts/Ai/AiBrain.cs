// AiBrain.cs
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public partial class AiBrain : MonoBehaviour
{
    [SerializeField] private Transform originPosition;
    [SerializeField] private List<WaypointPath> waypointPaths;
    [SerializeField] public CharacterData characterData;

    public GameObject currentWaypoint;

    private Animator animator;
    private IAiState currentState;

    // Public properties for states to access
    public Transform OriginPosition => originPosition;
    public CharacterData Data => characterData;
    public List<WaypointPath> WaypointPaths => waypointPaths;
    public GameObject CurrentTarget { get; set; }
    public WaypointPath CurrentPath { get; set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
        Random.InitState(System.Environment.TickCount);
        ChangeState(new SittingState());
    }

    private void Update()
    {
        if (!GameData.Instance.IsGameActive) return;
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
