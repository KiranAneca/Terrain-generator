#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGen))]
public class TerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapGen func = (MapGen)target;

        if (GUILayout.Button("Generate map"))
        {
            func.GenerateMap();
        }

        //This draws the default screen.  You don't need this if you want
        //to start from scratch, but I use this when I'm just adding a button or
        //some small addition and don't feel like recreating the whole inspector.

        if (GUILayout.Button("Get all Float Nodes"))
        {
            func.GetAllFloatNodes();
        }
    }
}
#endif