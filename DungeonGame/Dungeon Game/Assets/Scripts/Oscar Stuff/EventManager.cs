using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static EventManager instance;

    //inventory open close
    public event EventHandler OnOpenInventory;
    public void OpenInventory()
    {
        OnOpenInventory?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnCloseInventory;
    public void CloseInventory()
    {
        OnCloseInventory?.Invoke(this, EventArgs.Empty);
    }


    //Change Equipment
    public event EventHandler<OnChangeEquipmentArgs> OnChangeEquipment;
    public class OnChangeEquipmentArgs : EventArgs
    {
        public ItemType ItemType;
        public GameObject ItemModel;
    }

    public void ChangeEquipment(ItemType itemtype, GameObject itemmodel)
    {
        OnChangeEquipment?.Invoke(this, new OnChangeEquipmentArgs { ItemType = itemtype, ItemModel = itemmodel });
    }

    //ChangeCurrencyUI
    public event EventHandler<OnChangeCurrencyUIArgs> OnChangeCurrencyUI;
    public class OnChangeCurrencyUIArgs : EventArgs
    {
        public int CurrencyValue;
        public float Time;
    }

    public void ChangeCurrencyUI(int currencyvalue, float time)
    {
        OnChangeCurrencyUI?.Invoke(this, new OnChangeCurrencyUIArgs { CurrencyValue = currencyvalue, Time = time });
    }

    // Change Tile UI
    public event EventHandler<OnChangeTileUIArgs> OnChangeTileUI;
    public class OnChangeTileUIArgs : EventArgs
    {
        public int TileValue;
        public float Time;
    }

    public void ChangeTileUI(int tilevalue, float time)
    {
        OnChangeTileUI?.Invoke(this, new OnChangeTileUIArgs { TileValue = tilevalue, Time = time });
    }

    // Start is called before the first frame update
    void Awake()
    {
        if(instance = null)
        {
            instance = this;
        }
    }
}
