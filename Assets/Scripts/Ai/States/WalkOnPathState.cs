
using System.Collections.Generic;
using UnityEngine;

public class WalkOnPathState : IAiState
{
    private AiBrain aiBrain;
    private int pathIndex;

    public void Enter(AiBrain aiBrain)
    {
        this.aiBrain = aiBrain;
        aiBrain.pathfindingPath.Initialize(aiBrain.pathfindingStrategy, aiBrain.transform.position);
                // Ensure there are waypoints to walk to
        if (aiBrain.pathfindingPath.Path == null || aiBrain.pathfindingPath.Path.Count == 0)
        {
            aiBrain.pathfindingPath.Initialize(new DirectPathfindingStrategy(), aiBrain.transform.position);
        }
        pathIndex = 0;
        if (aiBrain.pathfindingPath.IsValid && aiBrain.pathfindingPath.Path.Count > 0)
            aiBrain.LookTowards(aiBrain.pathfindingPath.Path[pathIndex].position);
        aiBrain.ChangeAnimation("Walking");
    }

    public void Exit() { }

    public void Update()
    {
        if (aiBrain.pathfindingPath == null
        || !aiBrain.pathfindingPath.IsValid
        || aiBrain.pathfindingPath.Path.Count == 0
        || pathIndex >= aiBrain.pathfindingPath.Path.Count)
            return;

        Vector3 target = aiBrain.pathfindingPath.Path[pathIndex].position;
        aiBrain.transform.position = Vector3.MoveTowards(aiBrain.transform.position, target, aiBrain.Data.WalkSpeed * Time.deltaTime);

        if (Vector3.Distance(aiBrain.transform.position, target) <= 0.1f)
        {
            aiBrain.pathfindingPath.currentNode = aiBrain.pathfindingPath.Path[pathIndex];
            pathIndex++;
            if (pathIndex < aiBrain.pathfindingPath.Path.Count)
                aiBrain.LookTowards(aiBrain.pathfindingPath.Path[pathIndex].position);
            else
            {
                if (aiBrain.pathfindingPath.IsOnLookingNode())
                    aiBrain.ChangeState(new LookingState());
                else if (aiBrain.pathfindingPath.IsOnDrinkingNode())
                    aiBrain.ChangeState(new DrinkingState());
            }
        }
    }
}