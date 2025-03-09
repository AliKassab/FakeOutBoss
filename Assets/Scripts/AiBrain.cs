using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AiBrain : MonoBehaviour
{

    [SerializeField] private Transform originPosition;
    [SerializeField] private List<WaypointPath> waypointPaths; // Changed to hold paths
    [SerializeField] private float walkSpeed = 1f;

    [SerializeField] private float minLookingDelay = 1f;
    [SerializeField] private float maxLookingDelay = 3f;

    [SerializeField] private float minStandDelay = 1f;
    [SerializeField] private float maxStandDelay = 3f;

    [SerializeField] private GameObject currentTarget;

    public enum Action { Sitting, Standing, WalkingToWaypoint, Looking, WalkingBackToOrigin }
    #region Private Members
    private Animator animator;
    private Action currentAction;
    private bool isMoving = false;
    private float actionTimer = 0f;
    private Vector3 targetPosition;
    private WaypointPath currentPath;
    private int currentWaypointIndexInPath;
    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();
        Random.InitState(System.Environment.TickCount);
        SetStateSettings(GetRandomDelay(), Action.Sitting);
        targetPosition = originPosition.position;
    }

    private void Update()
    {
        if (!GameData.Instance.IsGameActive) return;
        actionTimer -= Time.deltaTime;

        if (actionTimer <= 0f)
        {
            switch (currentAction)
            {
                case Action.Sitting:
                    StartStanding();
                    break;

                case Action.Standing:
                    StartWalkingToWaypoint();
                    break;

                case Action.WalkingToWaypoint:
                    MoveToWaypoint();
                    break;

                case Action.Looking:
                    GameData.Instance.IsAILooking = true;
                    if (!GameData.Instance.IsPlaying)
                        StartWalkingBackToOrigin();
                    break;

                case Action.WalkingBackToOrigin:
                    GameData.Instance.IsAILooking = false;
                    MoveBackToOrigin();
                    break;
            }
        }
    }

    private void StartStanding() =>
        SetStateSettings(GetRandomDelayStanding(), Action.Standing);

    private void StartWalkingToWaypoint()
    {
        if (waypointPaths.Count == 0)
        {
            Debug.LogError("No waypoint paths assigned!");
            return;
        }

        int pathIndex = Random.Range(0, waypointPaths.Count);
        currentPath = waypointPaths[pathIndex];
        currentWaypointIndexInPath = 0;
        isMoving = true;
        LookTowards(currentPath.wayPoints[currentWaypointIndexInPath].position);
        SetStateSettings(0f, Action.WalkingToWaypoint);
    }

    private void MoveToWaypoint()
    {
        if (!isMoving) return;

        if (currentPath == null || currentWaypointIndexInPath >= currentPath.wayPoints.Length)
        {
            isMoving = false;
            return;
        }

        Transform targetWaypoint =  currentPath.wayPoints[currentWaypointIndexInPath];
        currentTarget = currentPath.wayPoints[currentWaypointIndexInPath].gameObject;
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, walkSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) <= 0.1f)
        {
            if (currentWaypointIndexInPath < currentPath.wayPoints.Length - 1)
            {
                currentWaypointIndexInPath++;
                LookTowards(currentPath.wayPoints[currentWaypointIndexInPath].position);
            }
            else
            {
                isMoving = false;
                if (targetWaypoint.name == "PlayerWaypoint")
                    SetStateSettings(0f, Action.Looking);
                else
                    StartWalkingBackToOrigin();
            }
        }
    }

    private void StartWalkingBackToOrigin()
    {
        // Start from the last waypoint index and move backward
        currentWaypointIndexInPath = currentPath.wayPoints.Length - 1;
        currentWaypointIndexInPath--; // Move to the previous waypoint

        if (currentWaypointIndexInPath >= 0)
        {
            currentTarget = currentPath.wayPoints[currentWaypointIndexInPath].gameObject;
            targetPosition = currentTarget.transform.position;
            LookTowards(targetPosition);
        }
        else
        {
            // If no more waypoints, move directly to origin
            currentTarget = originPosition.gameObject;
            targetPosition = originPosition.position;
            LookTowards(targetPosition);
        }

        isMoving = true;
        SetStateSettings(0f, Action.WalkingBackToOrigin);
    }

    private void MoveBackToOrigin()
    {
        if (isMoving)
        {
            if (currentWaypointIndexInPath >= 0)
            {
                // Move towards the current reversed waypoint
                Transform targetWaypoint = currentPath.wayPoints[currentWaypointIndexInPath];
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, walkSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetWaypoint.position) <= 0.1f)
                {
                    currentWaypointIndexInPath--;

                    if (currentWaypointIndexInPath >= 0)
                    {
                        currentTarget = currentPath.wayPoints[currentWaypointIndexInPath].gameObject;
                        targetPosition = currentTarget.transform.position;
                        LookTowards(targetPosition);
                    }
                    else
                    {
                        // All reversed waypoints visited, move to origin
                        currentTarget = originPosition.gameObject;
                        targetPosition = originPosition.position;
                        LookTowards(targetPosition);
                    }
                }
            }
            else
            {
                // Move towards the origin position
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, walkSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    isMoving = false;
                    SetStateSettings(GetRandomDelay(), Action.Sitting);
                }
            }
        }
    }

    private void LookTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.forward = new Vector3(direction.x, 0, direction.z);
    }

    private void SetStateSettings(float actionDelay, Action currentAction)
    {
        this.currentAction = currentAction;
        actionTimer = actionDelay;
        ChangeAnimation();
    }

    private void ChangeAnimation()
    {
        switch (currentAction)
        {
            case Action.Sitting:
                animator.Play("Sitting");
                break;
            case Action.Standing:
                animator.Play("Standing");
                break;
            case Action.WalkingToWaypoint:
                animator.Play("Walking");
                break;
            case Action.Looking:
                animator.Play("Looking");
                break;
            case Action.WalkingBackToOrigin:
                animator.Play("Walking");
                break;
        }
    }

    private float GetRandomDelay() 
        => Random.Range(minLookingDelay, maxLookingDelay);

    private float GetRandomDelayStanding() 
        => Random.Range(minStandDelay, maxStandDelay);
}
