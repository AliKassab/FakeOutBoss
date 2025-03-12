// AiBrain.cs
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class AiBrain : MonoBehaviour
{
    [SerializeField] private Transform originPosition;
    [SerializeField] private List<WaypointPath> waypointPaths;
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float minLookingDelay = 1f;
    [SerializeField] private float maxLookingDelay = 3f;
    [SerializeField] private float minStandDelay = 1f;
    [SerializeField] private float maxStandDelay = 3f;
    [SerializeField] private GameObject currentTarget;

    private Animator animator;
    private IAiState currentState;
    [SerializeField] private string currentStateName;

    // Public properties for states to access
    public Transform OriginPosition => originPosition;
    public List<WaypointPath> WaypointPaths => waypointPaths;
    public float WalkSpeed => walkSpeed;
    public float MinLookingDelay => minLookingDelay;
    public float MaxLookingDelay => maxLookingDelay;
    public float MinStandDelay => minStandDelay;
    public float MaxStandDelay => maxStandDelay;
    public GameObject CurrentTarget { get => currentTarget; set => currentTarget = value; }
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
        currentStateName = currentState.GetType().ToString();
        currentState.Enter(this);
    }

    public void LookTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.forward = new Vector3(direction.x, 0, direction.z);
    }

    public void ChangeAnimation(string animationName) => animator.Play(animationName);
}