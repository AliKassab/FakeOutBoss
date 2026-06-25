using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode : MonoBehaviour
{
    public Vector3 position;
    public bool isWalkable = true;

    // Returns the neighbors of this node using the grid
    public List<PathfindingNode> GetNeighbors(PathfindingGrid grid)
    {
        List<PathfindingNode> neighbors = new List<PathfindingNode>();
        // Step by the grid's cell size, not a hardcoded 1, so neighbor positions
        // actually line up with placed nodes when cellSize != 1.
        float step = grid.cellSize;
        Vector3[] directions = new Vector3[]
        {
            Vector3.forward * step,
            Vector3.back * step,
            Vector3.left * step,
            Vector3.right * step
        };
        foreach (Vector3 dir in directions)
        {
            Vector3 neighborPos = position + dir;
            PathfindingNode neighbor = grid.GetNodeByPosition(neighborPos);
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }
        return neighbors;
    }
}
