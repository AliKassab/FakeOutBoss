using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BFSPathfindingStrategy : IPathfindingStrategy
{
    public string Name => "BFS";
    public List<Vector3> FindPath(Vector3 start, Vector3 end, PathfindingGrid grid)
    {
        // Implement BFS here
        // For now, just return direct path
        return new List<Vector3> { start, end };
    }
}
