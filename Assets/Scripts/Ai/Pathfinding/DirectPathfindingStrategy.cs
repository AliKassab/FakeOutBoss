using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DirectPathfindingStrategy : IPathfindingStrategy
{
    public string Name => "Direct";
    public List<Vector3> FindPath(Vector3 start, Vector3 end, PathfindingGrid grid)
    {
        return new List<Vector3> { start, end };
    }
}
