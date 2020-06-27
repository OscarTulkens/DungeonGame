﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryOpenCloseScript : MonoBehaviour
{
    private Action _actionOnInventoryOpenDone;
    private Action _actionOnInventoryCloseDone;
    public float InventoryOpenTime;
    private ScrollRect _inventoryScrollRect;
    public GameObject _inventoryButton;
    private Vector3 _startPositionInventoryButton;

    private List<int> _onGoingTweens = new List<int>();

    private InventoryVisualManagerScript _visualInventoryManager;

    private bool _inventoryOpened = false;

    public float MovementDistance = 0;

    public GameObject _playerModelScreen = null;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInChildren<InventoryVisualManagerScript>()!=null)
        {
            _visualInventoryManager = GetComponentInChildren<InventoryVisualManagerScript>();
        }

        //StartInventoryEventBind
        if (CombatManagerScript.Instance != null)
        {
            CombatManagerScript.Instance.OnStartCombat += CloseInventory;
            CombatManagerScript.Instance.OnStartCombat += DisableButton;
        }
        if (TreasureManager.Instance != null)
        {
            TreasureManager.Instance.OnStartTreasure += CloseInventory;
            TreasureManager.Instance.OnStartTreasure += DisableButton;
            TreasureManager.Instance.OnEndTreasure += EnableButton;
        }

        _startPositionInventoryButton = _inventoryButton.transform.position;
        _inventoryScrollRect = GetComponent<ScrollRect>();
        _inventoryScrollRect.enabled = false;
        _actionOnInventoryOpenDone += ActivateItemWheel;
        _actionOnInventoryCloseDone += DeactivateItemWheel;

        CloseItemWheel();
        EnableButton();
        _playerModelScreen.transform.position = new Vector3(Screen.width + 10, 0);

    }

    #region Inventory Events

    public void OpenInventory(object sender, EventArgs e)
    {
        CancelOnGoingTweens();
        OpenItemWheel();
        OpenPlayerModelScreen();
    }

    public void CloseInventory(object sender, EventArgs e)
    {
        CancelOnGoingTweens();
        CloseItemWheel();
        ClosePlayerModelScreen();
    }

    #endregion

    #region Item Wheel

    private void ActivateItemWheel()
    {
        _inventoryScrollRect.enabled = true;
    }

    private void DeactivateItemWheel()
    {
        _visualInventoryManager.DeleteInventorySlots();
    }

    public void OpenItemWheel()
    {
        _inventoryScrollRect.enabled = true;
        _onGoingTweens.Add(LeanTween.move(transform.gameObject, new Vector3(0, 0), InventoryOpenTime).setEaseOutQuint().setOnComplete(_actionOnInventoryOpenDone).id);
        if (_visualInventoryManager != null)
        {
            _visualInventoryManager.CreateInventorySlots();
        }
    }

    public void CloseItemWheel()
    {
        _inventoryScrollRect.enabled = false;
        _onGoingTweens.Add(LeanTween.move(transform.gameObject, new Vector3(0, -MovementDistance), InventoryOpenTime).setOnComplete(_actionOnInventoryCloseDone).setEaseOutQuint().id);
    }

    #endregion

    #region Change Button

    public void DisableButton(object sender, EventArgs e)
    {
        DisableButton();
    }

    public void DisableButton()
    {
        _onGoingTweens.Add(LeanTween.move(_inventoryButton, new Vector3(_inventoryButton.transform.position.x, -MovementDistance), InventoryOpenTime / 2).setEaseOutExpo().id);
        _inventoryButton.GetComponent<Button>().interactable = false;
    }

    public void EnableButton(object sender, EventArgs e)
    {
        EnableButton();
    }

    public void EnableButton()
    {
        _onGoingTweens.Add(LeanTween.move(_inventoryButton, _startPositionInventoryButton, InventoryOpenTime).setEaseOutExpo().id);
        _inventoryButton.GetComponent<Button>().interactable = true;
    }

    private void MoveButton()
    {
        if (!_inventoryOpened)
        {
            _onGoingTweens.Add(LeanTween.move(_inventoryButton, new Vector3(_inventoryButton.transform.position.x, _startPositionInventoryButton.y + MovementDistance), InventoryOpenTime).setEaseOutQuint().id);
        }

        else if (_inventoryOpened)
        {
            _onGoingTweens.Add(LeanTween.move(_inventoryButton, _startPositionInventoryButton, InventoryOpenTime).setEaseOutQuint().id);
        }
    }

    #endregion

    #region Miscellaneous 

    public void ControlScriptOn(bool _bool)
    {
        ControlScript.Instance.enabled = _bool;
    }

    public void CancelOnGoingTweens()
    {
        foreach (int tween in _onGoingTweens)
        {
            LeanTween.cancel(tween);
        }
    }

    #endregion

    #region Inventory Button Interaction
    public void InventoryButtonPressed()
    {
        if (!_inventoryOpened)
        {
            CancelOnGoingTweens();
            OpenItemWheel();
            MoveButton();
            OpenPlayerModelScreen();
            ControlScriptOn(false);
            _inventoryOpened = true;
        }

        else if (_inventoryOpened)
        {
            CancelOnGoingTweens();
            CloseItemWheel();
            MoveButton();
            ClosePlayerModelScreen();
            ControlScriptOn(true);
            _inventoryOpened = false;
        }
    }
    #endregion

    #region Player Model Screen

    private void OpenPlayerModelScreen()
    {
        _onGoingTweens.Add(LeanTween.move(_playerModelScreen, new Vector3(0, 0), InventoryOpenTime).setEaseOutQuint().id);
    }

    private void ClosePlayerModelScreen()
    {
        _onGoingTweens.Add(LeanTween.move(_playerModelScreen, new Vector3(Screen.width + 10, 0), InventoryOpenTime).setEaseOutQuint().id);
    }

    #endregion

    private void SpriteSwap()
    {

    }
}
