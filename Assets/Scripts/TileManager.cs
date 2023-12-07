using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    // Singleton
    private static TileManager _Instance = null;

    private List<Tile> _tiles;
    private List<Tile>[,] _partitionedMap;
    private Vector2Int _partitionSize;

    [SerializeField] private BiomeGraph _biomeDeterminator;

    public BiomeNode DetermineBiome(Tile tile)
    {
        return _biomeDeterminator.DetermineBiome(tile);
    }

    public void SetPartitionedMap(List<Tile>[,] value, Vector2Int partitionSize)
    {
        _partitionedMap = value;
        _partitionSize = partitionSize;
    }

    public void SetTiles(List<Tile> tiles)
    {
        _tiles = tiles;
    }

    public int FindTileInRange(Tile centerTile, int range, string valueToCheck)
    {

        for (int x = 0; x < _partitionedMap.GetLength(0); x++)
        {
            for (int y = 0; y < _partitionedMap.GetLength(1); y++)
            {
                // If the chunk is needed for the range (+1 in case the tile is on the edge of the chunk)
                if (Mathf.Abs(x - centerTile.PartionedChunk.x) <= (range / _partitionSize.x +1) && Mathf.Abs(y - centerTile.PartionedChunk.y) <= (range / _partitionSize.y + 1))
                {
                    foreach (Tile tile in _partitionedMap[x, y])
                    {
                        // If the tile is in range
                        int foundDistance = GetDistance(centerTile.GetGridPosition(), tile.GetGridPosition());
                        if( foundDistance <= range)
                        {
                            var type = tile.GetType();
                            var property = type.GetField(valueToCheck, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.GetProperty);
                            if (property == null)
                            {
                                Debug.LogError("FindTileInRange property not set in: " + tile.GetBiome().name + ". Transitionvariable is: " + valueToCheck);
                                return int.MaxValue;
                            }
                            var value = property.GetValue(tile);

                            bool found = Convert.ToBoolean(value);
                            if (found)
                            {
                                return range;
                            }
                        }
                    }
                }
            }
        }
        return int.MaxValue;
    }

    public int GetDistance(Vector2Int firstTile, Vector2Int secondTile)
    {
        int dcol = Mathf.Abs(firstTile.x - secondTile.x);
        int drow = Mathf.Abs(firstTile.y - secondTile.y);
        return dcol + Mathf.Max(0, (int) ((drow - dcol) / 2f));
    }

    public List<Tile> GetSurroundingTiles(Tile centerTile)
    {
        List<Tile> neighbourTiles = new List<Tile>();
        List<Vector2Int> neighbours = new List<Vector2Int> { new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(0, -2), new Vector2Int(-1, -1), new Vector2Int(-1, 1), new Vector2Int(0, 2) };

        Vector2Int centerPos = centerTile.GetGridPosition();
        for (int x = 0; x < _partitionedMap.GetLength(0); x++)
        {
            for (int y = 0; y < _partitionedMap.GetLength(1); y++)
            {
                // If the chunk is not further then 1 chunk away
                if (Mathf.Abs(x - centerTile.PartionedChunk.x) <= 1 && Mathf.Abs(y - centerTile.PartionedChunk.y) <= 1)
                {
                    foreach (Tile tile in _partitionedMap[x, y])
                    {
                        if (neighbours.Contains(centerPos - tile.GetGridPosition()))
                        {
                            neighbourTiles.Add(tile);
                        }
                    }
                }
            }
        }

        //// Loop over the entire map
        //foreach (Tile tile in _tileMap)
        //{
        //    if (neighbours.Contains(new Vector2(tile.GetGridPosition().x, tile.GetGridPosition().y) - centerPos))
        //    {
        //        neighbourTiles.Add(tile);
        //    }
        //}

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
