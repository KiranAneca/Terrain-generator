using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    // Singleton
    private static TileManager _Instance = null;

    private GameObject _CurrentTileMenu;
    private List<Tile> _Tiles;

    [Header("Tiles")]
    [SerializeField] private GameObject _BeachTile;
    [SerializeField] private GameObject _WaterTile;
    [SerializeField] private GameObject _OceanTile;
    [SerializeField] private GameObject _GrassTile;
    [SerializeField] private GameObject _MountainTile;
    [SerializeField] private GameObject _SnowTile;

    [Header("Details")]
    [SerializeField] private GameObject _ForestDetail;

    public void SetTiles(List<Tile> tiles)
    {
        _Tiles = tiles;
    }

    public GameObject GetTileObject(TileType type)
    {
        switch (type)
        {
            // Details
            case TileType.Forest: return _ForestDetail;
            case TileType.Mountain: return _MountainTile;

            // Tiles
            case TileType.Beach: return _BeachTile;
            case TileType.Ocean: return _OceanTile;
            case TileType.Plains: return _GrassTile;
            case TileType.Water: return _WaterTile;
            case TileType.Snow: return _SnowTile;

            default: return null;
        }
    }

    public static List<Tile> GetSurroundingTiles(Tile centerTile)
    {
        List<Tile> neighbourTiles = new List<Tile>();

        Vector2 centerPos = new Vector2(centerTile._GridPosition.x, centerTile._GridPosition.y);
        foreach(Tile tile in _Instance._Tiles)
        {
            if(Mathf.Abs(tile._GridPosition.x - centerPos.x) <= 1 && Mathf.Abs(tile._GridPosition.y - centerPos.y) <= 1)
            {
                neighbourTiles.Add(tile);
            }
        }

        return neighbourTiles;
    }

    public static TileManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType(typeof(TileManager)) as TileManager;
            }

            if (_Instance == null)
            {
                var obj = new GameObject("TileManager");
                _Instance = obj.AddComponent<TileManager>();
            }

            return _Instance;
        }
    }
}
