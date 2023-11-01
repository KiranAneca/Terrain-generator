using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatTransition : BaseTransition
{
    public override float Determine(Tile tile, string transitionVariable, float maxRange = 0)
    {
        var type = tile.GetType();
        var property = type.GetField(transitionVariable, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.GetProperty);
        if(property == null)
        {
            Debug.LogError("FloatTransition property not set in: " + tile.GetBiome().name + ". Transitionvariable is: " + transitionVariable);
            return 0;
        }
        var value = property.GetValue(tile);
        return (float)value;
    }
}
