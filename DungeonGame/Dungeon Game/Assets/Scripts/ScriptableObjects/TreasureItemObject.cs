using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//These are scriptableobjects for TreasureItems. These are the items the player can collect from treasurepools.
[CreateAssetMenu(fileName = "TreasureItemObject", menuName = "Inventory/TreasureItem", order = 1)]
public class TreasureItemObject : ScriptableObject
{
    public string Name;
    public Sprite InventoryImage;
    public GameObject Model;
    public ItemType ItemType;
    public int Value;
    public Rarity Rarity;
    [TextArea(15,20)]
    public string Description;
}

public enum ItemType
{
    Default,
    Helmet,
    Armor,
    Weapon,
    Offhand
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Cosmic_Infused,
    Inter_Dimensional
}
