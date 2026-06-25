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

    // The NPC's desk. Captured the first time it leaves so it can always walk back.
    [HideInInspector] public PathfindingNode originNode;

    public void Initialize(IPathfindingStrategy strategy, Vector3 aiPosition)
    {
        PathfindingNode startNode = currentNode = PathfindingGrid.Instance.GetNearestNode(aiPosition);
        if (originNode == null) originNode = startNode;

        IsValid = false;
        path.Clear();
        if (PathfindingGrid.Instance.IsInitialized && startNode != null && endNodes.Count > 0)
        {
            // Randomly pick one end node
            int idx = UnityEngine.Random.Range(0, endNodes.Count);
            PathfindingNode chosenEndNode = endNodes[idx];
            path = strategy.FindPath(startNode, chosenEndNode);
            IsValid = path != null && path.Count > 0;
        }
    }

    // Build a path to a specific node (used for the return trip to the desk).
    public void InitializeTo(IPathfindingStrategy strategy, Vector3 aiPosition, PathfindingNode target)
    {
        PathfindingNode startNode = currentNode = PathfindingGrid.Instance.GetNearestNode(aiPosition);
        if (originNode == null) originNode = startNode;

        IsValid = false;
        path.Clear();
        if (PathfindingGrid.Instance.IsInitialized && startNode != null && target != null)
        {
            path = strategy.FindPath(startNode, target);
            IsValid = path != null && path.Count > 0;
        }
    }

    public bool IsOnLookingNode() => currentNode == lookingNode;
    public bool IsOnDrinkingNode() => currentNode == drinkingNode;
}
