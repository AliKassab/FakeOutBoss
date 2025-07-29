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
        Vector3[] directions = new Vector3[]
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right
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
