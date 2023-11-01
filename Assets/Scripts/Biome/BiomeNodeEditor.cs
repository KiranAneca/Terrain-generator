using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using XNodeEditor;
using static XNodeEditor.NodeEditor;

[CustomNodeEditor(typeof(BiomeNode))]
public class BiomeNodeEditor : NodeEditor
{
    private BiomeNode _baseNode;

    public override void OnBodyGUI()
    {
        if (_baseNode == null) _baseNode = target as BiomeNode;

        // Update serialized object's representation
        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_inputValue"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_transitionType"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_transitionVariable"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_transitionCondition"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_transitionValues"));    
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_biomeMat"));

        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }
}
