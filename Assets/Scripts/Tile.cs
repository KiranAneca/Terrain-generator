using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Land = 0,
    Water = 1,
}

public struct TileData
{
    public BiomeNode BiomeType;
    public float Elevation;
    public float Temperature;
    public float Rain;
    public string Vegetation;
    public float Iron;
    public float Coal;

}

[AttributeUsage(AttributeTargets.Field)]
public class BoolVar : Attribute { }

[AttributeUsage(AttributeTargets.Field)]
public class FloatVar : Attribute { }

[Serializable]
public class Tile : MonoBehaviour
{
    [SerializeField] private BiomeNode _biome;
    [SerializeField] private TileType _tileType;
    [SerializeField] private Vector2Int _gridPosition;

    // Actual data
    private TileData _data;

    [SerializeField] private List<Tile> _neighbours;

    // Parameters for generation
    [SerializeField][BoolVar] public bool IsWater;
    [SerializeField][BoolVar] public bool IsLand;

    [SerializeField][FloatVar] public float RawElevation;
    [SerializeField][FloatVar] public float RawTemperature;
    [SerializeField][FloatVar] public float RawRain;
    [SerializeField][FloatVar] public float RawVegetation;

    [SerializeField][FloatVar] public float RawCoal;
    [SerializeField][FloatVar] public float RawIron;

    [SerializeField] private Vector2Int _partionedChunk;

    public Vector2Int PartionedChunk { get => _partionedChunk; set => _partionedChunk = value; }


    // Functions
    public void VisualizeElevation(float multiplier)
    {
        if(IsWater) 
        {
            return; 
        }
        transform.localScale = new Vector3(1, 1, RawElevation * multiplier);
    }

    public void DetermineBiome()
    {
        // Determining the biome
        _biome = TileManager.Instance.DetermineBiome(this);

        // Visuals
        GetComponent<Renderer>().material = _biome.GetMat();
        GameObject obj = _biome.GetExtraVisual();
        if (obj != null)
        {
            GameObject extraVisual = Instantiate(obj);
            extraVisual.transform.parent = transform;
            extraVisual.transform.localPosition = Vector3.zero;
            extraVisual.transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
        }

        FillData();
    }


    private void FillData()
    {
        // Fill in the correct data
        _data.BiomeType = _biome;

        _data.Elevation = RawElevation;
        _data.Temperature = RawTemperature;
        _data.Rain = RawRain;
        _data.Vegetation = RawVegetation.ToString("F2");
        _data.Coal = RawCoal;
        _data.Iron = RawIron;
    }

    public BiomeNode GetBiome()
    {
        return _biome;
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
        IsLand = tileType == TileType.Land;
    }

    public Vector2Int GetGridPosition()
    {
        return _gridPosition;
    }

    public void SetGridPosition(Vector2Int gridPosition)
    {
        _gridPosition = gridPosition;
    }

    private void OnMouseDown()
    {
        BiomeCard.Instance.OpenBiomeCard(_data);
    }
}

