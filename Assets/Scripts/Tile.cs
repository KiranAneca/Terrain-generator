using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TileType
{
    Ocean = 0,
    Water = 1,
    Beach = 2,
    Plains = 3,
    Forest = 4,
    Mountain = 5,
    Snow = 6
}

public class Tile : MonoBehaviour
{
    [SerializeField] public TileType _TileType;
    [SerializeField] public Vector3 _GridPosition;

    [SerializeField] public float _Temperature;
    [SerializeField] public float _Rain;
}

