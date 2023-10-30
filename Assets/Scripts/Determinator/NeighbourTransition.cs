using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourTransition : BaseTransition
{
    public override float Determine(Tile tile, string transitionVariable)
    {
        // TO BE IMPLEMENTED
        var type = tile.GetType();
        var property = type.GetProperty(transitionVariable, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.GetProperty);
        var value = property.GetValue(tile);
        return (float)value;
    }
}
