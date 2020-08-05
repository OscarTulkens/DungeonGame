using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryObject _playerInventoryObject;
    [SerializeField] private BoxCollider2D _inventoryEquipPanel;
    [SerializeField] private GameObject _inventoryFullPopUpPrefab;

    public InventoryObject PlayerInventoryObject { get { return _playerInventoryObject; }}
    public BoxCollider2D InventoryEquipPanel { get { return _inventoryEquipPanel; } }

    public static InventoryManager Instance { get; private set; }

    [HideInInspector] public bool Replace = false;
    [HideInInspector] public TreasureItemObject _tempItemObject = null;

    public BoxCollider2D DeleteIconCollider = null;

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
            _tempItemObject = treasureItemObject;
            Instantiate(_inventoryFullPopUpPrefab, GameObject.Find("Canvas Main").transform).GetComponent<InventoryFullPopUpScript>();
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

    public void RemoveItem(int itemNumber)
    {
        _playerInventoryObject.TreasureList.RemoveAt(itemNumber);
    }

    public void ReplaceItem(int indexOfItemToRemove, TreasureItemObject treasureToAdd)
    {
        _playerInventoryObject.TreasureList[indexOfItemToRemove] = treasureToAdd;
        EventManager.Instance.UpdateInventoryVisuals();
    }

}
