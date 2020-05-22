using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TreasurePool", menuName = "Inventory/TreasurePool", order = 2)]
public class TreasurePoolObject : ScriptableObject
{
    [Range(1,10)]
    public int AmountOfTreasures;
    public List<TreasureItemObject> PossibleTreasures;
    public int MinTiles;
    public int MaxTiles;
    public int MinCoins;
    public int MaxCoins;
}
