using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGen : MonoBehaviour
{
    [SerializeField] private GameObject _FogOfWar;

    [Header("Parameters")]
    [SerializeField] private int _MapSizeX = 40;
    [SerializeField] private int _MapSizeY = 20;
    [SerializeField] [Range(0.0f, 1f)] private float _SeaLevel = 0.5f;
    [SerializeField] [Range(0.0f, 1f)] private float _BeachLevel = 0.6f;
    [SerializeField] [Range(0.0f, 1f)] private float _MountainLevel = 0.8f;
    [SerializeField] [Range(0.01f, 0.25f)] private float _Scatter = 0.34f;
    [Space(20)]

    [SerializeField] [Range(0.0f, 1f)] private float _ForestDensity = 0.5f;
    [SerializeField] [Range(0.01f, 0.25f)] private float _ForestScatter = 0.3f;

    [SerializeField] [Range(0.0f, 0.1f)] private float _RiverDensity = 0.03f;

    [Header("Biomes")]
    [SerializeField] [Range(0.01f, 1f)] private float _BiomeScatter = 0.34f;
    [Space(20)]

    [SerializeField] [Range(0.01f, 1f)] private float _SnowDensity = 0.23f;

    private List<Tile> _Tiles = new List<Tile>();


    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        // Delete the old map
        foreach (Tile tile in _Tiles)
        {
            GameObject.Destroy(tile.gameObject);
        }
        _Tiles.Clear();
        TileManager tilesManager = TileManager.Instance;

        int mapOffset = Random.Range(1, 1000);
        int temperatureOffset = Random.Range(1, 1000);
        int rainOffset = Random.Range(1, 1000);


        for (int x = 0; x < _MapSizeX; ++x)
        {
            for (int y = 0; y < _MapSizeY; ++y)
            {
                Vector3 vec = new Vector3(x, 0, y + (x % 2 / 2f));

                float yVal = Mathf.PerlinNoise(x * _Scatter + mapOffset, y * _Scatter + mapOffset);
                float temperatureVal = Mathf.PerlinNoise(x * _BiomeScatter + temperatureOffset, y * _BiomeScatter + temperatureOffset);
                float rainVal = Mathf.PerlinNoise(x * _BiomeScatter + rainOffset, y * _BiomeScatter + rainOffset);

                if (yVal >= _MountainLevel) // Spawn mountain
                {
                    SpawnTile(TileType.Mountain, vec, yVal, temperatureVal, rainVal);
                }
                else if (yVal >= _BeachLevel) // Spawn plains
                {
                    SpawnTile(TileType.Plains, vec, yVal, temperatureVal, rainVal);
                }
                else if (yVal >= _SeaLevel) // Spawn beach
                {
                    SpawnTile(TileType.Beach, vec, yVal, temperatureVal, rainVal);
                }
                else if (yVal >= _SeaLevel - 0.1f) // Spawn water
                {
                    SpawnTile(TileType.Water, vec, yVal, temperatureVal, rainVal);
                }
                else // Spawn ocean
                {
                    SpawnTile(TileType.Ocean, vec, yVal, temperatureVal, rainVal);
                }
            }
        }
        AddRivers();
        AddMapDetails();
        TileManager.Instance.SetTiles(_Tiles);

        Camera.main.transform.eulerAngles = new Vector3(50, 0, 0);
    }

    private void AddRivers()
    {
        // Currently hardcoded adding 3 rivers
        for (int i = 0; i < 3; ++i)
        {
            int randIdx = Random.Range(0, _Tiles.Count);

        }
    }

    private void AddMapDetails()
    {
        // Adding forest
        int offset = Random.Range(1, 1000);
        int i = 0;
        foreach (Tile tile in _Tiles)
        {
            if (!tile.gameObject.CompareTag("Camp"))
            {
                if (tile._TileType == TileType.Plains)
                {
                    float val = Mathf.PerlinNoise(i % _MapSizeY * _ForestScatter + offset, i / _MapSizeX * _ForestScatter + offset);
                    if (val <= _ForestDensity)
                    {
                        GameObject temp = Instantiate(TileManager.Instance.GetTileObject(TileType.Forest), tile.transform.position, TileManager.Instance.GetTileObject(TileType.Forest).transform.rotation);
                        temp.transform.SetParent(tile.transform);
                        tile._TileType = TileType.Forest;
                    }
                }
            }
            ++i;
        }
    }

    private void SpawnTile(TileType type, Vector3 vec, float yVal, float temperatureVal, float rainVal)
    {
        TileManager tilesManager = TileManager.Instance;

        TileType curType = type;

        temperatureVal += 0.5f;
        temperatureVal -= yVal;

        // Change plains, later all tiles should be changed based on biome
        if(type == TileType.Plains || type == TileType.Beach)
        {
            if(temperatureVal <= _SnowDensity) // Change tile into snowtile
            {
                curType = TileType.Snow;
            }
        }

        GameObject temp = Instantiate(tilesManager.GetTileObject(curType), vec, tilesManager.GetTileObject(curType).transform.rotation);
        temp.transform.SetParent(this.transform);
        temp.transform.position += new Vector3(0.0f, yVal * 3, 0.0f);

        // Setting the parameters
        Tile tile = temp.GetComponent<Tile>();
        tile._TileType = type;
        tile._GridPosition = new Vector3(temp.transform.position.x, temp.transform.position.z * 2, yVal);
        tile._Temperature = temperatureVal;
        tile._Rain = rainVal;

        _Tiles.Add(tile);
    }
}
