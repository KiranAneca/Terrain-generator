#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

public class TransitionGenerator 
{
    [MenuItem("Tools/Generate Transition enum")]
    public static void Go()
    {
        Type trans = typeof(BaseTransition);
        Assembly sim = Assembly.GetAssembly(trans);
        Type[] types = sim.GetTypes();
        System.Type[] options = (from System.Type type in types where type.IsSubclassOf(typeof(BaseTransition)) select type).ToArray();

        string filePathAndName = "Assets/Scripts/Enums" + "TransitionEnum" + ".cs"; //The folder Scripts/Enums/ is expected to exist

        using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
        {
            streamWriter.WriteLine("using System;");
            streamWriter.WriteLine("[Serializable]");
            streamWriter.WriteLine("public enum " + "TransitionEnum");
            streamWriter.WriteLine("{");
            for (int i = 0; i < options.Length; i++)
            {
                if(options[i] != null)
                {
                    streamWriter.WriteLine("	" + options[i].ToString() + ",");
                }
            }
            streamWriter.WriteLine("}"); 
        }

        getProperties(typeof(Tile));

        Debug.Write("Generated enum");
        AssetDatabase.Refresh(); 
    }

    public static void getProperties(Type type)
    {
        List<string> properties = new List<string>();
        var propertyValues = type.GetProperties();
        for (int i = 0; i < propertyValues.Length; i++)
        {
            if (propertyValues[i].Name.Contains("T_"))
            {
                properties.Add(propertyValues[i].Name); 
            }
        }

        string filePathAndName = "Assets/Scripts/Enums" + "TransitionVariableEnum" + ".cs"; //The folder Scripts/Enums/ is expected to exist
        using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
        {
            streamWriter.WriteLine("using System;");
            streamWriter.WriteLine("[Serializable]");
            streamWriter.WriteLine("public enum " + "TransitionVariableEnum");
            streamWriter.WriteLine("{");
            for (int i = 0; i < properties.Count; i++)
            {
                if (properties[i] != null)
                {
                    streamWriter.WriteLine("	" + properties[i].ToString() + ",");
                }
            }
            streamWriter.WriteLine("}");
        }
    }
}
#endif