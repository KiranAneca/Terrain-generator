using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using XNodeEditor;
using static XNodeEditor.NodeEditor;
using static System.TimeZoneInfo;
using System;

[CustomNodeEditor(typeof(BiomeNode))]
public class BiomeNodeEditor : NodeEditor
{
    private BiomeNode _baseNode;
    private bool _showCondition = false;
    private bool _showVisuals = false;

    public override Color GetTint()
    {
        if (_baseNode == null) _baseNode = target as BiomeNode;

        if (_baseNode == null) return base.GetTint();

        if (_baseNode.GetTransitionValues().Count != 0)
        {
            return base.GetTint() / 1.2f;
        }
        return base.GetTint();
    }

    public override void OnBodyGUI()
    {
        if (_baseNode == null) _baseNode = target as BiomeNode;

        if (_baseNode == null) return;

        // Update serialized object's representation
        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_inputValue"));

        _showVisuals = EditorGUILayout.Foldout(_showVisuals, "Visuals");
        if (_showVisuals)
        {
            DrawVisuals();
        }

        _showCondition = EditorGUILayout.Foldout(_showCondition, "Condition");
        if (_showCondition)
        {
            DrawTransitionCondition();
        }

        if (_baseNode.GetTransitionValues().Count != 0)
        {
            //NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_transitionType"));
            //NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_transitionVariable"));
            //NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_transitionCondition"));
        }

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_transitionValues"));



        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawVisuals()
    {
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_biomeMat"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_extraVisual"));
    }

    private void DrawTransitionCondition()
    {
        GUILayout.BeginVertical("HelpBox");

        // Transition Type
        GUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 5;
        EditorGUILayout.LabelField("Type");
        EditorGUIUtility.labelWidth = 0;
        int selectedTransitionType = EditorGUILayout.Popup(_baseNode._condition.TransitionTypesList.IndexOf(_baseNode._condition.TransitionType), _baseNode._condition.TransitionTypesList.ToArray());
        _baseNode._condition.TransitionType = _baseNode._condition.TransitionTypesList[selectedTransitionType];
        GUILayout.EndHorizontal();

        var type = Utility.GetType(_baseNode._condition.TransitionType);
        if(type != null) 
        {
            // Transition variable
            GUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 5;
            EditorGUILayout.LabelField("Variable");
            EditorGUIUtility.labelWidth = 0;
            if (type.IsDefined(typeof(BoolTrans), false)) // Draw the bool variables
            {
                int selectedTransitionVariable = EditorGUILayout.Popup(_baseNode._condition.GetTransitionBoolVariables().IndexOf(_baseNode._condition.TransitionVariable), _baseNode._condition.GetTransitionBoolVariables().ToArray());
                if(selectedTransitionVariable < 0) { selectedTransitionVariable = 0; } // If the type changed, the index will retun -1 and needs to be acounted for
                _baseNode._condition.TransitionVariable = _baseNode._condition.GetTransitionBoolVariables()[selectedTransitionVariable];

            }
            else if(type.IsDefined(typeof(FloatTrans), false)) // Draw the float variables
            {
                int selectedTransitionVariable = EditorGUILayout.Popup(_baseNode._condition.GetTransitionFloatVariables().IndexOf(_baseNode._condition.TransitionVariable), _baseNode._condition.GetTransitionFloatVariables().ToArray());
                if (selectedTransitionVariable < 0) { selectedTransitionVariable = 0; } // If the type changed, the index will retun -1 and needs to be acounted for
                _baseNode._condition.TransitionVariable = _baseNode._condition.GetTransitionFloatVariables()[selectedTransitionVariable];
            }
            else // Draw all variables
            {
                int selectedTransitionVariable = EditorGUILayout.Popup(_baseNode._condition.TransitionVariableList.IndexOf(_baseNode._condition.TransitionVariable), _baseNode._condition.TransitionVariableList.ToArray());
                if (selectedTransitionVariable < 0) { selectedTransitionVariable = 0; } // If the type changed, the index will retun -1 and needs to be acounted for
                _baseNode._condition.TransitionVariable = _baseNode._condition.TransitionVariableList[selectedTransitionVariable];
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }
}
