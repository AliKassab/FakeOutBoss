using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    public static PathfindingGrid Instance { get; private set; }
    private Dictionary<Vector3, PathfindingNode> nodes = new Dictionary<Vector3, PathfindingNode>();
    public bool IsInitialized { get; private set; } = false;

    [Header("Grid Settings")]
    public float cellSize = 1f;

    public float gizmosSize = 0.9f; // Size of the gizmos spheres

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple PathfindingGrid instances found! Destroying duplicate.");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    [ContextMenu("Initialize Grid")]
    public void InitializeGrid()
    {
        IsInitialized = false;
        nodes.Clear();
        int width = Mathf.RoundToInt(transform.localScale.x / cellSize);
        int height = Mathf.RoundToInt(transform.localScale.z / cellSize);
        Vector3 gridOrigin = transform.position - new Vector3((width * cellSize) / 2f, 0, (height * cellSize) / 2f);
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 pos = gridOrigin + new Vector3(x * cellSize, 0, z * cellSize);
                CreateNode(pos);
            }
        }
        IsInitialized = true;
    }

    public void CreateNode(Vector3 position)
    {
        if (!nodes.ContainsKey(position))
        {
            // Create a new GameObject for the node
            GameObject nodeObj = new GameObject($"Node_{position.x}_{position.z}");
            nodeObj.transform.parent = this.transform;
            nodeObj.transform.position = position;
            // Add a PathfindingNode component (must be a MonoBehaviour)
            PathfindingNode nodeComp = nodeObj.AddComponent<PathfindingNode>();
            nodeComp.position = position;
            nodeComp.isWalkable = true;
            nodes[position] = nodeComp;
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

    // Get the nearest node to a given position
    public PathfindingNode GetNearestNode(Vector3 position)
    {
        PathfindingNode nearest = null;
        float minDist = float.MaxValue;
        foreach (var node in nodes.Values)
        {
            float dist = Vector3.Distance(node.position, position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = node;
            }
        }
        return nearest;
    }

    // Visualize the grid in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (var kvp in nodes)
        {
            if (kvp.Value.isWalkable)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(kvp.Key, cellSize * gizmosSize);
        }
    }
}
