using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotScript : MonoBehaviour
{
    public int SlotNumber;
    private BoxCollider2D _boxCollider2D;
    private ScrollRect _scrollrect;
    [SerializeField] private GameObject _draggedItemPrefab;

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _scrollrect = GetComponentInParent<ScrollRect>();
        SetSlotImage();
    }

    private void Update()
    {
        StartDragHandler();
    }

    private void SetSlotImage()
    {
        GetComponent<Image>().sprite = InventoryManager.Instance.PlayerInventoryObject.TreasureList[SlotNumber].InventoryImage;
    }

    private void StartDragHandler()
    {
        if (Input.GetMouseButtonDown(0) && _boxCollider2D.bounds.Contains(Input.mousePosition))
        {
            _scrollrect.enabled = false;
            Instantiate(_draggedItemPrefab, Input.mousePosition, Quaternion.identity, transform.root).GetComponent<InventoryDraggedItemHandlerScript>().SetSlotNumber(this);
        }
    }

    public void StopDragHandler()
    {
        _scrollrect.enabled = true;
    }
}
