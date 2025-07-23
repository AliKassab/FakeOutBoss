using System.Collections.Generic;
using UnityEngine;

public interface IPathfindingStrategy
{
    List<Vector3> FindPath(Vector3 start, Vector3 end, PathfindingGrid grid);
    string Name { get; }
}
