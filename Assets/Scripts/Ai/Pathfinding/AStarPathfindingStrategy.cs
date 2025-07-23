using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AStarPathfindingStrategy : IPathfindingStrategy
{
    public string Name => "A*";
    public List<Vector3> FindPath(Vector3 start, Vector3 end, PathfindingGrid grid)
    {
        return new List<Vector3> { start, end };
    }
    private float Heuristic(Vector3 a, Vector3 b) => Vector3.Distance(a, b);
    private List<Vector3> ReconstructPath(Dictionary<Vector3, Vector3> cameFrom, Vector3 current)
    {
        var path = new List<Vector3> { current };
        return path;
    }
}
