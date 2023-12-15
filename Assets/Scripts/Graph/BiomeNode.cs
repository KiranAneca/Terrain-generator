using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using static System.TimeZoneInfo;

[NodeWidth(300)]
public class BiomeNode : BaseNode 
{
    [SerializeField] public BiomeTransition _condition =  new BiomeTransition();

    [SerializeField] private TransitionCondition _transitionCondition;

    [SerializeField] private Material _biomeMat;
    [SerializeField] private GameObject _extraVisual;

 

    // Use this for initialization
    protected override void Init() {
		base.Init();
	}

    public void SetTransitionValue(int idx, float value)
    {
        var t = _transitionValues[idx];
        t.Value = value;
        _transitionValues[idx] = t;
    }

    public void SetTransitionNode(int idx, FloatNode node)
    {
        var t = _transitionValues[idx];
        t.Node = node;
        _transitionValues[idx] = t;
    }

    public override BaseNode GetOutputNode(Tile tile)
    {
        // Add all the options to the list
        var ports = GetAllOutputPorts();
        List<NodePort> connections = new List<NodePort>();

        for (int i = 0; i < ports.Count; i++)
        {
            if (ports[i].ConnectionCount > 0)
            {
                connections.Add(ports[i].GetConnection(0));
            }
        }

        string typeName = _condition.TransitionType;
        Type type = Type.GetType(typeName);

        float biggestRange = 0;
        for (int i = 0; i < connections.Count; i++)
        {
            if (_transitionValues[i].Value >= biggestRange)
            {
                biggestRange = _transitionValues[i].Value;
            }
        }

        BaseTransition transition = Activator.CreateInstance(type) as BaseTransition;
        float transitionValue = transition.Determine(tile, _condition.TransitionVariable, biggestRange);

        NodePort newNode = connections[0];
        switch (GetTransitionCondition())
        {
            case TransitionCondition.Threshold:
                for (int i = 0; i < connections.Count; i++)
                {
                    // If the testvalue is bigger then the threshold
                    float testvalue = transitionValue - _transitionValues[i].Value;
                    if (testvalue >= 0)
                    {
                        newNode = connections[i];
                    }
                }
                break;
            default:
                // Closest
                float closestValue = Mathf.Infinity;

                for (int i = 0; i < connections.Count; i++)
                {
                    // If the testvalue is closer to the output
                    float testvalue = Mathf.Abs(_transitionValues[i].Value - transitionValue);
                    if (testvalue <= closestValue)
                    {
                        closestValue = testvalue;
                        newNode = connections[i];
                    }
                }
                break;
        }
        return newNode.node as BaseNode; 
	}

    public TransitionCondition GetTransitionCondition() 
    {
        return _transitionCondition; 
    }

    public List<OutputFloat> GetTransitionValues()
    {
        return _transitionValues;
    }

    public string GetTransitionVariable()
    {
        return _condition.TransitionVariable;
    }

    public string GetTransitionType()
    {
        return _condition.TransitionType;
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