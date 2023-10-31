using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DeterminatorHelper : MonoBehaviour
{ 
    private static DeterminatorHelper _instance; 
    public static DeterminatorHelper Instance { get { return _instance; } } 
private static List<string> _transitionTypeList = new List<string>() {	"BoolTransition",	"FloatTransition",	"NeighbourCountTransition",};
private static List<string> _transitionVariableList = new List<string>() {"FT_Elevation","FT_Temperature","FT_Rain","FT_Vegetation","FT_Coal","FT_Iron","BT_isWater",};
    public static List<string> GetTransitionTypes() { return _transitionTypeList; } 
    public static List<string> GetTransitonVariables() { return _transitionVariableList; } 
    private void Awake() { _instance = this; } }
