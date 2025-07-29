
using System.Collections.Generic;
using UnityEngine;

public class WalkOnPathState : IAiState
{
    private AiBrain aiBrain;
    private int pathIndex;

    public void Enter(AiBrain aiBrain)
    {
        this.aiBrain = aiBrain;
        // Ensure there are waypoints to walk to
        if (aiBrain.CurrentPath == null || aiBrain.CurrentPath.wayPoints == null || aiBrain.CurrentPath.wayPoints.Length == 0)
        {
            Debug.LogError("No waypoint paths assigned!");
            return;
        }

        PathfindingGrid grid = aiBrain.GetComponent<PathfindingGrid>();
        if (grid == null || !grid.IsInitialized)
        {
            Debug.LogError("PathfindingGrid component not found or not initialized on AiBrain GameObject.");
            return;
        }

        // Use the first and last waypoints as start/end for pathfinding
        Vector3 start = aiBrain.transform.position;
        Vector3 end = aiBrain.CurrentPath.wayPoints[aiBrain.CurrentPath.wayPoints.Length - 1].position;

        aiBrain.pathfindingPath.Initialize(aiBrain.pathfindingStrategy);
        pathIndex = 0;
        if (aiBrain.pathfindingPath.IsValid && aiBrain.pathfindingPath.Path.Count > 0)
            aiBrain.LookTowards(aiBrain.pathfindingPath.Path[pathIndex].position);
        aiBrain.ChangeAnimation("Walking");
    }

    public void Exit() { }

    public void Update()
    {
        if (aiBrain.pathfindingPath == null || !aiBrain.pathfindingPath.IsValid || aiBrain.pathfindingPath.Path.Count == 0 || pathIndex >= aiBrain.pathfindingPath.Path.Count)
            return;

        Vector3 target = aiBrain.pathfindingPath.Path[pathIndex].position;
        aiBrain.transform.position = Vector3.MoveTowards(aiBrain.transform.position, target, aiBrain.Data.WalkSpeed * Time.deltaTime);

        if (Vector3.Distance(aiBrain.transform.position, target) <= 0.1f)
        {
            pathIndex++;
            if (pathIndex < aiBrain.pathfindingPath.Path.Count)
            {
                aiBrain.LookTowards(aiBrain.pathfindingPath.Path[pathIndex].position);
            }
            else
            {
                if (aiBrain.CurrentPath.wayPoints[aiBrain.CurrentPath.wayPoints.Length - 1].name == "PlayerWaypoint")
                    aiBrain.ChangeState(new LookingState());
                else
                    aiBrain.ChangeState(new DrinkingState());
            }
        }
    }
}