using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DeterminatorHelper
{
private static List<string> _transitionTypeList = new List<string>() {	"PLEASE SELECT VALUE",	"BoolTransition",	"FloatTransition",	"InRangeTransition",	"NeighbourCountTransition",};
private static List<string> _transitionVariableList = new List<string>() {"PLEASE SELECT VALUE","IsWater","IsLand","RawElevation","RawTemperature","RawRain","RawVegetation","RawCoal","RawIron",};
    public static List<string> GetTransitionTypes() { return _transitionTypeList; }
    public static List<string> GetTransitonVariables() { return _transitionVariableList; }
}
