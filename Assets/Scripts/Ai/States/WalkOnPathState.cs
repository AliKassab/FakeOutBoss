
using System.Collections.Generic;
using UnityEngine;

public class WalkOnPathState : IAiState
{
    private AiBrain aiBrain;
    private List<PathfindingNode> path;
    private int pathIndex;

    public void Enter(AiBrain aiBrain)
    {
        this.aiBrain = aiBrain;
        // Example: pick a random path and use pathfinding to get a path from AI's position to the first waypoint
        if (path.Count == 0)
        {
            Debug.LogError("No waypoint paths assigned!");
            path = null;
            return;
        }

        Vector3 start = aiBrain.transform.position;
        Vector3 end = Vector3.zero; // Default end position
        // Get the grid reference from AiBrain or elsewhere as needed
        PathfindingGrid grid = aiBrain.GetComponent<PathfindingGrid>();
        if (grid == null)
        {
            Debug.LogError("PathfindingGrid component not found on AiBrain GameObject.");
            path = null;
            return;
        }
        path = aiBrain.pathfindingStrategy.FindPath(grid.GetNodeByPosition(start), grid.GetNodeByPosition(end), grid);
        pathIndex = 0;
        if (path != null && path.Count > 0)
            aiBrain.LookTowards(path[pathIndex].position);
        aiBrain.ChangeAnimation("Walking");
    }

    public void Exit() { }

    public void Update()
    {
        if (path == null || path.Count == 0 || pathIndex >= path.Count) return;

        Vector3 target = path[pathIndex].position;
        aiBrain.transform.position = Vector3.MoveTowards(aiBrain.transform.position, target, aiBrain.Data.WalkSpeed * Time.deltaTime);

        if (Vector3.Distance(aiBrain.transform.position, target) <= 0.1f)
        {
            pathIndex++;
            if (pathIndex < path.Count)
            {
                aiBrain.LookTowards(path[pathIndex].position);
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