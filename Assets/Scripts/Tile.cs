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
public class TransitionVar : Attribute { }

public class Tile : MonoBehaviour
{
    [SerializeField] private BiomeNode _biome;
    [SerializeField] private TileType _tileType;
    [SerializeField] private Vector3 _gridPosition;

    [SerializeField] private List<Tile> _neighbours;

    // Parameters for generation
    [SerializeField] private bool _isWater;

    [SerializeField] private float _rawElevation;
    [SerializeField] private float _rawTemperature;
    [SerializeField] private float _rawRain;
    [SerializeField] private float _rawVegetation;

    [SerializeField] private float _rawCoal;
    [SerializeField] private float _rawIron;

    public float FT_Elevation { get => _rawElevation; set => SetElevation(value); }
    public float FT_Temperature { get => _rawTemperature; set => _rawTemperature = value; }
    public float FT_Rain { get => _rawRain; set => _rawRain = value; }
    public float FT_Vegetation { get => _rawVegetation; set => _rawVegetation = value; }
    public float FT_Coal { get => _rawCoal; set => _rawCoal = value; }
    public float FT_Iron { get => _rawIron; set => _rawIron = value; }
    public bool BT_isWater { get => _isWater; set => _isWater = value; }

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
        _rawElevation = value;
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
        _isWater = tileType == TileType.Water;
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

