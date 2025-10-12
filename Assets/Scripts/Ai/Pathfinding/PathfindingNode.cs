using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode : MonoBehaviour
{
    public Vector3 position;
    public bool isWalkable = true;

    void Start()
    {
        // Ensure position is synced with transform position
        position = transform.position;
    }

    // Returns the neighbors of this node using the grid
    public List<PathfindingNode> GetNeighbors(PathfindingGrid grid)
    {
        List<PathfindingNode> neighbors = new List<PathfindingNode>();
        float cellSize = grid.cellSize;
        Vector3[] directions = new Vector3[]
        {
            Vector3.forward * cellSize,
            Vector3.back * cellSize,
            Vector3.left * cellSize,
            Vector3.right * cellSize
        };
        
        foreach (Vector3 dir in directions)
        {
            Vector3 neighborPos = position + dir;
            PathfindingNode neighbor = grid.GetNodeByPosition(neighborPos);
            if (neighbor != null && neighbor.isWalkable)
            {
                neighbors.Add(neighbor);
            }
        }
        return neighbors;
    }
}
