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
    {
        IsAlmostSpotted = false;
        IsAILooking = false;
    }

    public bool IsPlaying = true;
    public bool IsAlmostSpotted = false;
    public bool IsAILooking = false;
}

