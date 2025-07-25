using System.Collections.Generic;
using UnityEngine;

public interface IPathfindingStrategy
{
    List<PathfindingNode> FindPath(PathfindingNode start, PathfindingNode end, PathfindingGrid grid);

    List<PathfindingNode> ReconstructPath(Dictionary<PathfindingNode, PathfindingNode> cameFrom, PathfindingNode start, PathfindingNode end);
    PathfindingAlgorithm Name { get => Name;}
}


public enum PathfindingAlgorithm
{
    Direct,
    BFS,
    DFS,
    AStar
}