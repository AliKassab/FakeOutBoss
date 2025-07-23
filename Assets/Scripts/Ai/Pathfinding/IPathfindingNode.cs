using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode
{
    bool IsWalkable(Vector3 position) { return true; }
    List<Vector3> GetNeighbors(Vector3 position) { return new List<Vector3>();}
}
