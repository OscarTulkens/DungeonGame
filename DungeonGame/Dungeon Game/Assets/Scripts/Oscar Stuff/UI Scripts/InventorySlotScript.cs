using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotScript : MonoBehaviour
{
    public int SlotNumber;
    private BoxCollider2D _boxCollider2D;
    [HideInInspector] public bool BeingDragged = false;
    private GameObject _parentObject;
    private ScrollRect _scrollrect;

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _parentObject = transform.parent.gameObject;
        _scrollrect = GetComponentInParent<ScrollRect>();
        SetSlotImage();
    }

    private void Update()
    {
        DragHandler();
    }

    private void SetSlotImage()
    {
        GetComponent<Image>().sprite = InventoryManager.Instance.PlayerInventoryObject.TreasureList[SlotNumber].InventoryImage;
    }

    private void DragHandler()
    {
        if(!BeingDragged)
        {
            //if (Input.touchCount >= 1 && _boxCollider2D.bounds.Contains(Input.GetTouch(0).position))
            //{
            //    _scrollrect.enabled = false;
            //    transform.SetParent(null);
            //    BeingDragged = true;
            //}
            if (Input.GetMouseButtonDown(0) && _boxCollider2D.bounds.Contains(Input.mousePosition))
            {
                _scrollrect.enabled = false;
                transform.SetParent(_scrollrect.transform);
                BeingDragged = true;
            }
        }

        else if (BeingDragged)
        {
            //if (Input.touchCount>=1)
            //{
            //    transform.position = Input.GetTouch(0).position;
            //}

            //else if (Input.touchCount<1)
            //{
            //    CheckToEquip();
            //    transform.SetParent(_parentObject.transform);
            //    _scrollrect.enabled = true;
            //    BeingDragged = false;
            //}

            if (Input.GetMouseButton(0))
            {
                transform.position = Input.mousePosition;
            }

            else if (!Input.GetMouseButton(0))
            {
                CheckToEquip();
                transform.SetParent(_parentObject.transform);
                _scrollrect.enabled = true;
                BeingDragged = false;
            }
        }
    }

    private void CheckToEquip()
    {
        if (InventoryManager.Instance.InventoryEquipPanel.bounds.Contains(_boxCollider2D.bounds.center))
        {
            Debug.Log("Equip");
            EquipmentManager.Instance.EquipItem(InventoryManager.Instance.PlayerInventoryObject.TreasureList[SlotNumber]);
        }
    }
}
