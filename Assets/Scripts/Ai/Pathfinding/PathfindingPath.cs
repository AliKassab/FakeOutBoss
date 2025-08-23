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
        if (originNode == null && startNode != null)
            originNode = startNode;

        chosenEndNode = null;
        IsValid = false;
        path.Clear();
        if (PathfindingGrid.Instance.IsInitialized && startNode != null && endNodes.Count > 0)
        {
            // Randomly pick one end node
            int idx = UnityEngine.Random.Range(0, endNodes.Count);
            chosenEndNode = endNodes[idx];
            path = strategy.FindPath(startNode, chosenEndNode);
            IsValid = path != null && path.Count > 0;
        }
    }

    public bool IsOnLookingNode() => currentNode == lookingNode;
    public bool IsOnDrinkingNode() => currentNode == drinkingNode;
}
