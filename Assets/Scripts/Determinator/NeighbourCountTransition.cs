using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourCountTransition : BaseTransition
{
    public override float Determine(Tile tile, string transitionVariable)
    {
        float result = 0;
        var type = tile.GetType();
        var property = type.GetField(transitionVariable, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.GetProperty);
        if (property == null)
        {
            Debug.LogError("NeighbourCountTransition property not set in: " + tile.GetBiome().name + ". Transitionvariable is: " + transitionVariable);
            return 0;
        }
        // Get all the values of the neighbours
        foreach (var neighbour in tile.GetNeighbours())
        {
            var temp = property.GetValue(neighbour);
            result += Convert.ToInt32(temp);
        }
        // Normalize them to range [0, 1] to be able to determine the percentage easy
        result = Utility.NormalizeValueToNormalRange(0, tile.GetNeighbours().Count, result);
        return result;
    }
}
