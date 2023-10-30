using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Land = 0,
    Water = 1,
}

public class Tile : MonoBehaviour
{
    [SerializeField] private Biome _biome;
    [SerializeField] private TileType _tileType;
    [SerializeField] private Vector3 _gridPosition;

    [SerializeField] private List<Tile> _neighbours;

    // Parameters
    [SerializeField] private float _elevation;
    [SerializeField] private float _temperature;
    [SerializeField] private float _rain;

    public float Elevation { get => _elevation; set => SetElevation(value); }
    public float Temperature { get => _temperature; set => _temperature = value; }
    public float Rain { get => _rain; set => _rain = value; }

    // Functions
    public void DetermineBiome()
    {
        while(!_biome.IsEndBiome())
        {
            _biome = _biome.Determine(this);
        }
        GetComponent<Renderer>().material = _biome.GetMat();
    }

    public void SetElevation(float value)
    {
        _elevation = Elevation;
        if (_tileType == TileType.Water)
        { 
            _elevation = -_elevation;
        }
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

