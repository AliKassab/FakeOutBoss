using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AStarPathfindingStrategy : IPathfindingStrategy
{
    PathfindingAlgorithm Name { get => PathfindingAlgorithm.AStar; }

    public List<PathfindingNode> FindPath(PathfindingNode start, PathfindingNode end)
    {
        throw new NotImplementedException();
    }

    public List<PathfindingNode> ReconstructPath(Dictionary<PathfindingNode, PathfindingNode> cameFrom, PathfindingNode start, PathfindingNode end)
    {
        throw new NotImplementedException();
    }

    private float Heuristic(Vector3 a, Vector3 b) => Vector3.Distance(a, b);
}
