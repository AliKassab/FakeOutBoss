using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform deskPosition;
    [SerializeField] private List<Transform> waypoint;  // Single waypoint
    [SerializeField] private float walkSpeed = 1f;

    [SerializeField] private float minLookingDelay = 1f; // Minimum delay
    [SerializeField] private float maxLookingDelay = 3f; // Maximum delay

    [SerializeField] private float minStandDelay = 1f; // Minimum delay
    [SerializeField] private float maxStandDelay = 3f; // Maximum delay
    public enum Action { Sitting, Standing, WalkingToWaypoint, Looking, WalkingBackToDesk }
    public Action currentAction;

    [SerializeField] private KeyChallengeManager keyChallengeManager; // Reference to KeyChallengeManager

    private bool isMoving = false;
    private float actionTimer = 0f;
    private Vector3 targetPosition;

    private void Start()
    {
        Random.InitState(System.Environment.TickCount);
        currentAction = Action.Sitting;
        actionTimer = GetRandomDelay();
        targetPosition = deskPosition.position;
        ChangeAnimation();
    }

    private void Update()
    {
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
                    if (!keyChallengeManager.IsChallengeActive()) 
                    {
                        keyChallengeManager.StartKeyChallenge();
                    }
                    break;

                case Action.WalkingBackToDesk:
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
        int index = GetRandomIndex();
        targetPosition = waypoint[index].position;
        isMoving = true;
        LookTowards(targetPosition);
        ChangeAnimation();
    }

    private void MoveToWaypoint()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, walkSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                currentAction = Action.Looking;
                actionTimer = 0f; // No delay
                ChangeAnimation();
            }
        }
    }

    

    public void KeyChallengeSuccess()
    {
        StartWalkingBackToDesk();
    }

    public void KeyChallengeFail()
    {
        Debug.Log("Player failed the challenge!");
        // Here you can add failure consequences if needed
    }

    private void StartWalkingBackToDesk()
    {
        currentAction = Action.WalkingBackToDesk;
        actionTimer = 0f;
        targetPosition = deskPosition.position;
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
                animator.Play("Look");
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
