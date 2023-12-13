using NaughtyAttributes;
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

public enum floatComparator
{
    SmallerThan,
    SmallerOrEqual,
    BiggerThan,
    BiggerOrEqual,
    Equal,
    NotEqual
}


[Serializable]
public struct TransitionBiomeNode
{
    public BiomeNode Biome;
    public float TransitionValue;
}



[NodeWidth(300)]
public class BiomeNode : Node {

    [SerializeField][Dropdown("transitionTypes")] private string _transitionType;
    private List<string> transitionTypes { get { return DeterminatorHelper.GetTransitionTypes(); } }

    [SerializeField][Dropdown("transitionVariables")] private string _transitionVariable;
    private List<string> transitionVariables { get { return DeterminatorHelper.GetTransitonVariables(); } }

    [SerializeField] public BiomeTransition _condition = new BiomeTransition();

    [SerializeField] private TransitionCondition _transitionCondition;
    [SerializeField][Output(dynamicPortList = true)] private List<float> _transitionValues = new List<float>();

    [SerializeField] private Material _biomeMat;
    [SerializeField] private GameObject _extraVisual;

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

    public GameObject GetExtraVisual() 
    { 
        return _extraVisual;
    }
}