using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Treasure", menuName = "TileSpawns/Treasure", order = 2)]
public class TreasureObject : ScriptableObject
{
    public GameObject TreasurePrefab;
    public string Name;
    public float Rarity;
    public int MinValue;
    public int MaxValue;
}
