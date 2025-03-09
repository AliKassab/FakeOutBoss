using System.Collections.Generic;
using UnityEngine;

public class AiBrain : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform originPosition;
    [SerializeField] private List<WaypointPath> waypointPaths; // Changed to hold paths
    [SerializeField] private float walkSpeed = 1f;

    [SerializeField] private float minLookingDelay = 1f;
    [SerializeField] private float maxLookingDelay = 3f;

    [SerializeField] private float minStandDelay = 1f;
    [SerializeField] private float maxStandDelay = 3f;

    public enum Action { Sitting, Standing, WalkingToWaypoint, Looking, WalkingBackToDesk }
    #region Private Members
    private Action currentAction;
    private bool isMoving = false;
    private float actionTimer = 0f;
    private Vector3 targetPosition;
    private WaypointPath currentPath; // Track current path
    private int currentWaypointIndexInPath; // Track current waypoint in path
    #endregion

    private void Start()
    {
        Random.InitState(System.Environment.TickCount);
        currentAction = Action.Sitting;
        ChangeAnimation();
        actionTimer = GetRandomDelay();
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
                    MoveToWaypoint(); // Updated to handle path
                    break;

                case Action.Looking:
                    GameData.Instance.IsAILooking = true;
                    if (!GameData.Instance.IsPlaying)
                        StartWalkingBackToOrigin();
                    break;

                case Action.WalkingBackToDesk:
                    GameData.Instance.IsAILooking = false;
                    MoveBackToOrigin();
                    break;
            }
        }
    }

    private void StartStanding()
    {
        currentAction = Action.Standing;
        actionTimer = GetRandomDelayStanding();
        ChangeAnimation();
    }

    private void StartWalkingToWaypoint()
    {
        if (waypointPaths.Count == 0)
        {
            Debug.LogError("No waypoint paths assigned!");
            return;
        }

        currentAction = Action.WalkingToWaypoint;
        actionTimer = 0f;
        // Select a random path
        int pathIndex = Random.Range(0, waypointPaths.Count);
        currentPath = waypointPaths[pathIndex];
        currentWaypointIndexInPath = 0;
        isMoving = true;
        LookTowards(currentPath.wayPoints[currentWaypointIndexInPath].position);
        ChangeAnimation();
    }

    private void MoveToWaypoint()
    {
        if (!isMoving) return;

        // Check if current path is valid
        if (currentPath == null || currentWaypointIndexInPath >= currentPath.wayPoints.Length)
        {
            isMoving = false;
            return;
        }

        Transform targetWaypoint = currentPath.wayPoints[currentWaypointIndexInPath];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, walkSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) <= 0.1f)
        {
            // Check if there are more waypoints in the path
            if (currentWaypointIndexInPath < currentPath.wayPoints.Length - 1)
            {
                currentWaypointIndexInPath++;
                LookTowards(currentPath.wayPoints[currentWaypointIndexInPath].position);
            }
            else
            {
                // Reached the end of the path
                isMoving = false;
                if (targetWaypoint.name == "PlayerWaypoint")
                {
                    currentAction = Action.Looking;
                    actionTimer = 0f;
                    ChangeAnimation();
                }
                else
                {
                    StartWalkingBackToOrigin();
                }
            }
        }
    }

    public void KeyChallengeSuccess()
    {
        StartWalkingBackToOrigin();
    }

    private void StartWalkingBackToOrigin()
    {
        currentAction = Action.WalkingBackToDesk;
        actionTimer = 0f;
        targetPosition = originPosition.position + new Vector3(0.1f, 0, 0.1f);
        isMoving = true;
        LookTowards(targetPosition);
        ChangeAnimation();
    }

    private void MoveBackToOrigin()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, walkSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                currentAction = Action.Sitting;
                actionTimer = GetRandomDelay();
                ChangeAnimation();
            }
        }
    }

    private void LookTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.forward = new Vector3(direction.x, 0, direction.z);
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
            case Action.WalkingBackToDesk:
                animator.Play("Walking");
                break;
        }
    }

    private float GetRandomDelay()
    {
        return Random.Range(minLookingDelay, maxLookingDelay);
    }

    private float GetRandomDelayStanding()
    {
        return Random.Range(minStandDelay, maxStandDelay);
    }

}
