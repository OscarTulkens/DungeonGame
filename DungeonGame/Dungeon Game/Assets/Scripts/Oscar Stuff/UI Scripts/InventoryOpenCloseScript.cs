using System;
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
            CombatManagerScript.Instance.OnStartCombat += DisableOpenButton;
        }
        if (TreasureManager.Instance != null)
        {
            TreasureManager.Instance.OnStartTreasure += CloseInventory;
            TreasureManager.Instance.OnStartTreasure += DisableOpenButton;
            TreasureManager.Instance.OnEndTreasure += EnableOpenButton;
        }

        _startPositionInventoryButton = _inventoryButton.transform.position;
        _inventoryScrollRect = GetComponent<ScrollRect>();
        _inventoryScrollRect.enabled = false;
        _actionOnInventoryOpenDone += ActivateInventory;
        _actionOnInventoryCloseDone += DeactivateInventory;

        CloseInventory();
        EnableOpenButton();
    }

    public void OpenInventory(object sender, EventArgs e)
    {
        OpenInventory();
    }

    public void OpenInventory()
    {
        CancelOnGoingTweens();
        if (ControlScript.Instance != null) ControlScript.Instance.enabled = false;
        _inventoryScrollRect.enabled = true;
        _onGoingTweens.Add(LeanTween.move(transform.gameObject, new Vector3(0, 0), InventoryOpenTime).setEaseOutQuint().setOnComplete(_actionOnInventoryOpenDone).id);
        if (_visualInventoryManager!=null)
        {
            _visualInventoryManager.CreateInventorySlots();
        }
    }

    public void CloseInventory(object sender, EventArgs e)
    {
        CloseInventory();
    }

    public void CloseInventory()
    {
        CancelOnGoingTweens();
        _inventoryScrollRect.enabled = false;
        _onGoingTweens.Add(LeanTween.move(transform.gameObject, new Vector3(Screen.width + 10, 0), InventoryOpenTime).setOnComplete(_actionOnInventoryCloseDone).setEaseOutQuint().id);
    }


    private void ActivateInventory()
    {
        _inventoryScrollRect.enabled = true;
    }

    private void DeactivateInventory()
    {
        _visualInventoryManager.DeleteInventorySlots();
    }



    public void DisableOpenButton(object sender, EventArgs e)
    {
        DisableOpenButton();
    }

    public void DisableOpenButton()
    {
        _onGoingTweens.Add(LeanTween.move(_inventoryButton, new Vector3(_inventoryButton.transform.position.x, -200), InventoryOpenTime / 2).setEaseOutExpo().id);
        _inventoryButton.GetComponent<Button>().interactable = false;
    }



    public void EnableOpenButton(object sender, EventArgs e)
    {
        EnableOpenButton();
    }

    public void EnableOpenButton()
    {
        _onGoingTweens.Add(LeanTween.move(_inventoryButton, _startPositionInventoryButton, InventoryOpenTime).setEaseOutExpo().id);
        _inventoryButton.GetComponent<Button>().interactable = true;
    }



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

}
