using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Transition")]
[Serializable]
public class BaseTransition 
{
    public virtual float Determine(Tile tile)
    {
        return 0;
    }
}
