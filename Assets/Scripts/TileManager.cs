using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    // Singleton
    private static TileManager _Instance = null;

    private List<Tile> _tiles;

    [SerializeField] private BiomeGraph _biomeDeterminator;

    public BiomeNode DetermineBiome(Tile tile)
    {
        return _biomeDeterminator.DetermineBiome(tile);
    }


    public void SetTiles(List<Tile> tiles)
    {
        _tiles = tiles;
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
