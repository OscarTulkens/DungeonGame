using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryVisualManagerScript : MonoBehaviour
{
    public GameObject InventorySlotPrefab;
    private HorizontalLayoutGroup _inventoryHorizontalLayoutGroup;
    public float Size =200;

    private RectTransform _inventorySlideBarRectTransform;

    private List<GameObject> _slotList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _inventorySlideBarRectTransform = GetComponent<RectTransform>();
        _inventoryHorizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        CreateInventorySlots();
        EventManager.Instance.OnUpdateInventoryVisuals += UpdateInventory;
        EventManager.Instance.OnUpdateInventoryItems += RegenerateInventory;
    }

    private void SetScrollBarSize()
    {
        _inventorySlideBarRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (_slotList.Count * Size) + (_inventoryHorizontalLayoutGroup.padding.left + _inventoryHorizontalLayoutGroup.padding.right) + ((_slotList.Count-1)*_inventoryHorizontalLayoutGroup.spacing));
    }

    public void CreateInventorySlots()
    {
        for (int i = 0; i < InventoryManager.Instance.PlayerInventoryObject.TreasureList.Count; i++)
        {
            GameObject _newSlot = Instantiate(InventorySlotPrefab, this.transform);
            _newSlot.GetComponent<InventorySlotScript>().SlotNumber = i;
            _slotList.Add(_newSlot);
        }
        SetScrollBarSize();
    }

    public void DeleteInventorySlots()
    {
        foreach (GameObject slot in _slotList)
        {
            Destroy(slot);
        }
        _slotList.Clear();
    }

    public void UpdateInventory(object sender, EventArgs e)
    {
        foreach (GameObject slot in _slotList)
        {
            slot.GetComponent<InventorySlotScript>().SetSlotImage();
        }
    }

    public void RegenerateInventory(object sender, EventArgs e)
    {
        DeleteInventorySlots();
        CreateInventorySlots();
    }
}
