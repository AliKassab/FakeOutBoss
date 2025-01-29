using UnityEngine;

public class BossAI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform deskPosition;
    [SerializeField] private Transform waypoint;  // Single waypoint
    [SerializeField] private float walkSpeed = 1f;

    [SerializeField] private float minLookingDelay = 1f; // Minimum delay
    [SerializeField] private float maxLookingDelay = 3f; // Maximum delay

    [SerializeField] private float minStandDelay = 1f; // Minimum delay
    [SerializeField] private float maxStandDelay = 3f; // Maximum delay

    public enum Action { Sitting, Standing, WalkingToWaypoint, Looking, WalkingBackToDesk }
    public Action currentAction;

    private bool isMoving = false; // Flag for movement
    private float actionTimer = 0f; // Timer to handle delays
    private Vector3 targetPosition;

    private void Start()
    {
        // Initialize random seed to be based on system tick count
        Random.InitState(System.Environment.TickCount);

        // Start the sequence from sitting
        currentAction = Action.Sitting;
        actionTimer = GetRandomDelay();
        targetPosition = deskPosition.position;
        ChangeAnimation();
    }

    private void Update()
    {
        // Handle each action step
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
                    StartWalkingBackToDesk();
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
        targetPosition = waypoint.position;
        isMoving = true;
        LookTowards(targetPosition);  // Look at the waypoint
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
                actionTimer = GetRandomDelay();
                ChangeAnimation();
            }
        }
    }

    private void StartWalkingBackToDesk()
    {
        currentAction = Action.WalkingBackToDesk;
        actionTimer = 0f; // No delay when walking back to the desk
        targetPosition = deskPosition.position;
        isMoving = true;
        LookTowards(targetPosition);  // Look at the desk
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
        transform.forward = new Vector3(direction.x, 0, direction.z);  // Make boss look at target
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

    // Generates a random delay between minStateDelay and maxStateDelay
    private float GetRandomDelay()
    {
        return Random.Range(minLookingDelay, maxLookingDelay);
    }
    private float GetRandomDelayStanding()
    {
        return Random.Range(minStandDelay, maxStandDelay);
    }

}
