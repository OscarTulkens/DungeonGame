using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TreasureItemTileObject", menuName = "Inventory/TreasureItemTile", order = 4)]
public class TreasureItemTileObject : TreasureItemObject
{
    [Range(1, 100)]
    public int value;
}
