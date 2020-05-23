using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scriptable object for TreasurePools. These are added to enemies/treasurechests that should drop treasures.
[CreateAssetMenu(fileName = "TreasurePool", menuName = "Inventory/TreasurePool", order = 2)]
public class TreasurePoolObject : ScriptableObject
{
    [Header("Treasures")]
    [Range(1,10)]
    public int MaxAmountOfTreasures = 5;
    [Tooltip("Calculated before calculating the other chances")]
    [Range(1,100)]
    public int ItemChance;
    public List<TreasureItemObject> PossibleTreasures;
    [Space]
    [Header("Tiles")]
    [Tooltip("Calculated after checking for item")]
    [Range(1,100)]
    public int TileChance;
    public int MinAmountOfTiles;
    public int MaxAmountOfTiles;
    [Space]
    [Header("Coins")]
    public int MinAmountOfCoins;
    public int MaxAmountOfCoins;
}
