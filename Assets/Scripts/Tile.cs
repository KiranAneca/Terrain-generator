using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Land = 0,
    Water = 1,
}

[AttributeUsage(AttributeTargets.Field)]
public class BiomeVar : Attribute { }

public class Tile : MonoBehaviour
{
    [SerializeField] private BiomeNode _biome;
    [SerializeField] private TileType _tileType;
    [SerializeField] private Vector3 _gridPosition;

    [SerializeField] private List<Tile> _neighbours;

    // Parameters for generation
    [SerializeField][BiomeVar] private bool isWater;

    [SerializeField][BiomeVar] public float RawElevation;
    [SerializeField][BiomeVar] public float RawTemperature;
    [SerializeField][BiomeVar] public float RawRain;
    [SerializeField][BiomeVar] public float RawVegetation;

    [SerializeField][BiomeVar] public float RawCoal;
    [SerializeField][BiomeVar] public float RawIron;

    public bool IsWater { get => isWater; set => isWater = value; }

    // Functions
    public void DetermineBiome()
    {
        _biome = TileManager.Instance.DetermineBiome(this);
        GetComponent<Renderer>().material = _biome.GetMat();
    }

    public BiomeNode GetBiome()
    {
        return _biome;
    }

    public void SetElevation(float value)
    {
        RawElevation = value;
        //gameObject.transform.localPosition += new Vector3(0, value, 0);
    }

    public TileType GetTileType()
    {
        return _tileType;
    }

    public void SetNeighbours(List<Tile> neighbours)
    {
        _neighbours = neighbours;
    }

    public List<Tile> GetNeighbours()
    {
        return _neighbours;
    }
    public void SetTileType(TileType tileType)
    {
        _tileType = tileType;
        IsWater = tileType == TileType.Water;
    }

    public Vector3 GetGridPosition()
    {
        return _gridPosition;
    }

    public void SetGridPosition(Vector3 gridPosition)
    {
        _gridPosition = gridPosition;
    }
}

