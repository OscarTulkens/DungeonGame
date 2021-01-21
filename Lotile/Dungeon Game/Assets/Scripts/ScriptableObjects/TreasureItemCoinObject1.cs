using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TreasureItemCoinObject", menuName = "Inventory/TreasureItemCoin", order = 3)]
public class TreasureItemCoinObject : TreasureItemObject
{
    [Range(1, 100)]
    public int value;
}
