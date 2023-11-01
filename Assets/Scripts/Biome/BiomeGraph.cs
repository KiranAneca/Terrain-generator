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
        BiomeNode currentBiome = _startingBiome;
        while(!currentBiome.IsEndBiome())
        {
            string transitionVariable = currentBiome.GetTransitionVariable();
            string transitionType = currentBiome.GetTransitionType();
            List<float> transitionValues = currentBiome.GetTransitionValues();

            // Add all the options to the list
            var ports = currentBiome.GetAllOutputPorts();
            List<NodePort> connections = new List<NodePort>();

            for (int i = 0; i < ports.Count; i++)
            {
                if (ports[i].ConnectionCount > 0)
                {
                    connections.Add(ports[i].GetConnection(0));
                }
            }

            string typeName = transitionType.ToString();
            Type type = Type.GetType(typeName);

            float biggestRange = 0;
            for (int i = 0; i < connections.Count; i++)
            {
                if(transitionValues[i] >= biggestRange)
                {
                    biggestRange = transitionValues[i];
                }
            }

            BaseTransition transition = Activator.CreateInstance(type) as BaseTransition;
            float transitionValue = transition.Determine(tile, transitionVariable.ToString(), biggestRange);


            NodePort newNode = connections[0];
            switch (currentBiome.GetTransitionCondition())
            {
                case TransitionCondition.Threshold:
                    for (int i = 0; i < connections.Count; i++)
                    {
                        // If the testvalue is bigger then the threshold
                        float testvalue = transitionValue - transitionValues[i];
                        if (testvalue >= 0)
                        {
                            newNode = connections[i];
                        }
                    }
                    break;
                default:
                    // Closest
                    float closestValue = Mathf.Infinity;

                    for (int i = 0; i < connections.Count; i++)
                    {
                        // If the testvalue is closer to the output
                        float testvalue = Mathf.Abs(transitionValues[i] - transitionValue);
                        if (testvalue <= closestValue)
                        {
                            closestValue = testvalue;
                            newNode = connections[i];
                        }
                    }
                    break;
            }

            currentBiome = newNode.node as BiomeNode;
        }

        return currentBiome;
    }

    private void Determine(Tile tile, TransitionCondition condition)
    {

    }
}