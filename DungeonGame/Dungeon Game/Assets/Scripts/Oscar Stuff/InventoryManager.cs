using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryObject _playerInventoryObject;
    [SerializeField] private BoxCollider2D _inventoryEquipPanel;

    public InventoryObject PlayerInventoryObject { get { return _playerInventoryObject; }}
    public BoxCollider2D InventoryEquipPanel { get { return _inventoryEquipPanel; } }

    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    //Add Item Methods
    public void AddItem(TreasureItemObject treasureToAdd)
    {
        AddItem(treasureToAdd, _playerInventoryObject);
    }

    public void AddItem(TreasureItemObject treasureItemObject, InventoryObject inventoryToChange)
    {
        if (inventoryToChange.TreasureList.Count<inventoryToChange.MaxCapacity)
        {
            inventoryToChange.TreasureList.Add(treasureItemObject);
        }
        else
        {
            //Code for delete item popup
        }
    }



    //Remove Item Methods
    public void RemoveItem(TreasureItemObject treasureToRemove)
    {
        RemoveItem(treasureToRemove, _playerInventoryObject);
    }

    public void RemoveItem(TreasureItemObject treasureToRemove, InventoryObject inventoryToChange)
    {
        inventoryToChange.TreasureList.Remove(treasureToRemove);
    }

}
