using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WeightedItem
{
    public IAiState state;
    public string animationName;
    public int weight;
    public Func<float> weightFunc; // Dynamic weight function

    public WeightedItem(IAiState state, Func<float> weightFunc)
    {
        this.state = state;
        this.weightFunc = weightFunc;
    }
    public WeightedItem(IAiState state, int weight)
    {
        this.state = state;
        this.weight = weight;
    }
}

public class WeightedRandom
{
    public List<WeightedItem> items = new();

    public IAiState GetRandomState()
    {
        int totalWeight = 0;
        List<(IAiState state, int weight)> calculatedWeights = new();

        foreach (var item in items)
        {
            int weight = Mathf.Max(0, Mathf.RoundToInt(item.weightFunc()));
            totalWeight += weight;
            calculatedWeights.Add((item.state, weight));
        }

        if (totalWeight == 0) return null;

        int randomValue = UnityEngine.Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (var (state, weight) in calculatedWeights)
        {
            cumulativeWeight += weight;
            if (randomValue < cumulativeWeight)
            {
                return state;
            }
        }

        return null;
    }
}

