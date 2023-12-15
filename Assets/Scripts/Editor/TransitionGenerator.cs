#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Collections.ObjectModel;

public class TransitionGenerator 
{
    [MenuItem("Tools/Generate Transition variables")]
    public static void Go()
    {

        // Get all the transition scripts
        Type trans = typeof(BaseTransition);
        Assembly sim = Assembly.GetAssembly(trans);
        Type[] types = sim.GetTypes();
        System.Type[] options = (from System.Type type in types where type.IsSubclassOf(typeof(BaseTransition)) select type).ToArray();
        List<string> typeList = new List<string>();
        {
            foreach (var item in options)
            {
                typeList.Add(item.ToString());
            }
        }

        // Get all the flagged variables from the tile script
        List<string> boolVars = new List<string>();
        List<string> floatVars = new List<string>();
        var propertyValues = typeof(Tile).GetFields();

        for (int i = 0; i < propertyValues.Length; i++)
        {
            BoolVar lookupBoolVar = Attribute.GetCustomAttribute(propertyValues[i], typeof(BoolVar)) as BoolVar;
            if (lookupBoolVar != null)
            {
                boolVars.Add(propertyValues[i].Name);
            }
            FloatVar lookupfloatVar = Attribute.GetCustomAttribute(propertyValues[i], typeof(FloatVar)) as FloatVar;
            if (lookupfloatVar != null)
            {
                floatVars.Add(propertyValues[i].Name);
            }
        }

        // Generate the new text lines
        string transitionType = "private static List<string> _transitionTypeList = new List<string>() {";
        transitionType += "	\"" + "PLEASE SELECT VALUE" + "\",";
        foreach (var item in typeList)
        {
            transitionType += "	\"" + item.ToString() + "\",";
        }
        transitionType += "};";

        string transitionVariables = "private static List<string> _transitionVariableList = new List<string>() {";
        transitionVariables += "\"" + "PLEASE SELECT VALUE" + "\",";
        foreach (var item in boolVars)
        {
            transitionVariables += "\"" + item.ToString() + "\",";
        }
        transitionVariables += "};";

        // Read the file and change the lists to the newly generated ones
        string filePathAndName = "Assets/Scripts/Determinator/" + "DeterminatorHelper" + ".cs"; //The folder Scripts/Enums/ is expected to exist
        StreamReader reader = new StreamReader(filePathAndName);
        List<string> newText = new List<string>();
        while (!reader.EndOfStream) 
        { 
            string text = reader.ReadLine();
            if (text.Contains("private static List<string> _transitionTypeList"))
            {
                text = transitionType;
            }
            if (text.Contains("private static List<string> _transitionVariableList"))
            {
                text = transitionVariables;
            }
            newText.Add(text);
        }
        reader.Close();
        StreamWriter writer = new StreamWriter(filePathAndName);
        foreach(var item in newText)
        {
            writer.WriteLine(item);
        }
        writer.Close();

        AssetDatabase.Refresh(); 
    }
}
#endif