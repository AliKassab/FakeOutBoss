using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "PathfindingAlgorithms", menuName = "PathfindingAlgorithms")]
public class PathfindingAlgorithms : ScriptableObject
{
    private static PathfindingAlgorithms _instance;

    public static PathfindingAlgorithms Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<PathfindingAlgorithms>("PathfindingAlgorithms");
                if (_instance == null)
                {
                    Debug.LogError("PathfindingAlgorithms not found in Resources folder!");
                }
            }
            return _instance;
        }
    }

    public Dictionary<PathfindingAlgorithm, IPathfindingStrategy> Strategies = new Dictionary<PathfindingAlgorithm, IPathfindingStrategy>();

    public List<PathfindingAlgorithm> strategiesList = new();

    public void SetAllStrategies()
    {
        foreach (KeyValuePair<PathfindingAlgorithm, IPathfindingStrategy> strategy in Strategies)
            strategiesList.Add(strategy.Key);
    }
    private void OnEnable()
    {
        Strategies.Clear();
        Strategies.Add(PathfindingAlgorithm.BFS, new BFSPathfindingStrategy());
        Strategies.Add(PathfindingAlgorithm.DFS, new DFSPathfindingStrategy());
        Strategies.Add(PathfindingAlgorithm.AStar, new AStarPathfindingStrategy());
        Strategies.Add(PathfindingAlgorithm.Direct, new DirectPathfindingStrategy());
        SetAllStrategies();
    }
}

