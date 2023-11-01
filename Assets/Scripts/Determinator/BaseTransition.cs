using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseTransition 
{
    public virtual float Determine(Tile tile, string transitionVariable, float maxRange = 0)
    {
        return 0;
    }
}
