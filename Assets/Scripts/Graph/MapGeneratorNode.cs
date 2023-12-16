using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[DisallowMultipleNodesAttribute]
public class MapGeneratorNode : BaseNode {

	[Input] public Noisemap HeightMap;


	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

	public void GetInputs()
	{
        HeightMap = GetInputPort("HeightMap").GetInputValue() as Noisemap;
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}

}