using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DirectPathfindingStrategy : IPathfindingStrategy
{
    PathfindingAlgorithm Name { get => PathfindingAlgorithm.Direct; }

    public List<PathfindingNode> FindPath(PathfindingNode start, PathfindingNode end, PathfindingGrid grid)
    {
        throw new NotImplementedException();
    }

    public List<PathfindingNode> ReconstructPath(Dictionary<PathfindingNode, PathfindingNode> cameFrom, PathfindingNode start, PathfindingNode end)
    {
        throw new NotImplementedException();
    }
}
