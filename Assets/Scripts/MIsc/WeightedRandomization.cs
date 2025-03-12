using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WeightedItem
{
    public IAiState state;
    public string animationName;
    public int weight;

    public WeightedItem(IAiState state, int weight)
    {
        this.state = state;
        this.weight = weight;
    }
}
public class WeightedRandom : MonoBehaviour
{
    public List<WeightedItem> items = new();

    public IAiState GetRandomState()
    {
        int totalWeight = 0;
        foreach (var item in items)
        {
            totalWeight += item.weight;
        }

        int randomValue = UnityEngine.Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (var item in items)
        {
            cumulativeWeight += item.weight;
            if (randomValue < cumulativeWeight)
            {
                return item.state;
            }
        }

        return null; // fallback
    }
}
