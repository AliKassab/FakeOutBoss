using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathfindingPath
{
    private List<PathfindingNode> path = new List<PathfindingNode>();
    [SerializeField] private PathfindingNode startNode;
    [SerializeField] private List<PathfindingNode> endNodes = new List<PathfindingNode>();
    [SerializeField] private PathfindingNode chosenEndNode;
    public bool IsValid { get; private set; } = false;
    public IReadOnlyList<PathfindingNode> Path => path;

    public void Initialize(IPathfindingStrategy strategy)
    {
        endNodes.Clear();
        foreach (PathfindingNode end in endNodes)
        {
            if (end != null)
                endNodes.Add(end);
        }
        chosenEndNode = null;
        IsValid = false;
        path.Clear();
        if (PathfindingGrid.Instance != null && PathfindingGrid.Instance.IsInitialized && startNode != null && endNodes.Count > 0)
        {
            // Randomly pick one end node
            int idx = UnityEngine.Random.Range(0, endNodes.Count);
            chosenEndNode = endNodes[idx];
            path = strategy.FindPath(startNode, chosenEndNode);
            IsValid = path != null && path.Count > 0;
        }
    }
}
