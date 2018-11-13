using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Skynet))]
public class SkynetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        Skynet myScript = (Skynet)target;
        if(GUILayout.Button("Next Generation"))
        {
            myScript.ForceNextGeneration();
        }
    }
}

