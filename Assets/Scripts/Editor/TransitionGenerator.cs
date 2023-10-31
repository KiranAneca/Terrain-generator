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
    [MenuItem("Tools/Generate Transition enum")]
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
        List<string> properties = new List<string>();
        var propertyValues = typeof(Tile).GetProperties();
        for (int i = 0; i < propertyValues.Length; i++)
        {
            if (propertyValues[i].Name.Contains("T_"))
            {
                properties.Add(propertyValues[i].Name);
            }
        }

        // Generate the new text lines
        string transitionType = "private static List<string> _transitionTypeList = new List<string>() {";
        foreach (var item in typeList)
        {
            transitionType += "	\"" + item.ToString() + "\",";
        }
        transitionType += "};";

        string transitionVariables = "private static List<string> _transitionVariableList = new List<string>() {";
        foreach (var item in properties)
        {
            transitionVariables += "\"" + item.ToString() + "\",";
        }
        transitionVariables += "};";

        // Read the file and change the lists to the newly generated ones
        string filePathAndName = "Assets/Scripts/Determinator/" + "DeterminatorHelper" + ".cs"; //The folder Scripts/Enums/ is expected to exist
        StreamReader reader = new StreamReader(filePathAndName);
        string newText = "";
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
            newText += text;
        }
        reader.Close();
        StreamWriter writer = new StreamWriter(filePathAndName);
        writer.Write(newText);
        writer.Close();

        AssetDatabase.Refresh(); 
    }
}
#endif