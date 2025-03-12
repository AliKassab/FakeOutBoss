// WalkingBackToOriginState.cs
using Unity.VisualScripting;
using UnityEngine;

public class WalkingBackToOriginState : IAiState
{
    private AiBrain aiBrain;
    private WaypointPath path;
    private int currentWaypointIndex;

    public WalkingBackToOriginState(WaypointPath path)
       => this.path = path;

    public void Enter(AiBrain aiBrain)
    {
        this.aiBrain = aiBrain;
        currentWaypointIndex = path.wayPoints.Length - 1;
        aiBrain.ChangeAnimation("Walking");
        MoveToPreviousWaypoint();
    }

    public void Exit() { }

    public void Update()
    {
        if (currentWaypointIndex >= 0)
            FollowPathBack();
        else
            ReturnToOrigin();
    }

    private void FollowPathBack()
    {
        Transform target = path.wayPoints[currentWaypointIndex];
        aiBrain.transform.position = Vector3.MoveTowards(aiBrain.transform.position, target.position, aiBrain.WalkSpeed * Time.deltaTime);

        if (Vector3.Distance(aiBrain.transform.position, target.position) <= 0.1f)
        {
            currentWaypointIndex--;
            MoveToPreviousWaypoint();
        }
    }

    private void ReturnToOrigin()
    {
        aiBrain.transform.position = Vector3.MoveTowards(aiBrain.transform.position, aiBrain.OriginPosition.position, aiBrain.WalkSpeed * Time.deltaTime);
        if (Vector3.Distance(aiBrain.transform.position, aiBrain.OriginPosition.position) <= 0.1f)
            aiBrain.ChangeState(new SittingState());
    }

    private void MoveToPreviousWaypoint()
    {
        if (currentWaypointIndex >= 0)
        {
            aiBrain.LookTowards(path.wayPoints[currentWaypointIndex].position);
            aiBrain.CurrentTarget = path.wayPoints[currentWaypointIndex].gameObject;
        }
        else
        {
            aiBrain.LookTowards(aiBrain.OriginPosition.position);
            aiBrain.CurrentTarget = aiBrain.OriginPosition.gameObject;
        }
    }
}