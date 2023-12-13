using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionAddition
{
    None, 
    And,
    Sum
}

[Serializable]
public class BiomeTransition
{
    [Dropdown("TransitionTypesList")] public string TransitionType;
    public List<string> TransitionTypesList { get { return DeterminatorHelper.GetTransitionTypes(); } }

    [Dropdown("transitionVariables")] public string TransitionVariable;
    private List<string> transitionVariables { get { return DeterminatorHelper.GetTransitonVariables(); } }

    public TransitionCondition Condition;
    public floatComparator FloatComparator;
}
