
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

        pathIndex = 0;
        if (aiBrain.pathfindingPath.IsValid && aiBrain.pathfindingPath.path.Count > 0)
            aiBrain.LookTowards(aiBrain.pathfindingPath.path[pathIndex].position);
        aiBrain.ChangeAnimation("Walking");
    }

    public void Exit() { }

    public void Update()
    {
        if (aiBrain.pathfindingPath == null
        || !aiBrain.pathfindingPath.IsValid
        || aiBrain.pathfindingPath.path.Count == 0
        || pathIndex >= aiBrain.pathfindingPath.path.Count)
            return;

        Vector3 target = aiBrain.pathfindingPath.path[pathIndex].position;
        aiBrain.target = target;
        aiBrain.transform.position = Vector3.MoveTowards(aiBrain.transform.position, target, aiBrain.Data.WalkSpeed * Time.deltaTime);

        if (Vector3.Distance(aiBrain.transform.position, target) <= 0.1f)
        {
            aiBrain.pathfindingPath.currentNode = aiBrain.pathfindingPath.path[pathIndex];
            pathIndex++;
            if (pathIndex < aiBrain.pathfindingPath.path.Count)
                aiBrain.LookTowards(aiBrain.pathfindingPath.path[pathIndex].position);
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