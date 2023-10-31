using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DeterminatorHelper
{
private static List<string> _transitionTypeList = new List<string>() {	"PLEASE SELECT VALUE",	"BoolTransition",	"FloatTransition",	"NeighbourCountTransition",};
private static List<string> _transitionVariableList = new List<string>() {"PLEASE SELECT VALUE","FT_Elevation","FT_Temperature","FT_Rain","FT_Vegetation","FT_Coal","FT_Iron","BT_isWater",};
    public static List<string> GetTransitionTypes() { return _transitionTypeList; }
    public static List<string> GetTransitonVariables() { return _transitionVariableList; }
}
