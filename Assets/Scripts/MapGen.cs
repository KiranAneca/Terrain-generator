using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGen : MonoBehaviour
{
    [SerializeField] private GameObject _tile;

    [Header("Cellular Automata")]
    [SerializeField] private bool _useCellularAutomata = false;
    [SerializeField][Range(0.25f, 0.5f)] private float _startWaterPercentage = 0.45f;
    [SerializeField][Range(2, 4)] private int _caIterationAmount = 3;

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

    private List<Tile> _tileMap;

    void Start()
    {
        _tileMap = new List<Tile>();
        GenerateMap();
    }

    public void GenerateMap()
    {
        TileManager tilesManager = TileManager.Instance;

        System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
        System.Type[] possible = (from System.Type type in types where type.IsSubclassOf(typeof(BaseTransition)) select type).ToArray();

        // Delete the old map
        foreach (Tile tile in _tileMap)
        {
            if(tile != null)
            {
                GameObject.Destroy(tile.gameObject);
            }
        }
        _tileMap.Clear();

        // Generate the initial map
        for (int x = 0; x < _MapSizeX; ++x)
        {
            for (int y = 0; y < _MapSizeY; ++y)
            {
                // Randomly assign a a tile to be plains or water
                GameObject obj = Instantiate(_tile, new Vector3(x, 0, y + (x % 2 / 2f)), _tile.transform.rotation);
                obj.transform.SetParent(this.transform);
                Tile tile = obj.GetComponent<Tile>();
                tile.SetGridPosition(new Vector3(obj.transform.position.x, obj.transform.position.z * 2, 0));


                if (Random.value < _startWaterPercentage)
                {
                    tile.SetTileType(TileType.Water);
                }
                else
                {
                    tile.SetTileType(TileType.Land);
                }

                _tileMap.Add(tile);
            }
        }
        // After generation we first of all set the map that is generated
        TileManager.Instance.SetTiles(_tileMap);

        // Set the neighbours of the tiles
        foreach (Tile tile in _tileMap)
        {
            tile.SetNeighbours(TileManager.Instance.GetSurroundingTiles(tile));
        }

        // Use cellular automata
        if(_useCellularAutomata)
        {
            PerformCellularAutomata();
        }

        GenerateBiomes();

        Camera.main.transform.eulerAngles = new Vector3(50, 0, 0);
    }

    public void GenerateBiomes()
    {
        // Set the offset for the noisemaps, so they aren't the same noisemap
        int mapOffset = Random.Range(1, 1000);
        int temperatureOffset = Random.Range(1, 1000);
        int rainOffset = Random.Range(1, 1000);

        int idx = -1;
        for (int x = 0; x < _MapSizeX; ++x)
        {
            for (int y = 0; y < _MapSizeY; ++y)
            {
                idx++;

                // Generate the value for every tile and set them
                float elevation = Mathf.PerlinNoise(x * _Scatter + mapOffset, y * _Scatter + mapOffset);
                float temperatureVal = Mathf.PerlinNoise(x * _BiomeScatter + temperatureOffset, y * _BiomeScatter + temperatureOffset);
                float rainVal = Mathf.PerlinNoise(x * _BiomeScatter + rainOffset, y * _BiomeScatter + rainOffset);

                _tileMap[idx].Elevation = elevation;
                _tileMap[idx].Temperature = temperatureVal;
                _tileMap[idx].Rain = rainVal;

                _tileMap[idx].DetermineBiome();
            }
        }
    }

    public void PerformCellularAutomata()
    {
        for (int i = 0; i < _caIterationAmount; i++)
        {
            // Make a buffer as to not use the changed data in the process of one iteration
            TileType[,] newTileMap = new TileType[_MapSizeX, _MapSizeY];

            int idx = -1;
            for (int x = 0; x < _MapSizeX; ++x)
            {
                for (int y = 0; y < _MapSizeY; ++y)
                {
                    idx++;

                    // Loop over all neighbours and if more then 3 are plains, make them plains as well
                    int plainsTiles = 0;
                    List<Tile> neighbours = TileManager.Instance.GetSurroundingTiles(_tileMap[idx]);
                    foreach (Tile tile in _tileMap[idx].GetNeighbours())
                    {
                        if (tile.GetTileType() == TileType.Land)
                        {
                            plainsTiles++;
                        }
                    }
                    if (plainsTiles <= 3) 
                    {
                        newTileMap[x, y] = (TileType.Water);
                    }
                    else
                    {
                        newTileMap[x, y] = (TileType.Land);
                    }
                }
            }

            idx = -1;
            // Copy over the values from the buffer, to be used
            for (int x = 0; x < _MapSizeX; ++x)
            {
                for (int y = 0; y < _MapSizeY; ++y)
                {
                    idx++; 
                    _tileMap[idx].SetTileType(newTileMap[x, y]);
                }
            }
        }
    }

    //public void GenerateMap()
    //{
    //    // Delete the old map
    //    foreach (Tile tile in _tiles)
    //    {
    //        GameObject.Destroy(tile.gameObject);
    //    }
    //    _tiles.Clear();
    //    TileManager tilesManager = TileManager.Instance;

    //    int mapOffset = Random.Range(1, 1000);
    //    int temperatureOffset = Random.Range(1, 1000);
    //    int rainOffset = Random.Range(1, 1000);

    //    for (int x = 0; x < _MapSizeX; ++x)
    //    {
    //        for (int y = 0; y < _MapSizeY; ++y)
    //        {
    //            Vector3 vec = new Vector3(x, 0, y + (x % 2 / 2f));

    //            float yVal = Mathf.PerlinNoise(x * _Scatter + mapOffset, y * _Scatter + mapOffset);
    //            float temperatureVal = Mathf.PerlinNoise(x * _BiomeScatter + temperatureOffset, y * _BiomeScatter + temperatureOffset);
    //            float rainVal = Mathf.PerlinNoise(x * _BiomeScatter + rainOffset, y * _BiomeScatter + rainOffset);

    //            if (yVal >= _MountainLevel) // Spawn mountain
    //            {
    //                SpawnTile(TileType.Mountain, vec, yVal, temperatureVal, rainVal);
    //            }
    //            else if (yVal >= _BeachLevel) // Spawn plains
    //            {
    //                SpawnTile(TileType.Plains, vec, yVal, temperatureVal, rainVal);
    //            }
    //            else if (yVal >= _SeaLevel) // Spawn beach
    //            {
    //                SpawnTile(TileType.Beach, vec, yVal, temperatureVal, rainVal);
    //            }
    //            else if (yVal >= _SeaLevel - 0.1f) // Spawn water
    //            {
    //                SpawnTile(TileType.Water, vec, yVal, temperatureVal, rainVal);
    //            }
    //            else // Spawn ocean
    //            {
    //                SpawnTile(TileType.Ocean, vec, yVal, temperatureVal, rainVal);
    //            }
    //        }
    //    }
    //    AddMapDetails();
    //    TileManager.Instance.SetTiles(_tiles);

    //    Camera.main.transform.eulerAngles = new Vector3(50, 0, 0);
    //}



    //private void AddMapDetails()
    //{
    //    // Adding forest
    //    int offset = Random.Range(1, 1000);
    //    int i = 0;
    //    foreach (Tile tile in _tiles)
    //    {
    //        if (!tile.gameObject.CompareTag("Camp"))
    //        {
    //            if (tile._TileType == TileType.Plains)
    //            {
    //                float val = Mathf.PerlinNoise(i % _MapSizeY * _ForestScatter + offset, i / _MapSizeX * _ForestScatter + offset);
    //                if (val <= _ForestDensity)
    //                {
    //                    GameObject temp = Instantiate(TileManager.Instance.GetTileObject(TileType.Forest), tile.transform.position, TileManager.Instance.GetTileObject(TileType.Forest).transform.rotation);
    //                    temp.transform.SetParent(tile.transform);
    //                    tile._TileType = TileType.Forest;
    //                }
    //            }
    //        }
    //        ++i;
    //    }
    //}

    //private void SpawnTile(TileType type, Vector3 vec, float yVal, float temperatureVal, float rainVal)
    //{
    //    TileManager tilesManager = TileManager.Instance;

    //    TileType curType = type;

    //    temperatureVal += 0.5f;
    //    temperatureVal -= yVal;

    //    // Change plains, later all tiles should be changed based on biome
    //    if(type == TileType.Plains || type == TileType.Beach)
    //    {
    //        if(temperatureVal <= _SnowDensity) // Change tile into snowtile
    //        {
    //            curType = TileType.Snow;
    //        }
    //    }

    //    GameObject temp = Instantiate(tilesManager.GetTileObject(curType), vec, tilesManager.GetTileObject(curType).transform.rotation);
    //    temp.transform.SetParent(this.transform);
    //    //temp.transform.position += new Vector3(0.0f, yVal * 3, 0.0f);

    //    // Setting the parameters
    //    Tile tile = temp.GetComponent<Tile>();
    //    tile._TileType = type;
    //    tile._GridPosition = new Vector3(temp.transform.position.x, temp.transform.position.z * 2, yVal);
    //    tile._Temperature = temperatureVal;
    //    tile._Rain = rainVal;

    //    _tiles.Add(tile);
    //}
}
