using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChallengesCompletionDataSaver))]
public class ChallengeDataSaverEditor : Editor
{ 
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        ChallengesCompletionDataSaver e = target as ChallengesCompletionDataSaver;
        if (GUILayout.Button("Clear"))
            e.ClearGameSaves();
    }
}
