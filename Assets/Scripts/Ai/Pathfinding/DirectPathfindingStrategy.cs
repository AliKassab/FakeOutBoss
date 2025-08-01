using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DirectPathfindingStrategy : IPathfindingStrategy
{
    PathfindingAlgorithm Name { get => PathfindingAlgorithm.Direct; }

    public List<PathfindingNode> FindPath(PathfindingNode start, PathfindingNode end)
    {
        var path = new List<PathfindingNode>();
        if (start != null) path.Add(start);
        if (end != null && end != start) path.Add(end);
        return path;
    }

    public List<PathfindingNode> ReconstructPath(Dictionary<PathfindingNode, PathfindingNode> cameFrom, PathfindingNode start, PathfindingNode end)
    {
        // For direct path, just return start and end
        return FindPath(start, end);
    }
}
