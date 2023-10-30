using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    // Singleton
    private static TileManager _Instance = null;

    private GameObject _CurrentTileMenu;
    private List<Tile> _tiles;

    [Header("Tiles")]
    [SerializeField] private Material _waterMat;
    [SerializeField] private Material _plainsMat;


    public void SetTiles(List<Tile> tiles)
    {
        _tiles = tiles;
    }

    public Material GetTileMat(TileType type)
    {
        switch (type)
        {
            case TileType.Land: return _plainsMat;
            case TileType.Water: return _waterMat;

            default: return null;
        }
    }

    public List<Tile> GetSurroundingTiles(Tile centerTile)
    {
        List<Tile> neighbourTiles = new List<Tile>();
        List<Vector2> neighbours = new List<Vector2>{ new Vector2(1, 1), new Vector2(1, -1), new Vector2(0, -2), new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(0, 2) };

        Vector2 centerPos = new Vector2(centerTile.GetGridPosition().x, centerTile.GetGridPosition().y);
        foreach(Tile tile in _Instance._tiles)
        {
            if(neighbours.Contains(new Vector2(tile.GetGridPosition().x, tile.GetGridPosition().y) - centerPos))
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
