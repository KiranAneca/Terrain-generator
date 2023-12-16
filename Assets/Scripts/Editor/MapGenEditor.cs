using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static XNodeEditor.NodeEditor;

[CustomNodeEditor(typeof(MapGen))]
public class MapGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Called whenever the inspector is drawn for this object.
        DrawDefaultInspector();
        //This draws the default screen.  You don't need this if you want
        //to start from scratch, but I use this when I'm just adding a button or
        //some small addition and don't feel like recreating the whole inspector.

        if (GUILayout.Button("Your ButtonText"))
        {

        }
    }
}
