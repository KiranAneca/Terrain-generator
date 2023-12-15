using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BaseNode : Node
{
    [SerializeField][Input] protected float _inputValue;
    [SerializeField][Output(dynamicPortList = true)] protected List<float> _transitionValues = new List<float>();
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
