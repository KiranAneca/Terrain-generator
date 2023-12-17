using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNodeEditor;
using static XNodeEditor.NodeEditor;

[DisallowMultipleNodesAttribute]
public class MapGeneratorNode : BaseNode {

	public Vector2Int MapSize;
	[Input] public Noisemap HeightMap;
	[Input] public Noisemap TemperatureMap;
	[Input] public Noisemap RainMap;
	[Input] public Noisemap VegetationMap;
	[Input] public Noisemap CoalMap;
	[Input] public Noisemap IronMap;

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

	public void GetInputs()
	{
		HeightMap = GetInputPort("HeightMap").GetInputValue() as Noisemap;
		TemperatureMap = GetInputPort("TemperatureMap").GetInputValue() as Noisemap;
		RainMap = GetInputPort("RainMap").GetInputValue() as Noisemap;
		VegetationMap = GetInputPort("VegetationMap").GetInputValue() as Noisemap;
		CoalMap = GetInputPort("CoalMap").GetInputValue() as Noisemap;
		IronMap = GetInputPort("IronMap").GetInputValue() as Noisemap;
	}
	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}

}

[CustomNodeEditor(typeof(MapGeneratorNode))]
public class MapGeneratorNodeEditor : NodeEditor
{
    public override Color GetTint()
    {
        return new Color(0.55f, 0.31f, 0.52f);
    }
}