using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsWaterTransition : BaseTransition
{
    public override float Determine(Tile tile)
    {
        bool isWater = tile.GetTileType() == TileType.Water;
        if (isWater)
        {
            return 1f;
        }
        return 0f;
    }
}
