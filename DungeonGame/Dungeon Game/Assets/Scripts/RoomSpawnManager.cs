using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomSpawnManager : MonoBehaviour
{
    public GameObject SpawnpointsPrefab;
    [SerializeField] private float _chanceAtMonsterPercentage = 0;
    [SerializeField] private float _chanceAtTreasure = 0;

    [HideInInspector] public List<GameObject> CornerTiles = new List<GameObject>();
    [HideInInspector] public List<GameObject> StraightTiles = new List<GameObject>();
    [HideInInspector] public List<GameObject> TSplitTiles = new List<GameObject>();
    [HideInInspector] public List<GameObject> FourSplitTiles = new List<GameObject>();
    [HideInInspector] public List<GameObject> DeadEndTiles = new List<GameObject>();

    [HideInInspector] public List<MonsterObject> MonsterPrefabs = new List<MonsterObject>();
    [HideInInspector] public List<TreasureObject> TreasurePrefabs = new List<TreasureObject>();

    public static RoomSpawnManager Instance;

    private void Awake()
    {
        Instance = this;

        foreach (GameObject straightTile in Resources.LoadAll<GameObject>("TileTypes/StraightTiles"))
        {
            StraightTiles.Add(straightTile);
        }
        foreach (GameObject fourSplitTile in Resources.LoadAll<GameObject>("TileTypes/FourSplitTiles"))
        {
            FourSplitTiles.Add(fourSplitTile);
        }
        foreach (GameObject tSplitTile in Resources.LoadAll<GameObject>("TileTypes/TSplitTiles"))
        {
            TSplitTiles.Add(tSplitTile);
        }
        foreach (GameObject cornerTile in Resources.LoadAll<GameObject>("TileTypes/CornerTiles"))
        {
            CornerTiles.Add(cornerTile);
        }
        foreach (GameObject deadEndTile in Resources.LoadAll<GameObject>("TileTypes/DeadEndTiles"))
        {
            DeadEndTiles.Add(deadEndTile);
        }

        foreach(MonsterObject monster in Resources.LoadAll<MonsterObject>("TileSpawns/Monsters"))
        {
            MonsterPrefabs.Add(monster);
        }
        foreach(TreasureObject treasure in Resources.LoadAll<TreasureObject>("TileSpawns/Treasures"))
        {
            TreasurePrefabs.Add(treasure);
        }

    }

    public void SpawnTile(TileScript tilescript, Sides requiredSides)
    {
        if (RandomInt(0,100) < _chanceAtMonsterPercentage)
        {
            tilescript.ContainsMonster = true;
        }

        else if (RandomInt(0,100) < _chanceAtTreasure)
        {
           tilescript.ContainsTreasure = true;
        }


        Transform modeltransform = tilescript.TileObject.transform;

        if (requiredSides.HasFlag(Sides.Top | Sides.Bot | Sides.Left | Sides.Right))
        {
            Instantiate(FourSplitTiles[RandomInt(0, FourSplitTiles.Count)], modeltransform.position, Quaternion.Euler(new Vector3(0, 0, 0)), modeltransform);
        }

        else if (requiredSides.HasFlag(Sides.Top | Sides.Bot | Sides.Right))
        {
            Instantiate(TSplitTiles[RandomInt(0, TSplitTiles.Count)], modeltransform.position, Quaternion.Euler(new Vector3(0, -90, 0)), modeltransform);
        }

        else if (requiredSides.HasFlag(Sides.Left | Sides.Bot | Sides.Right))
        {
            Instantiate(TSplitTiles[RandomInt(0, TSplitTiles.Count)], modeltransform.position, Quaternion.Euler(new Vector3(0, 0, 0)), modeltransform);
        }
        else if (requiredSides.HasFlag(Sides.Top | Sides.Left | Sides.Right))
        {
            Instantiate(TSplitTiles[RandomInt(0, TSplitTiles.Count)], modeltransform.position, Quaternion.Euler(new Vector3(0, 180, 0)), modeltransform);
        }
        else if (requiredSides.HasFlag(Sides.Top | Sides.Bot | Sides.Left))
        {
            Instantiate(TSplitTiles[RandomInt(0, TSplitTiles.Count)], modeltransform.position, Quaternion.Euler(new Vector3(0, 90, 0)), modeltransform);
        }

        else if (requiredSides.HasFlag(Sides.Top | Sides.Bot))
        {
            Instantiate(StraightTiles[RandomInt(0, StraightTiles.Count)], modeltransform.position, Quaternion.Euler(new Vector3(0, 90, 0)), modeltransform);
        }

        else if (requiredSides.HasFlag(Sides.Left | Sides.Right))
        {
            Instantiate(StraightTiles[RandomInt(0, StraightTiles.Count)], modeltransform.position, Quaternion.Euler(new Vector3(0, 0, 0)), modeltransform);
        }

        else if (requiredSides.HasFlag(Sides.Top | Sides.Right))
        {
            Instantiate(CornerTiles[RandomInt(0, CornerTiles.Count)], modeltransform.position, Quaternion.Euler(new Vector3(0, -90, 0)), modeltransform);
        }

        else if (requiredSides.HasFlag(Sides.Top | Sides.Left))
        {
            Instantiate(CornerTiles[RandomInt(0, CornerTiles.Count)], modeltransform.position, Quaternion.Euler(new Vector3(0, 180, 0)), modeltransform);
        }

        else if (requiredSides.HasFlag(Sides.Bot | Sides.Right))
        {
            Instantiate(CornerTiles[RandomInt(0, CornerTiles.Count)], modeltransform.position, Quaternion.Euler(new Vector3(0, 0, 0)), modeltransform);
        }

        else if (requiredSides.HasFlag(Sides.Bot | Sides.Left))
        {
            Instantiate(CornerTiles[RandomInt(0, CornerTiles.Count)], modeltransform.position, Quaternion.Euler(new Vector3(0, 90, 0)), modeltransform);
        }

        else if (requiredSides.HasFlag(Sides.Left))
        {
            Instantiate(DeadEndTiles[RandomInt(0, DeadEndTiles.Count)], modeltransform.position, Quaternion.Euler(new Vector3(0, -90, 0)), modeltransform);
        }

        else if (requiredSides.HasFlag(Sides.Top))
        {
            Instantiate(DeadEndTiles[RandomInt(0, DeadEndTiles.Count)], modeltransform.position, Quaternion.Euler(new Vector3(0, 0, 0)),modeltransform);
        }

        else if (requiredSides.HasFlag(Sides.Right))
        {
            Instantiate(DeadEndTiles[RandomInt(0, DeadEndTiles.Count)], modeltransform.position, Quaternion.Euler(new Vector3(0, 90, 0)), modeltransform);
        }

        else if (requiredSides.HasFlag(Sides.Bot))
        {
            Instantiate(DeadEndTiles[RandomInt(0, DeadEndTiles.Count)], modeltransform.transform.position, Quaternion.Euler(new Vector3(0, 180, 0)), modeltransform);
        }
    }

    //Return a random int
    int RandomInt(int minvalue, int maxvalue)
    {
        return UnityEngine.Random.Range(minvalue, maxvalue);
    }
}

[Flags]
public enum Sides
{
    NONE = 0,
    Top = 1,
    Left = 2,
    Bot = 4,
    Right = 8
}
