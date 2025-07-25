using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BFSPathfindingStrategy : IPathfindingStrategy
{
    PathfindingAlgorithm Name { get => PathfindingAlgorithm.BFS;}

    public List<PathfindingNode> FindPath(PathfindingNode start, PathfindingNode end, PathfindingGrid grid)
    {
        Queue<PathfindingNode> queue = new Queue<PathfindingNode>();
        HashSet<PathfindingNode> visited = new HashSet<PathfindingNode>();
        Dictionary<PathfindingNode, PathfindingNode> cameFrom = new Dictionary<PathfindingNode, PathfindingNode>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            PathfindingNode current = queue.Dequeue();
            if (current == end)
                return ReconstructPath(cameFrom, start, end);

            foreach (PathfindingNode neighbor in current.GetNeighbors(grid))
            {
                if (!visited.Contains(neighbor) && neighbor.IsWalkable == true)
                {
                    queue.Enqueue(neighbor);
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
