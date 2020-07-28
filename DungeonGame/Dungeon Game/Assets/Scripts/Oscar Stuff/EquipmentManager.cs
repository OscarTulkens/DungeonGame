using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private TreasureItemObject _helmet;
    private TreasureItemObject _armor;
    private TreasureItemObject _weapon;
    private TreasureItemObject _offhand;

    public static EquipmentManager Instance = null;

    private void Awake()
    {
        Instance = this;
    }

    public void EquipItem(TreasureItemObject itemToEquip)
    {
        switch (itemToEquip.ItemType)
        {
            case ItemType.Default:
                break;
            case ItemType.Helmet:
                SetEquipment(itemToEquip, _helmet);
                break;
            case ItemType.Armor:
                SetEquipment(itemToEquip, _armor);
                break;
            case ItemType.Weapon:
                SetEquipment(itemToEquip, _weapon);
                break;
            case ItemType.Offhand:
                SetEquipment(itemToEquip, _offhand);
                break;
            default:
                break;
        }
    }

    private void SetEquipment(TreasureItemObject itemToEquip, TreasureItemObject equipedItem)
    {
        equipedItem = itemToEquip;
        EventManager.instance.ChangeEquipment(itemToEquip.ItemType, itemToEquip.Model);
    }
}
