using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Visualizer : MonoBehaviour
{
    [SerializeField] private MapGen _map;

    [SerializeField] private bool _drawVisuals;
    [SerializeField] private bool _drawPartitions;

    void Update()
    {
        if(!_drawVisuals) { return; }

        if(_drawPartitions)
        {
            DrawPartitions();
        }
    }

    private void DrawPartitions()
    {
        List<Tile>[,] partitionedMap = _map.GetPartitionedMap();
        Vector2Int partitionedMapSize = _map.PartitionedMapSize;

        int idx = 0;
        for (int x = 0; x < partitionedMap.GetLength(0); x++)
        {
            for (int y = 0; y < partitionedMap.GetLength(1); y++)
            {
                idx++;
                float xSize = x * partitionedMapSize.x + (0.5f * partitionedMapSize.x);
                float ySize = y * partitionedMapSize.y + (0.5f * partitionedMapSize.y);

                UnityEngine.Random.InitState(idx);
                DrawBox(new Vector3(xSize, 0, ySize), Quaternion.identity, new Vector3(partitionedMapSize.x, 1, partitionedMapSize.y), UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
            }
        }
    }



    public void DrawBox(Vector3 pos, Quaternion rot, Vector3 scale, Color c)
    {
        // create matrix
        Matrix4x4 m = new Matrix4x4();
        m.SetTRS(pos, rot, scale);

        var point1 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, 0.5f));
        var point2 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, 0.5f));
        var point3 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, -0.5f));
        var point4 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, -0.5f));

        var point5 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, 0.5f));
        var point6 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, 0.5f));
        var point7 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, -0.5f));
        var point8 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, -0.5f));

        Debug.DrawLine(point1, point2, c);
        Debug.DrawLine(point2, point3, c);
        Debug.DrawLine(point3, point4, c);
        Debug.DrawLine(point4, point1, c);

        Debug.DrawLine(point5, point6, c);
        Debug.DrawLine(point6, point7, c);
        Debug.DrawLine(point7, point8, c);
        Debug.DrawLine(point8, point5, c);

        Debug.DrawLine(point1, point5, c);
        Debug.DrawLine(point2, point6, c);
        Debug.DrawLine(point3, point7, c);
        Debug.DrawLine(point4, point8, c);

        // optional axis display
        //Debug.DrawRay(m.GetPosition(), m.GetForward(), Color.magenta);
        //Debug.DrawRay(m.GetPosition(), m.GetUp(), Color.yellow);
        //Debug.DrawRay(m.GetPosition(), m.GetRight(), Color.red);
    }
}
