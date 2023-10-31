using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility 
{
    public static float NormalizeValueToNormalRange(float min, float max, float value)
    {
        return (value - min) / (max - min);
    }
}
