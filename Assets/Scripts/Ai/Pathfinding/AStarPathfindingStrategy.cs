using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AStarPathfindingStrategy : IPathfindingStrategy
{
    PathfindingAlgorithm Name { get => PathfindingAlgorithm.AStar; }

    public List<PathfindingNode> FindPath(PathfindingNode start, PathfindingNode end)
    {
        var openSet = new List<PathfindingNode> { start };
        var cameFrom = new Dictionary<PathfindingNode, PathfindingNode>();
        var gScore = new Dictionary<PathfindingNode, float>();
        var fScore = new Dictionary<PathfindingNode, float>();
        var closedSet = new HashSet<PathfindingNode>();

        gScore[start] = 0f;
        fScore[start] = Heuristic(start.position, end.position);

        while (openSet.Count > 0)
        {
            // Get node with lowest fScore
            PathfindingNode current = openSet[0];
            float minF = fScore.ContainsKey(current) ? fScore[current] : float.MaxValue;
            foreach (var node in openSet)
            {
                float score = fScore.ContainsKey(node) ? fScore[node] : float.MaxValue;
                if (score < minF)
                {
                    minF = score;
                    current = node;
                }
            }

            if (current == end)
                return ReconstructPath(cameFrom, start, end);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbor in current.GetNeighbors(PathfindingGrid.Instance))
            {
                if (!neighbor.isWalkable || closedSet.Contains(neighbor))
                    continue;

                float tentativeG = gScore[current] + Vector3.Distance(current.position, neighbor.position);
                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeG >= (gScore.ContainsKey(neighbor) ? gScore[neighbor] : float.MaxValue))
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeG;
                fScore[neighbor] = tentativeG + Heuristic(neighbor.position, end.position);
            }
        }
        return new List<PathfindingNode>();
    }

    public List<PathfindingNode> ReconstructPath(Dictionary<PathfindingNode, PathfindingNode> cameFrom, PathfindingNode start, PathfindingNode end)
    {
        List<PathfindingNode> path = new List<PathfindingNode>();
        PathfindingNode current = end;
        while (current != start)
        {
            path.Add(current);
            if (!cameFrom.ContainsKey(current))
                return new List<PathfindingNode>(); // No path found
            current = cameFrom[current];
        }
        path.Add(start);
        path.Reverse();
        return path;
    }

    private float Heuristic(Vector3 a, Vector3 b) => Vector3.Distance(a, b);
}
