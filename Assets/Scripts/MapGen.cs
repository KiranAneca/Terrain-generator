using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGen : MonoBehaviour
{
    private static MapGen _instance = null;

    [SerializeField] private GameObject _tile;

    [Header("Visual")]
    [SerializeField] private float _heightMultiplier = 10f;

    [Header("Base")]
    [SerializeField][Range(0.01f, 0.35f)] private float _elevationScatter = 0.34f;
    [SerializeField][Range(0.0f, 1f)] private float _landSpawnThreshold = 0.3f;

    [Header("Cellular Automata")]
    [SerializeField] private bool _useCellularAutomata = false;
    [SerializeField][Range(2, 4)] private int _caIterationAmount = 3;

    [Header("Parameters")]
    [SerializeField] private int _mapSizeX = 40;
    [SerializeField] private int _mapSizeY = 20;

    [Space(20)]
    [SerializeField] [Range(0.01f, 0.35f)] private float _rainScatter = 0.3f;
    [SerializeField][Range(0.01f, 0.35f)] private float _temperatureScatter = 0.3f;
    [SerializeField][Range(0.01f, 0.35f)] private float _vegetationScatter = 0.3f;

    [Header("Resources")]
    [SerializeField][Range(0.01f, 0.35f)] private float _ironScatter = 0.3f;
    [SerializeField][Range(0.0f, 1f)] private float _ironSpawnThreshold = 0.5f;
    [SerializeField][Range(0.01f, 0.35f)] private float _coalScatter = 0.3f;
    [SerializeField][Range(0.0f, 1f)] private float _coalSpawnThreshold = 0.5f;

    [Header("Remapping")]
    public AnimationCurve TemperatureMapper;
    public AnimationCurve HeightMapper;
    

    private List<Tile> _tileMap;

    [SerializeField] private Vector2Int _partitionedMapSize;
    private List<Tile>[,] _partitionedMap;

    public Vector2Int PartitionedMapSize { get => _partitionedMapSize; set => _partitionedMapSize = value; }

    void Start()
    {
        _instance = this;
        _tileMap = new List<Tile>();
        GenerateMap();
    }

    public void GenerateMap()
    {
        _partitionedMap = new List<Tile>[_mapSizeX / PartitionedMapSize.x, _mapSizeY / PartitionedMapSize.y];

        TileManager tilesManager = TileManager.Instance;

        // Delete the old map
        foreach (Tile tile in _tileMap)
        {
            if(tile != null)
            {
                GameObject.Destroy(tile.gameObject);
            }
        }
        _tileMap.Clear();

        int idx = 0;
        int elevationOffset = Random.Range(1, 1000);
        // Generate the initial map
        for (int x = 0; x < _mapSizeX; ++x)
        {
            for (int y = 0; y < _mapSizeY; ++y)
            {
                idx++;

                _partitionedMap[x / PartitionedMapSize.x, y / PartitionedMapSize.y] = new List<Tile>();
                float elevation = Mathf.PerlinNoise(x * _elevationScatter + elevationOffset, y * _elevationScatter + elevationOffset);

                // Randomly assign a a tile to be plains or water
                GameObject obj = Instantiate(_tile, new Vector3(x, 0, y + (x % 2 / 2f)), _tile.transform.rotation);
                obj.transform.SetParent(this.transform);

                Tile tile = obj.GetComponent<Tile>();
                tile.SetGridPosition(new Vector3(obj.transform.position.x, obj.transform.position.z * 2, 0));
                tile.RawElevation = elevation;

                if (elevation < _landSpawnThreshold)
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

        PartitionMap();

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

        // Recalculate the waterline and set the heights
        foreach (Tile tile in _tileMap)
        {
            if(tile.IsWater) 
            {
                tile.RawElevation = _landSpawnThreshold;
            }
            tile.VisualizeElevation(_heightMultiplier);
        }

        GenerateBiomes();

        Camera.main.transform.eulerAngles = new Vector3(50, 0, 0);
    }

    private void PartitionMap()
    {
        foreach (Tile tile in _tileMap)
        {
            int x = (int)tile.GetGridPosition().x / PartitionedMapSize.x;
            int y = ((int)tile.GetGridPosition().y / 2) / PartitionedMapSize.y;
            _partitionedMap[x,y].Add(tile);

            tile.PartionedChunk = new Vector2Int(x, y);
        }
        TileManager.Instance.SetPartitionedMap(_partitionedMap, _partitionedMapSize);
    }

    public void GenerateBiomes()
    {
        // Set the offset for the noisemaps, so they aren't the same noisemap
        int temperatureOffset = Random.Range(1, 1000);
        int rainOffset = Random.Range(1, 1000);
        int vegetationOffset = Random.Range(1, 1000);
        int coalOffset = Random.Range(1, 1000);
        int IronOffset = Random.Range(1, 1000);

        int idx = -1;
        for (int x = 0; x < _mapSizeX; ++x)
        {
            for (int y = 0; y < _mapSizeY; ++y)
            {
                idx++;

                // Generate the value for every tile and set them
                float temperatureVal = Mathf.PerlinNoise(x * _temperatureScatter + temperatureOffset, y * _temperatureScatter + temperatureOffset);
                float rainVal = Mathf.PerlinNoise(x * _rainScatter + rainOffset, y * _rainScatter + rainOffset);
                float vegetationVal = Mathf.PerlinNoise(x * _vegetationScatter + vegetationOffset, y * _vegetationScatter + vegetationOffset);

                float coalVal = Mathf.PerlinNoise(x * _coalScatter + coalOffset, y * _coalScatter + coalOffset);
                if(coalVal < _coalSpawnThreshold) 
                { 
                    coalVal = 0;
                }
                else
                {
                    coalVal = Utility.NormalizeValueToNormalRange(_coalSpawnThreshold, 1, coalVal);
                }

                float ironVal = Mathf.PerlinNoise(x * _ironScatter + IronOffset, y * _ironScatter + IronOffset);
                if (ironVal < _ironSpawnThreshold) 
                { 
                    ironVal = 0;
                }
                else
                {
                    ironVal = Utility.NormalizeValueToNormalRange(_ironSpawnThreshold, 1, ironVal);
                }
                _tileMap[idx].RawTemperature = temperatureVal;
                _tileMap[idx].RawVegetation = vegetationVal;
                _tileMap[idx].RawRain = rainVal;
                _tileMap[idx].RawCoal = coalVal;
                _tileMap[idx].RawIron= ironVal;

                _tileMap[idx].DetermineBiome();
            }
        }
    }

    public void PerformCellularAutomata()
    {
        for (int i = 0; i < _caIterationAmount; i++)
        {
            // Make a buffer as to not use the changed data in the process of one iteration
            Tile[,] newTileMap = new Tile[_mapSizeX, _mapSizeY];


            int idx = -1;
            for (int x = 0; x < _mapSizeX; ++x)
            {
                for (int y = 0; y < _mapSizeY; ++y)
                {
                    idx++;
                    newTileMap[x, y] = new Tile();
                    newTileMap[x, y].RawElevation = _tileMap[idx].RawElevation;

                    // Loop over all neighbours and if more then 3 are plains, make them plains as well
                    int plainsTiles = 0;
                    float cumHeight = 0;
                    int loop = 0;
                    foreach (Tile tile in _tileMap[idx].GetNeighbours())
                    {
                        loop++;
                        cumHeight += tile.RawElevation;
                        if (tile.GetTileType() == TileType.Land)
                        {
                            plainsTiles++;
                        }
                    }
                    if (plainsTiles <= 3) 
                    {
                        newTileMap[x, y].SetTileType(TileType.Water);
                    }
                    else
                    {
                        // If the terrain changed to land, set it to the average of the neighbours
                        if (_tileMap[idx].GetTileType() != TileType.Land)
                        {
                            newTileMap[x, y].RawElevation = cumHeight / loop;
                        }
                        newTileMap[x, y].SetTileType(TileType.Land);
                    }
                }
            }

            idx = -1;
            // Copy over the values from the buffer, to be used
            for (int x = 0; x < _mapSizeX; ++x)
            {
                for (int y = 0; y < _mapSizeY; ++y)
                {
                    idx++; 
                    _tileMap[idx].SetTileType(newTileMap[x, y].GetTileType());
                    _tileMap[idx].RawElevation = newTileMap[x, y].RawElevation;
                }
            }
        }
    }

    public List<Tile>[,] GetPartitionedMap()
    {
        return _partitionedMap;
    }

    public static MapGen Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(MapGen)) as MapGen;
            }

            if (_instance == null)
            {
                var obj = new GameObject("Map");
                _instance = obj.AddComponent<MapGen>();
            }

            return _instance;
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
