using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public enum ConditionAddition
{
    None, 
    And,
    Sum
}

public enum ConditionType
{

}

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

[Serializable]
public class BiomeTransition
{
    [Dropdown("TransitionTypesList")] public string TransitionType = DeterminatorHelper.GetTransitionTypes()[0];
    public List<string> TransitionTypesList { get { return DeterminatorHelper.GetTransitionTypes(); } }

    [Dropdown("TransitionVariableList")] public string TransitionVariable = DeterminatorHelper.GetTransitonVariables()[0];
    public List<string> TransitionVariableList { get { return DeterminatorHelper.GetTransitonVariables(); } }

    //public TransitionCondition Condition;
    public floatComparator FloatComparator;

    public List<string> GetTransitionBoolVariables()
    {
        List<string> boolVars = new List<string>{"PLEASE SELECT VALUE" };

        var propertyValues = typeof(Tile).GetFields();

        for (int i = 0; i < propertyValues.Length; i++)
        {
            BoolVar lookupBoolVar = Attribute.GetCustomAttribute(propertyValues[i], typeof(BoolVar)) as BoolVar;
            if (lookupBoolVar != null)
            {
                boolVars.Add(propertyValues[i].Name);
            }
        }
        return boolVars;
    }

    public List<string> GetTransitionFloatVariables()
    {
        List<string> floatVars = new List<string> { "PLEASE SELECT VALUE" };

        var propertyValues = typeof(Tile).GetFields();

        for (int i = 0; i < propertyValues.Length; i++)
        {
            FloatVar lookupBoolVar = Attribute.GetCustomAttribute(propertyValues[i], typeof(FloatVar)) as FloatVar;
            if (lookupBoolVar != null)
            {
                floatVars.Add(propertyValues[i].Name);
            }
        }
        return floatVars;
    }

    public List<string> GetTransitionTypes()
    {
        Type trans = typeof(BaseTransition);
        Assembly sim = Assembly.GetAssembly(trans);
        Type[] types = sim.GetTypes();
        System.Type[] options = (from System.Type type in types where type.IsSubclassOf(typeof(BaseTransition)) select type).ToArray();
        List<string> typeList = new List<string> { "PLEASE SELECT VALUE" }; ;
        {
            foreach (var item in options)
            {
                typeList.Add(item.ToString());
            }
        }
        return typeList;
    }
}
