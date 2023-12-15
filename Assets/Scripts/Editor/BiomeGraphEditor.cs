using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using static XNodeEditor.NodeGraphEditor;

[CustomNodeGraphEditor(typeof(BiomeGraph))]
public class BiomeGraphEditor : NodeGraphEditor
{
    public override string GetNodeMenuName(System.Type type)
    {
        if (type == typeof(RandomizerNode))
        {
            return "Maths/Randomizer";
        }
        else if (type.BaseType == typeof(BaseNode))
        {
            return base.GetNodeMenuName(type);
        }
        else return null;
    }
}
