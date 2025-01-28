using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossAI : MonoBehaviour
{
    public enum BossState { Sitting, Standing, Walking, Angry, Sleeping, Glancing }
    private BossState currentState;

    [SerializeField] private Animator animator;
    [SerializeField] private Transform deskPosition;
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private Transform player;
    [SerializeField] private float glancingDistance = 5f; // Distance to trigger glancing

    private Transform targetWaypoint;
    private bool isReturning = false;
    private bool isGlancing = false;

    private void Start()
    {
        ChangeState(BossState.Sitting);
    }

    private void Update()
    {
        HandleStateBehavior();
    }

    private void HandleStateBehavior()
    {
        switch (currentState)
        {
            case BossState.Sitting:
                if (Random.value < 0.002f) ChangeState(BossState.Standing);
                break;

            case BossState.Standing:
                if (Random.value < 0.01f) StartCoroutine(WalkRoutine());
                break;

            case BossState.Walking:
                MoveToTarget();
                break;

            case BossState.Sleeping:
                if (Random.value < 0.003f) ChangeState(BossState.Standing);
                break;
        }
    }

    private IEnumerator WalkRoutine()
    {
        ChangeState(BossState.Standing);
        yield return new WaitForSeconds(1.5f); // Wait before walking

        targetWaypoint = waypoints[Random.Range(0, waypoints.Count)];
        isReturning = false;
        isGlancing = Vector3.Distance(targetWaypoint.position, player.position) < glancingDistance;

        ChangeState(BossState.Walking);
    }

    private void MoveToTarget()
    {
        if (targetWaypoint == null) return;

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, walkSpeed * Time.deltaTime);

        if (isGlancing)
        {
            animator.Play("Glance");
            isGlancing = false; // Only glance once per walk cycle
        }

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            if (!isReturning)
            {
                StartCoroutine(ReturnToDeskRoutine());
            }
            else
            {
                ChangeState(BossState.Sitting);
            }
        }
    }

    private IEnumerator ReturnToDeskRoutine()
    {
        yield return new WaitForSeconds(2f); // Pause before returning
        targetWaypoint = deskPosition;
        isReturning = true;
        ChangeState(BossState.Walking);
    }

    private void ChangeState(BossState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        if (newState != BossState.Glancing) // Avoid overriding glancing
        {
            animator.Play(newState.ToString()); // Play matching animation
        }
    }
}
