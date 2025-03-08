using System.Collections.Generic;
using UnityEngine;

public class AiBrain : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform deskPosition;
    [SerializeField] private List<Transform> waypoint;
    [SerializeField] private float walkSpeed = 1f;

    [SerializeField] private float minLookingDelay = 1f; // Minimum delay
    [SerializeField] private float maxLookingDelay = 3f; // Maximum delay

    [SerializeField] private float minStandDelay = 1f; // Minimum delay
    [SerializeField] private float maxStandDelay = 3f; // Maximum delay

    public enum Action { Sitting, Standing, WalkingToWaypoint, Looking, WalkingBackToDesk }
    #region Private Members
    private Action currentAction;
    private int waypointIndex = 0;
    private bool isMoving = false;
    private float actionTimer = 0f;
    private Vector3 targetPosition;
    #endregion



    private void Start()
    {
        Random.InitState(System.Environment.TickCount);
        currentAction = Action.Sitting;
        ChangeAnimation();
        actionTimer = GetRandomDelay();
        targetPosition = deskPosition.position;
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
                    MoveToWaypoint(waypointIndex);
                    break;

                case Action.Looking:
                    GameData.Instance.IsAILooking = true;
                    if (!GameData.Instance.IsPlaying)
                        StartWalkingBackToDesk();
                    break;

                case Action.WalkingBackToDesk:
                    GameData.Instance.IsAILooking = false;
                    MoveBackToDesk();
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
        currentAction = Action.WalkingToWaypoint;
        actionTimer = 0f; // No delay when starting to walk
        waypointIndex = GetRandomIndex();
        isMoving = true;
        LookTowards(waypoint[waypointIndex].position);
        ChangeAnimation();
    }

    private void MoveToWaypoint(int index)
    {
        if (!isMoving) return;
        transform.position = Vector3.MoveTowards(transform.position, waypoint[waypointIndex].position, walkSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoint[index].position) > 0.1f) return;
        if (waypoint[waypointIndex].name == "PlayerWaypoint")
        {
            isMoving = false;
            currentAction = Action.Looking;
            actionTimer = 0f; // No delay
            ChangeAnimation();
        }
        else
            StartWalkingBackToDesk();
    }

    public void KeyChallengeSuccess()
    {
        StartWalkingBackToDesk();
    }

    private void StartWalkingBackToDesk()
    {
        currentAction = Action.WalkingBackToDesk;
        actionTimer = 0f;
        targetPosition = deskPosition.position + new Vector3(0.1f, 0, 0.1f);
        isMoving = true;
        LookTowards(targetPosition);
        ChangeAnimation();
    }

    private void MoveBackToDesk()
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
    private int GetRandomIndex()
    {
        return Random.Range(0, waypoint.Count);
    }

}
