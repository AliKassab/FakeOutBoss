using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DFSPathfindingStrategy : IPathfindingStrategy
{
    PathfindingAlgorithm Name { get => PathfindingAlgorithm.DFS; }

    public List<PathfindingNode> FindPath(PathfindingNode start, PathfindingNode end, PathfindingGrid grid)
    {
        Stack<PathfindingNode> stack = new Stack<PathfindingNode>();
        HashSet<PathfindingNode> visited = new HashSet<PathfindingNode>();
        Dictionary<PathfindingNode, PathfindingNode> cameFrom = new Dictionary<PathfindingNode, PathfindingNode>();

        stack.Push(start);
        visited.Add(start);

        while (stack.Count > 0)
        {
            PathfindingNode current = stack.Pop();
            if (current == end)
                return ReconstructPath(cameFrom, start, end);

            foreach (PathfindingNode neighbor in current.GetNeighbors(grid))
            {
                if (!visited.Contains(neighbor) && neighbor.IsWalkable == true)
                {
                    stack.Push(neighbor);
                    visited.Add(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }
        
        return new List<PathfindingNode> { start, end };
    }

    public List<PathfindingNode> ReconstructPath(Dictionary<PathfindingNode, PathfindingNode> cameFrom, PathfindingNode start, PathfindingNode end)
    {
        List<PathfindingNode> path = new List<PathfindingNode>();
        PathfindingNode current = end;

        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Add(start);
        path.Reverse();
        return path;
    }
}
