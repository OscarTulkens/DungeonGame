using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Scriptable object for inventories
[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Inventory", order = 3)]
public class InventoryObject : ScriptableObject
{
    public int MaxCapacity;
    public List<TreasureItemObject> TreasureList = new List<TreasureItemObject>();
}
