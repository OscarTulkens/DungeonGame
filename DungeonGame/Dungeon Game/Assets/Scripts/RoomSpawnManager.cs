using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomSpawnManager : MonoBehaviour
{
    public GameObject SpawnpointsPrefab;
    [SerializeField] private float _chanceAtMonsterPercentage;
    [SerializeField] private float _chanceAtTreasure;

    [HideInInspector] public List<GameObject> CornerTiles = new List<GameObject>();
    [HideInInspector] public List<GameObject> StraightTiles = new List<GameObject>();
    [HideInInspector] public List<GameObject> TSplitTiles = new List<GameObject>();
    [HideInInspector] public List<GameObject> FourSplitTiles = new List<GameObject>();
    [HideInInspector] public List<GameObject> DeadEndTiles = new List<GameObject>();

    [HideInInspector] public List<GameObject> MonsterPrefabs = new List<GameObject>();
    [HideInInspector] public List<GameObject> TreasurePrefabs = new List<GameObject>();

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

        foreach(GameObject monster in Resources.LoadAll<GameObject>("TileSpawns/Monsters"))
        {
            MonsterPrefabs.Add(monster);
        }
        foreach(GameObject treasure in Resources.LoadAll<GameObject>("TileSpawns/Treasures"))
        {
            TreasurePrefabs.Add(treasure);
        }

    }

    public void SpawnTile(Transform tileTransform, Sides requiredSides)
    {
        if (RandomInt(0,100) < _chanceAtMonsterPercentage)
        {
            tileTransform.GetComponent<TileScript>().ContainsMonster = true;
        }

        else if (RandomInt(0,100) < _chanceAtTreasure)
        {
            tileTransform.GetComponent<TileScript>().ContainsTreasure = true;
        }


        Transform modeltransform = tileTransform.GetComponent<TileScript>().Model.transform;

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
