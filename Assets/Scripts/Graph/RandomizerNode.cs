using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class RandomizerNode : BaseNode 
{

	// Use this for initialization
	protected override void Init() 
    {
		base.Init();
		
	}

    public override BaseNode GetOutputNode(Tile tile)
    {
        // Add all the options to the list
        var ports = GetAllOutputPorts();
        List<NodePort> connections = new List<NodePort>();

        for (int i = 0; i < ports.Count; i++)
        {
            if (ports[i].ConnectionCount > 0)
            {
                connections.Add(ports[i].GetConnection(0));
            }
        }

        // Get random based on weights
        float totalRange = 0;
        for (int i = 0; i < connections.Count; i++)
        {
            totalRange += _transitionValues[i];
        }
        float randomFloat = UnityEngine.Random.Range(0, totalRange);

        float counter = 0;
        int idx = 0;
        for (int i = 0; i < connections.Count; i++)
        {
            counter += _transitionValues[i];
            if (randomFloat < counter) break;
            idx++;
        }

        NodePort newNode = connections[idx];
        return newNode.node as BaseNode;
    }
}