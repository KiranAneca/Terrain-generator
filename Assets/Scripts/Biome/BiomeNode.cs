using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public enum TransitionCondition
{
    PLEASE_SELECT,
    Closest,
    Threshold,
}

[Serializable]
public struct TransitionBiomeNode
{
    public BiomeNode Biome;
    public float TransitionValue;
}

[NodeWidth(250)]
public class BiomeNode : Node {

    [SerializeField][Dropdown("DeterminatorHelper.GetTransitionTypes()")] private string _transitionType;
    [SerializeField][Dropdown("DeterminatorHelper.GetTransitonVariables()")] private string _transitionVariable;
    [SerializeField] private TransitionCondition _transitionCondition;
    [SerializeField][Output(dynamicPortList = true)] private List<float> _transitionValues;

    [SerializeField] private Material _biomeMat;

    [SerializeField][Output] private float _outputValue;
    [SerializeField][Input] private float _inputValue;

    // Use this for initialization
    protected override void Init() {
		base.Init();
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return _outputValue; // Replace this
	}

    public TransitionCondition GetTransitionCondition() 
    {
        return _transitionCondition; 
    }

    public List<float> GetTransitionValues()
    {
        return _transitionValues;
    }

    public string GetTransitionVariable()
    {
        return _transitionVariable;
    }

    public string GetTransitionType()
    {
        return _transitionType;
    }

    public bool IsEndBiome()
    {
        if (_transitionValues.Count < 2)
        {
            return true;
        }
        return false;
    }

    public Material GetMat()
    {
        return _biomeMat;
    }
}