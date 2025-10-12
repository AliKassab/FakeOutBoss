using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathfindingPath
{
    [SerializeField] private List<PathfindingNode> endNodes = new List<PathfindingNode>();
    [SerializeField] private PathfindingNode lookingNode;
    [SerializeField] private PathfindingNode drinkingNode;
    
    public bool IsValid { get; private set; } = false;
    [HideInInspector] public PathfindingNode currentNode;

    public List<PathfindingNode> path = new List<PathfindingNode>();
    private PathfindingNode startNode;
    private PathfindingNode chosenEndNode;
    private PathfindingNode originNode;

    public void Initialize(IPathfindingStrategy strategy, Vector3 aiPosition)
    {
        startNode = currentNode = PathfindingGrid.Instance.GetNearestNode(aiPosition);
        
        // Set origin node if not already set
        if (originNode == null && startNode != null)
            originNode = startNode;

        chosenEndNode = null;
        IsValid = false;
        path.Clear();
        
        // Validate PathfindingGrid
        if (PathfindingGrid.Instance == null || !PathfindingGrid.Instance.IsInitialized)
        {
            Debug.LogWarning("PathfindingGrid not initialized");
            return;
        }
        
        // Validate start node
        if (startNode == null)
        {
            Debug.LogWarning($"Could not find valid start node for AI position: {aiPosition}");
            return;
        }
        
        // Check if start node is a special node (looking or drinking node)
        bool isStartingFromSpecialNode = (startNode == lookingNode || startNode == drinkingNode);
        
        if (isStartingFromSpecialNode && originNode != null && originNode != startNode)
        {
            // If starting from a special node, go back to origin
            chosenEndNode = originNode;
            Debug.Log($"Starting from special node, returning to origin at {originNode.position}");
        }
        else
        {
            // Normal behavior: validate end nodes and pick random destination
            if (endNodes.Count == 0)
            {
                Debug.LogWarning("No end nodes configured for pathfinding");
                return;
            }

            // Randomly pick one end node
            int idx = UnityEngine.Random.Range(0, endNodes.Count);
            chosenEndNode = endNodes[idx];
            Debug.Log($"Normal pathfinding to random end node at {chosenEndNode.position}");
        }
        
        if (chosenEndNode == null)
        {
            Debug.LogWarning("Selected end node is null");
            return;
        }
        
        // Find path
        path = strategy.FindPath(startNode, chosenEndNode);
        IsValid = path != null && path.Count > 0;
        
        if (!IsValid)
        {
            Debug.LogWarning($"Failed to find path from {startNode.position} to {chosenEndNode.position}");
        }
        else
        {
            Debug.Log($"Successfully found path with {path.Count} nodes from {startNode.position} to {chosenEndNode.position}");
        }
    }

    public bool IsOnLookingNode() => currentNode == lookingNode;
    public bool IsOnDrinkingNode() => currentNode == drinkingNode;
    public bool IsSpecialNode(PathfindingNode node) => node == lookingNode || node == drinkingNode;
    public PathfindingNode OriginNode => originNode;
}
