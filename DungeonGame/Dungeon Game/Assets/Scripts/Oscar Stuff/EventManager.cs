using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static EventManager Instance;

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

    //Inventory Update
    public event EventHandler OnUpdateInventoryVisuals;
    public void UpdateInventoryVisuals()
    {
        OnUpdateInventoryVisuals?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnUpdateInventoryItems;
    public void UpdateInventoryItems()
    {
        OnUpdateInventoryItems?.Invoke(this, EventArgs.Empty);
    }


    //Change Equipment
    public event EventHandler<OnChangeEquipmentModelArgs> OnChangeEquipmentModel;
    public class OnChangeEquipmentModelArgs : EventArgs
    {
        public ItemType ItemType;
        public GameObject ItemModel;
    }

    public void ChangeEquipmentModel(ItemType itemtype, GameObject itemmodel)
    {
        OnChangeEquipmentModel?.Invoke(this, new OnChangeEquipmentModelArgs { ItemType = itemtype, ItemModel = itemmodel });
    }



    public event EventHandler<OnChangeEquipmentArgs> OnChangeEquipment;
    public class OnChangeEquipmentArgs:EventArgs
    {
        public TreasureItemObject EquipmentItem;
    }

    public void ChangeEquipment(TreasureItemObject equipmentitem)
    {
        OnChangeEquipment?.Invoke(this, new OnChangeEquipmentArgs { EquipmentItem = equipmentitem });
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


    //Treasure Events

    public event EventHandler<OnGetTreasureItemEventArgs> OnGetTreasureItem;
    public class OnGetTreasureItemEventArgs : EventArgs
    {
        public string Name;
        public Sprite Image;
        public string Subtext;
    }

    public void GetTreasureItem(string name, Sprite image, string subtext)
    {
        OnGetTreasureItem?.Invoke(this, new OnGetTreasureItemEventArgs { Name = name, Image = image, Subtext = subtext });
    }

    public event EventHandler OnEndTreasure;

    public void EndTreasure()
    {
        OnEndTreasure?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnStartTreasure;

    public void StartTreasure()
    {
        OnStartTreasure?.Invoke(this, EventArgs.Empty);
    }


    //Combat Events

    public event EventHandler OnStartCombat;
    public void StartCombat()
    {
        OnStartCombat?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnEndCombat;
    public void EndCombat()
    {
        OnEndCombat?.Invoke(this, EventArgs.Empty);
    }

    //PopUp Events

    public event EventHandler OnCloseInventoryFullPopUp;
    public void CloseInventoryFullPopUp()
    {
        OnCloseInventoryFullPopUp?.Invoke(this, EventArgs.Empty);
    }

    //Inventory Item Wheel Events

    public event EventHandler OnOpenItemWheel;
    public void OpenItemWheel()
    {
        OnOpenItemWheel?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnCloseItemWheel;
    public void CloseItemWheel()
    {
        OnCloseItemWheel?.Invoke(this, EventArgs.Empty);
    }

    //Update Stats
    public event EventHandler<OnUpdateEquipmentStatsArgs> OnUpdateEquipmentStats;
    public class OnUpdateEquipmentStatsArgs : EventArgs
    {
        public float EquipmentDamage;
        public float EquipmentHealth;
        public float EquipmentSpecialPower;
    }

    public void UpdateEquipmentStats(float equipmentdamage, float equipmenthealth, float equipmentspecialpower)
    {
        OnUpdateEquipmentStats?.Invoke(this, new OnUpdateEquipmentStatsArgs { EquipmentDamage = equipmentdamage, EquipmentHealth = equipmenthealth, EquipmentSpecialPower = equipmentspecialpower });
    }





    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }


}
