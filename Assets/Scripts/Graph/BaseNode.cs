using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[Serializable]
public struct OutputFloat
{
    public float Value;
    public FloatNode Node;
}

[Serializable]
public class BaseNode : Node
{
    [SerializeField][Input] protected float _inputValue;
    [SerializeField][Output(dynamicPortList = true)] protected List<OutputFloat> _transitionValues = new List<OutputFloat>();
    public virtual BaseNode GetOutputNode(Tile tile)
    {
        Debug.LogWarning("No GetOutputNode(Tile tile) override defined for " + GetType());
        return null;
    }

    public bool IsEndNode()
    {
        if (_transitionValues.Count < 2)
        {
            return true;
        }
        return false;
    }
}
