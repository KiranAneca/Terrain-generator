using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using static UnityEngine.GraphicsBuffer;
using XNodeEditor;
using static XNodeEditor.NodeEditor;

[Serializable]
public class FloatNode : BaseNode {

    [SerializeField] public float Value;
	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}

}

[CustomNodeEditor(typeof(FloatNode))]
public class FloatNodeEditor : NodeEditor
{
    private FloatNode _baseNode;
    public override void OnBodyGUI()
    {
        if (_baseNode == null) _baseNode = target as FloatNode;

        if (_baseNode == null) return;

        // Update serialized object's representation
        serializedObject.Update();
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Value"));
    } 
}