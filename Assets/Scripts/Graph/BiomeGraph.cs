using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class BiomeGraph : NodeGraph 
{
    [SerializeField] private BiomeNode _startingBiome;

    public BiomeNode DetermineBiome(Tile tile)
    {
        BaseNode currentNode = _startingBiome;
        while(!currentNode.IsEndNode())
        {
            currentNode = currentNode.GetOutputNode(tile);
        }

        return currentNode as BiomeNode;
    }
}