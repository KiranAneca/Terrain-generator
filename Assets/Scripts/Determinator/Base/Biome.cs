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

public enum FloatTransitionCondition
{
    None,
    Closest,
    Threshold,
}

[CreateAssetMenu(menuName = "Biome")]
public class Biome : ScriptableObject
{
    [SerializeField] private Biome _linkBackBiome;
    [SerializeField][Dropdown("DeterminatorHelper.Instance.GetTransitionTypes()")] private string _transition;
    [SerializeField][Dropdown("DeterminatorHelper.Instance.GetTransitonVariables()")] private string _transitionVariable;
    [SerializeField] private FloatTransitionCondition _floatTransitionCondition;
    [SerializeField] private bool _endBiome = true;
    [SerializeField] private List<TransitionBiome> _transitionBiomes;

    [SerializeField] private Material _biomeMat;

    public Biome Determine(Tile tile)
    {
        // Loop over all the possible biomes and select the one with the value the closest to the transition value

        string typeName = _transition.ToString();
        Type type = Type.GetType(typeName);

        BaseTransition transition = Activator.CreateInstance(type) as BaseTransition;

        float transitionValue = transition.Determine(tile, _transitionVariable.ToString());

        return FloatDeterminator(transitionValue);
    }

    private Biome FloatDeterminator(float transitionValue)
    {
        Biome newBiome = _transitionBiomes[0].Biome;
        switch (_floatTransitionCondition)
        {
            case FloatTransitionCondition.None: // In case this isn't a float determination, return the closest, which should be 0 or 1
                float closestValue = Mathf.Infinity;
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
            case FloatTransitionCondition.Closest: // Return the closest result
                closestValue = Mathf.Infinity;
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
            case FloatTransitionCondition.Threshold: // Return the biome with the biggest value that is met
                for (int i = 0; i < _transitionBiomes.Count; i++)
                {
                    float testvalue = transitionValue - _transitionBiomes[i].TransitionValue ;
                    if (testvalue >= 0)
                    {
                        newBiome = _transitionBiomes[i].Biome;
                    }
                }
                return newBiome;
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
