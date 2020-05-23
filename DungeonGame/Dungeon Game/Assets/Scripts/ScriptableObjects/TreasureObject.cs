using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scriptable object for TreasureObjects. These are the possible treasures that can be spawned on tiles.
[CreateAssetMenu(fileName = "Treasure", menuName = "TileSpawns/Treasure", order = 2)]
public class TreasureObject : ScriptableObject
{
    public GameObject TreasurePrefab;
    public string Name;
    public TreasurePoolObject TreasurePool;
}
