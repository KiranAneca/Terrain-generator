using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeTransition : BaseTransition
{
    public override float Determine(Tile tile, string transitionVariable, float maxRange = 0)
    {
        float result = 0;
        var type = tile.GetType();
        var property = type.GetField(transitionVariable, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.GetProperty);
        if (property == null)
        {
            Debug.LogError("InRangeTransition property not set in: " + tile.GetBiome().name + ". Transitionvariable is: " + transitionVariable);
            return 0;
        }

        float inRange = TileManager.Instance.FindTileInRange(tile, (int)maxRange, transitionVariable);
        if(inRange > maxRange ) 
        {
            //Debug.Log("Tried to find in range: " + maxRange + ". Found it in: " + inRange );
            inRange = -1f;
        }

        return inRange;
    }
}
