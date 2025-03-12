// AiBrain.cs
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public partial class AiBrain : MonoBehaviour
{
    [SerializeField] private Transform originPosition;
    [SerializeField] private List<WaypointPath> waypointPaths;

    private Animator animator;
    private IAiState currentState;

    // Public properties for states to access
    public Transform OriginPosition => originPosition;
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

public partial class AiBrain
{
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float lookingDuration = 1f;
    [SerializeField] private float minStandDelay = 1f;
    [SerializeField] private float maxStandDelay = 3f;
    [SerializeField] private float minSittingDelay = 1f;
    [SerializeField] private float maxSittingDelay = 3f;
    public float WalkSpeed => walkSpeed;
    public float LookingDuration => lookingDuration;
    public float MinStandDelay => minStandDelay;
    public float MaxStandDelay => maxStandDelay;
    public float MinSittingDelay => minSittingDelay;
    public float MaxSittingDelay => maxSittingDelay;
}