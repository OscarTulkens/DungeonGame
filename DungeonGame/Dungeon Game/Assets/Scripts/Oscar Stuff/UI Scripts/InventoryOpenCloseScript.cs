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
    public GameObject InventoryButton;
    private Vector3 _startPositionInventoryButton;

    private List<int> _onGoingTweens = new List<int>();

    private InventoryVisualManagerScript _visualInventoryManager;

    private bool _inventoryOpened = false;

    public float MovementDistance = 0;

    public GameObject PlayerModelScreen = null;

    public GameObject PlayerModel = null;
    private Vector3 _playerModelStartPosition = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInChildren<InventoryVisualManagerScript>() != null)
        {
            _visualInventoryManager = GetComponentInChildren<InventoryVisualManagerScript>();
        }

        //StartInventoryEventBind
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnStartCombat += CloseInventory;
            EventManager.Instance.OnStartCombat += DisableButton;
            EventManager.Instance.OnStartTreasure += CloseInventory;
            EventManager.Instance.OnStartTreasure += DisableButton;
            EventManager.Instance.OnEndTreasure += EnableButton;
            EventManager.Instance.OnOpenInventory += OpenInventory;
            EventManager.Instance.OnCloseInventory += CloseInventory;
            EventManager.Instance.OnOpenItemWheel += OpenItemWheel;
            EventManager.Instance.OnCloseItemWheel += CloseItemWheel;
        }

        _startPositionInventoryButton = InventoryButton.transform.position;
        _inventoryScrollRect = GetComponent<ScrollRect>();
        _inventoryScrollRect.enabled = false;
        _actionOnInventoryOpenDone += ActivateItemWheel;
        _actionOnInventoryCloseDone += DeactivateItemWheel;
        _playerModelStartPosition = PlayerModel.transform.localPosition;

        CloseItemWheel(this, EventArgs.Empty);
        EnableButton();
        ClosePlayerModelScreen();
    }

    #region Inventory Events

    public void OpenInventory(object sender, EventArgs e)
    {
        OpenPlayerModelScreen();
    }

    public void CloseInventory(object sender, EventArgs e)
    {
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
        //possible code to come
    }

    public void OpenItemWheel(object sender, EventArgs e)
    {
        _inventoryScrollRect.enabled = true;
        _onGoingTweens.Add(LeanTween.move(transform.gameObject, new Vector3(0, 0), InventoryOpenTime).setEaseOutQuint().setOnComplete(_actionOnInventoryOpenDone).id);
        if (_visualInventoryManager != null)
        {
            _visualInventoryManager.RegenerateInventory();
        }
    }

    public void CloseItemWheel(object sender, EventArgs e)
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
        _onGoingTweens.Add(LeanTween.move(InventoryButton, new Vector3(InventoryButton.transform.position.x, -MovementDistance), InventoryOpenTime / 2).setEaseOutExpo().id);
        InventoryButton.GetComponent<Button>().interactable = false;
    }

    public void EnableButton(object sender, EventArgs e)
    {
        EnableButton();
    }

    public void EnableButton()
    {
        _onGoingTweens.Add(LeanTween.move(InventoryButton, _startPositionInventoryButton, InventoryOpenTime).setEaseOutExpo().id);
        InventoryButton.GetComponent<Button>().interactable = true;
    }

    private void MoveButton()
    {
        if (!_inventoryOpened)
        {
            _onGoingTweens.Add(LeanTween.move(InventoryButton, new Vector3(InventoryButton.transform.position.x, _startPositionInventoryButton.y + MovementDistance), InventoryOpenTime).setEaseOutQuint().id);
        }

        else if (_inventoryOpened)
        {
            _onGoingTweens.Add(LeanTween.move(InventoryButton, _startPositionInventoryButton, InventoryOpenTime).setEaseOutQuint().id);
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
            OpenItemWheel(this, EventArgs.Empty);
            MoveButton();
            OpenPlayerModelScreen();
            ControlScriptOn(false);
            _inventoryOpened = true;
            EventManager.Instance.OpenInventory();

        }

        else if (_inventoryOpened)
        {
            CancelOnGoingTweens();
            CloseItemWheel(this, EventArgs.Empty);
            MoveButton();
            ClosePlayerModelScreen();
            ControlScriptOn(true);
            _inventoryOpened = false;
            EventManager.Instance.CloseInventory();
        }
    }
    #endregion

    #region Player Model Screen

    private void OpenPlayerModelScreen()
    {
        _onGoingTweens.Add(LeanTween.move(PlayerModelScreen, new Vector3(0, 0), InventoryOpenTime).setEaseOutQuint().id);
        _onGoingTweens.Add(LeanTween.moveLocal(PlayerModel, _playerModelStartPosition, InventoryOpenTime).setEaseOutQuint().id);

    }

    private void ClosePlayerModelScreen()
    {
        _onGoingTweens.Add(LeanTween.move(PlayerModelScreen, new Vector3(Screen.width + 10, 0), InventoryOpenTime).setEaseOutQuint().id);
        _onGoingTweens.Add(LeanTween.moveLocal(PlayerModel, _playerModelStartPosition + new Vector3(1.15f * (Screen.width / 828), 0), InventoryOpenTime).setEaseOutQuint().id);
    }

    #endregion

    private void SpriteSwap()
    {

    }
}
