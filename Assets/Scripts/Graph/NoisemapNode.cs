using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNodeEditor;
using static XNodeEditor.NodeEditor;

[Serializable]
public class Noisemap
{
    public List<float> NoiseMapValues;
    public Vector2Int NoiseMapSize;
}

public class NoisemapNode : BaseNode {

    public MapGeneratorNode MapgenNode; // Reference to the mapgenNode
    [Output] public Noisemap Noisemap = new Noisemap();
    [SerializeField][Range(0.01f, 0.3f)] private float _scatter;
    protected override void Init() {
		base.Init();

        Initialize();
        GenerateNewMap();
    }

    public void Initialize()
    {
        BiomeGraph graph = this.graph as BiomeGraph;
        MapGeneratorNode node = graph.nodes.Find(t => t is MapGeneratorNode) as MapGeneratorNode;
        if (node != null)
        {
            MapgenNode = node;
        }
    }

    public void GenerateNewMap()
    {
        if(MapgenNode != null) { Noisemap.NoiseMapSize = MapgenNode.MapSize; }

        int offset = UnityEngine.Random.Range(1, 1000);
        // Delete the old map
        Noisemap.NoiseMapValues = new List<float>();
        for (int x = 0; x < Noisemap.NoiseMapSize.x; ++x)
        {
            for (int y = 0; y < Noisemap.NoiseMapSize.y; ++y)
            {
                float elevation = Mathf.PerlinNoise(x * _scatter + offset, y * _scatter + offset);
                Noisemap.NoiseMapValues.Add(elevation);
            }
        }
    }
	public override object GetValue(NodePort port) 
	{
		return Noisemap; 
	}
}

[CustomNodeEditor(typeof(NoisemapNode))]
public class NoisemapNodeEditor : NodeEditor
{
    private NoisemapNode _baseNode;
    public override void OnBodyGUI()
    {
        if (_baseNode == null) _baseNode = target as NoisemapNode;

        if (_baseNode == null) return;

        // Update serialized object's representation
        serializedObject.Update();
        if(_baseNode.MapgenNode == null)
        {
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Noisemap").FindPropertyRelative("NoiseMapSize"));
        }
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_scatter"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Noisemap"));
    }

    public override Color GetTint()
    {
        return new Color(0.65f, 0.31f, 0.32f);
    }
}