
using System.Collections.Generic;
using UnityEngine;

public class WalkOnPathState : IAiState
{
    private AiBrain aiBrain;
    private int pathIndex;

    // Non-null => walking back to the desk (origin), then sit.
    // Null      => outbound to a random end node (look / drink / plain).
    private readonly PathfindingNode returnTarget;
    private bool IsReturning => returnTarget != null;

    public WalkOnPathState() { }
    public WalkOnPathState(PathfindingNode returnTarget) => this.returnTarget = returnTarget;

    public void Enter(AiBrain aiBrain)
    {
        this.aiBrain = aiBrain;

        if (IsReturning)
            aiBrain.pathfindingPath.InitializeTo(aiBrain.pathfindingStrategy, aiBrain.transform.position, returnTarget);
        else
            aiBrain.pathfindingPath.Initialize(aiBrain.pathfindingStrategy, aiBrain.transform.position);

        pathIndex = 0;
        if (aiBrain.pathfindingPath.IsValid && aiBrain.pathfindingPath.path.Count > 0)
            aiBrain.LookTowards(aiBrain.pathfindingPath.path[pathIndex].position);
        aiBrain.ChangeAnimation("Walking");
    }

    public void Exit() { }

    public void Update()
    {
        PathfindingPath p = aiBrain.pathfindingPath;
        if (p == null || !p.IsValid || p.path.Count == 0 || pathIndex >= p.path.Count)
        {
            // No usable path (or already consumed it) — settle into Sitting instead
            // of freezing in place.
            aiBrain.ChangeState(new SittingState());
            return;
        }

        Vector3 target = p.path[pathIndex].position;
        aiBrain.target = target;
        aiBrain.transform.position = Vector3.MoveTowards(aiBrain.transform.position, target, aiBrain.Data.WalkSpeed * Time.deltaTime);

        if (Vector3.Distance(aiBrain.transform.position, target) <= 0.1f)
        {
            p.currentNode = p.path[pathIndex];
            pathIndex++;
            if (pathIndex < p.path.Count)
            {
                aiBrain.LookTowards(p.path[pathIndex].position);
                return;
            }

            // Reached the end of the path.
            if (IsReturning)
                aiBrain.ChangeState(new SittingState());               // back at desk -> sit
            else if (p.IsOnLookingNode())
                aiBrain.ChangeState(new LookingState());
            else if (p.IsOnDrinkingNode())
                aiBrain.ChangeState(new DrinkingState());
            else
                aiBrain.ChangeState(new WalkOnPathState(p.originNode)); // plain node -> walk back to desk
        }
    }
}
