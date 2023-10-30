using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct TransitionBiome
{
    public Biome Biome;
    public float TransitionValue;
}



[CreateAssetMenu(menuName = "Biome")]
public class Biome : ScriptableObject
{
    [SerializeField] private bool _endBiome = true;
    [SerializeField] private Transitions _transition;
    [SerializeField] private List<TransitionBiome> _transitionBiomes;

    [SerializeField] private Material _biomeMat;

    public Biome Determine(Tile tile)
    {
        // Loop over all the possible biomes and select the one with the value the closest to the transition value

        string typeName = _transition.ToString();
        Type type = Type.GetType(typeName);

        BaseTransition transition = Activator.CreateInstance(type) as BaseTransition;

        float transitionValue = transition.Determine(tile);
        float closestValue = Mathf.Infinity;
        Biome newBiome = _transitionBiomes[0].Biome;
        for (int i = 0; i < _transitionBiomes.Count; i++)
        {
            float testvalue = Mathf.Abs(_transitionBiomes[i].TransitionValue - transitionValue);
            if (testvalue <= closestValue)
            {
                closestValue = testvalue;
                newBiome = _transitionBiomes[i].Biome;
            }
        }
        return newBiome;
    }

    public bool IsEndBiome()
    {
        if(_transitionBiomes.Count < 2)
        {
            return true;
        }
        return _endBiome;
    }

    public Material GetMat()
    { 
        return _biomeMat;
    }
}
