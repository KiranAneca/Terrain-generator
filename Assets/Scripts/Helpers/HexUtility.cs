using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexUtility : MonoBehaviour
{
    static public int CalculateHexDistance(Vector2 a, Vector2 b)
    {
        int dx = Mathf.Abs((int)a.x - (int)b.x);
        int dy = Mathf.Abs((int)a.y - (int)b.y);
        return (dx + Mathf.Max(0, (dy - dx) / 2));
    }

}
