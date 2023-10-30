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

        string filePathAndName = "Assets/Scripts/" + "TransitionEnum" + ".cs"; //The folder Scripts/Enums/ is expected to exist

        using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
        {
            streamWriter.WriteLine("using System;");
            streamWriter.WriteLine("[Serializable]");
            streamWriter.WriteLine("public enum " + "Transitions");
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
        AssetDatabase.Refresh(); 
    }
}
#endif