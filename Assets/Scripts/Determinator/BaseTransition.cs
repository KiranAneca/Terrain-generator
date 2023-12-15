using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class)]
public class BoolTrans : Attribute { }

[AttributeUsage(AttributeTargets.Class)]
public class FloatTrans : Attribute { }

[AttributeUsage(AttributeTargets.Class)]
public class CountTrans : Attribute { }


[Serializable]
public class BaseTransition 
{
    public virtual float Determine(Tile tile, string transitionVariable, float maxRange = 0)
    {
        return 0;
    }
}
