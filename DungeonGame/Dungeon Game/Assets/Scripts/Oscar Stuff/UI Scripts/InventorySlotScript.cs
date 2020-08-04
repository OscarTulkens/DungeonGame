using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotScript : MonoBehaviour
{
    public int SlotNumber = 0;
    private BoxCollider2D _boxCollider2D = null;
    private ScrollRect _scrollrect = null;
    [SerializeField] private GameObject _draggedItemPrefab = null;

    [SerializeField] private float _holdTime = 0;
    private float _holdTimer = 0;
    private bool _doHoldTimer = false;

    [SerializeField] private float _maxDragDistance = 0;
    private Vector3 _startDragPos = new Vector3();
    private float _dragDistance = 0;

    [SerializeField] private float _closeWaitTime = 0;
    private float _closeWaitTimer = 0;
    private bool _doCloseWaitTimer = false;

    private Vector3 _startSize = new Vector3(0, 0, 0);
    [SerializeField] private float _scaleIncreaseMultiplier = 0;

    private void Start()
    {
        ResetHoldTimer();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _scrollrect = GetComponentInParent<ScrollRect>();
        SetSlotImage();
    }

    private void Update()
    {
        HandleInteraction();
    }

    public void SetSlotImage()
    {
        if (GetComponent<Image>().sprite != InventoryManager.Instance.PlayerInventoryObject.TreasureList[SlotNumber].InventoryImage)
        {
            GetComponent<Image>().sprite = InventoryManager.Instance.PlayerInventoryObject.TreasureList[SlotNumber].InventoryImage;
        }
    }

    private void StartDragHandler()
    {
        _doHoldTimer = false;
        _scrollrect.enabled = false;
        Instantiate(_draggedItemPrefab, Input.mousePosition, Quaternion.identity, transform.root).GetComponent<InventoryDraggedItemHandlerScript>().SetSlotNumber(this);
        DisableImage();
    }

    public void StopDragHandler()
    {
        _scrollrect.enabled = true;
    }

    private void DoHoldTimer()
    {
        if (Input.GetMouseButtonDown(0) && _boxCollider2D.bounds.Contains(Input.mousePosition))
        {
            _startDragPos = Input.mousePosition;
            _doHoldTimer = true;
        }
        if (_doHoldTimer)
        {
            _holdTimer -= Time.deltaTime;
            if (_holdTimer <= 0)
            {
                StartDragHandler();
            }
        }
        if (Input.GetMouseButtonUp(0) || !_boxCollider2D.bounds.Contains(Input.mousePosition) || CalculateDragDistance() >_maxDragDistance)
        {
            ResetHoldTimer();
            _doHoldTimer = false;
        }
    }

    private void ResetHoldTimer()
    {
        _holdTimer = _holdTime;
    }

    private float CalculateDragDistance()
    {
        _dragDistance = Vector3.Distance(_startDragPos, Input.mousePosition);
        Debug.Log(_dragDistance);
        return _dragDistance;
    }

    private void ReplaceItem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startDragPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0) && _boxCollider2D.bounds.Contains(Input.mousePosition) && CalculateDragDistance()<=_maxDragDistance)
        {
            InventoryManager.Instance.ReplaceItem(SlotNumber, InventoryManager.Instance._tempItemObject);
            InventoryManager.Instance.Replace = false;
            EventManager.Instance.CloseInventoryFullPopUp();
            Bounce();
        }
    }

    private void HandleInteraction()
    {
        if (InventoryManager.Instance.Replace == false)
        {
            DoHoldTimer();
        }
        else if (InventoryManager.Instance.Replace == true)
        {
            ReplaceItem();
        }
    }

    private void Bounce()
    {
        _startSize = transform.localScale;
        LeanTween.scale(gameObject, _startSize * _scaleIncreaseMultiplier, 0.5f).setEasePunch();
    }

    private void DisableImage()
    {
        //GetComponent<Image>().enabled = false;
    }
}
