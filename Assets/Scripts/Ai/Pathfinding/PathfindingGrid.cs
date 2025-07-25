using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    private Dictionary<Vector3, PathfindingNode> nodes = new Dictionary<Vector3, PathfindingNode>();

    public void CreateNode(Vector3 position)
    {
        if (!nodes.ContainsKey(position))
        {
            nodes[position] = new PathfindingNode();
        }
    }

    // Get all nodes as a list
    public List<PathfindingNode> GetAllNodes()
    {
        return new List<PathfindingNode>(nodes.Values);
    }

    // Get a node's position (reverse lookup)
    public Vector3? GetNodePosition(PathfindingNode node)
    {
        foreach (KeyValuePair<Vector3, PathfindingNode> kvp in nodes)
        {
            if (kvp.Value == node)
                return kvp.Key;
        }
        return null;
    }

    // Get a node by position
    public PathfindingNode GetNodeByPosition(Vector3 position)
    {
        PathfindingNode node;
        nodes.TryGetValue(position, out node);
        return node;
    }

    // Get neighbors of a node is now handled by PathfindingNode
}
