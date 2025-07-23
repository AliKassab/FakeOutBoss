using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DFSPathfindingStrategy : IPathfindingStrategy
{
    public string Name => "DFS";
    public List<Vector3> FindPath(Vector3 start, Vector3 end, PathfindingGrid grid)
    {
        return new List<Vector3> { start, end };
    }
}
