using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotScript : MonoBehaviour
{
    public int SlotNumber;

    private void Start()
    {
        SetSlotImage();
    }

    private void SetSlotImage()
    {
        GetComponent<Image>().sprite = InventoryManager.Instance.PlayerInventoryObject.TreasureList[SlotNumber].InventoryImage;
    }
}
