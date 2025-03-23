using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData")]
public class GameData : ScriptableObject
{
    private static GameData _instance;

    public static GameData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<GameData>("GameData");
                if (_instance == null)
                {
                    Debug.LogError("GameData not found in Resources folder!");
                }
            }
            return _instance;
        }
    }

    private void OnEnable()
       => ResetData();

    public void ResetData()
    {
        IsAILooking = false;
        IsPlaying = true;
        IsGameActive = false;
        IsSpotted = false;
    }

    public string ChallengeCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public bool IsPlaying = true;
    public bool IsAILooking = false;
    public bool IsGameActive = false;
    public bool IsSpotted = false;
    public int creditNamesIndex = 0;
    public static List<AnimationDelay> AnimationClips => Instance.animationDelays;

    [SerializeField] private List<AnimationDelay> animationDelays = new List<AnimationDelay>();
}
[Serializable]
public struct AnimationDelay
{
    public Animations name;
    public AnimationClip clip;
}

