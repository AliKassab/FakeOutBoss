using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KeyChallengeManager))]
public class KeyChallengeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        KeyChallengeManager manager = (KeyChallengeManager)target;
        if (GUILayout.Button("Bypass Challenge"))
        {
            manager.ByPassChallenge();
        }
    }
}