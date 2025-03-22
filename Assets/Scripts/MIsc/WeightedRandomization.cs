using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WeightedItem
{
    public IAiState state;
    public string animationName;
    public float weight;

    public WeightedItem(IAiState state, float weight)
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
        float totalWeight = 0f;
        List<(IAiState state, float weight)> calculatedWeights = new();

        foreach (var item in items)
        {
            float weight = Mathf.Max(0f, item.weight);
            totalWeight += weight;
            calculatedWeights.Add((item.state, weight));
        }

        if (totalWeight == 0f) return null;

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

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

