// WalkingToWaypointState.cs
using Unity.VisualScripting;
using UnityEngine;

public class WalkingToWaypointState : IAiState
{
    private AiBrain aiBrain;
    private WaypointPath currentPath;
    private int currentWaypointIndex;

    public void Enter(AiBrain aiBrain)
    {
        this.aiBrain = aiBrain;
        if (aiBrain.WaypointPaths.Count == 0)
            Debug.LogError("No waypoint paths assigned!");
        else
        {
            currentPath = aiBrain.WaypointPaths[Random.Range(0, aiBrain.WaypointPaths.Count)];
            currentWaypointIndex = 0;
            aiBrain.LookTowards(currentPath.wayPoints[currentWaypointIndex].position);
        }
        aiBrain.ChangeAnimation("Walking");
    }

    public void Exit() { }

    public void Update()
    {
        if (currentPath == null) return;

        Transform targetWaypoint = currentPath.wayPoints[currentWaypointIndex];
        aiBrain.transform.position = Vector3.MoveTowards(aiBrain.transform.position, targetWaypoint.position, aiBrain.WalkSpeed * Time.deltaTime);

        if (Vector3.Distance(aiBrain.transform.position, targetWaypoint.position) <= 0.1f)
        {
            if (currentWaypointIndex < currentPath.wayPoints.Length - 1)
            {
                currentWaypointIndex++;
                aiBrain.LookTowards(currentPath.wayPoints[currentWaypointIndex].position);
            }
            else
            {
                if (targetWaypoint.name == "PlayerWaypoint")
                    aiBrain.ChangeState(new LookingState());
                else
                    aiBrain.ChangeState(new WalkingBackToOriginState(currentPath));
            }
        }
    }
}