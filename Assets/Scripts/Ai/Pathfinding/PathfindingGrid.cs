using System.Collections.Generic;
using System.Linq;
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
        FillNodes();
    }

    public void FillNodes()
    {
        IsInitialized = false;
        nodes.Clear();
        // Build manually instead of ToDictionary: two nodes sharing a position would
        // throw an ArgumentException and kill the whole grid. Last one wins, with a warning.
        foreach (PathfindingNode node in GetComponentsInChildren<PathfindingNode>())
        {
            if (nodes.ContainsKey(node.position))
                Debug.LogWarning($"Duplicate pathfinding node at {node.position}; overwriting.");
            nodes[node.position] = node;
        }
        IsInitialized = true;
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
        // Create a new GameObject for the node
        GameObject nodeObj = new GameObject($"Node_{position.x}_{position.z}");
        nodeObj.transform.parent = this.transform;
        nodeObj.transform.position = position;
        // Add a PathfindingNode component (must be a MonoBehaviour)
        PathfindingNode nodeComp = nodeObj.AddComponent<PathfindingNode>();
        nodeComp.position = position;
        nodeComp.isWalkable = true;
        // Register so the grid is actually usable right after InitializeGrid.
        nodes[position] = nodeComp;
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
        // Fast path: exact key match.
        if (nodes.TryGetValue(position, out PathfindingNode node))
            return node;

        // Fallback: float arithmetic on cellSize can drift by a few ulps so an
        // exact key miss doesn't mean "no node". Accept the nearest within a
        // quarter cell.
        float tol = cellSize * 0.25f;
        float tolSqr = tol * tol;
        foreach (var kvp in nodes)
        {
            if ((kvp.Key - position).sqrMagnitude <= tolSqr)
                return kvp.Value;
        }
        return null;
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
