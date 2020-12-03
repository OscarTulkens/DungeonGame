using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private TreasureItemObject _helmet = null;
    private TreasureItemObject _armor = null;
    private TreasureItemObject _weapon = null;
    private TreasureItemObject _offhand = null;

    private float _damageStat = 0;
    private float _healthStat = 0;
    private float _specialPowerStat = 0;
    private float _specialPowerMax = 0;

    private void Start()
    {
        EventManager.Instance.OnChangeEquipment += EquipItem;
    }

    private void EquipItem(object sender, EventManager.OnChangeEquipmentArgs e)
    {
        switch (e.EquipmentItem.ItemType)
        {
            case ItemType.Default:
                break;
            case ItemType.Helmet:
                CalculateStatChanges(_helmet, e.EquipmentItem);
                _helmet = e.EquipmentItem;
                SetEquipmentModel(e.EquipmentItem);
                break;
            case ItemType.Armor:
                CalculateStatChanges(_armor, e.EquipmentItem);
                _armor = e.EquipmentItem;
                SetEquipmentModel(e.EquipmentItem);
                break;
            case ItemType.Weapon:
                CalculateStatChanges(_weapon, e.EquipmentItem);
                _weapon = e.EquipmentItem;
                SetEquipmentModel(e.EquipmentItem);
                break;
            case ItemType.Offhand:
                CalculateStatChanges(_offhand, e.EquipmentItem);
                _offhand = e.EquipmentItem;
                SetEquipmentModel(e.EquipmentItem);
                break;
            default:
                break;
        }

    }

    private void SetEquipmentModel(TreasureItemObject itemToEquip)
    {
        EventManager.Instance.ChangeEquipmentModel(itemToEquip.ItemType, itemToEquip.Model);
    }

    private void CalculateStatChanges(TreasureItemObject oldEquipment, TreasureItemObject newEquipment)
    {
        if (oldEquipment!=null)
        {
            _damageStat -= oldEquipment.Damage;
            _healthStat -= oldEquipment.Health;
            _specialPowerStat -= oldEquipment.SpecialPower;
            if (oldEquipment.ItemType == ItemType.Weapon)
            {
                _specialPowerMax -= (oldEquipment as TreasureItemObjectWeapon).MaxSpecialStat;
            }
        }

        if (newEquipment!=null)
        {
            _damageStat += newEquipment.Damage;
            _healthStat += newEquipment.Health;
            _specialPowerStat += newEquipment.SpecialPower;
            if (newEquipment.ItemType == ItemType.Weapon)
            {
                _specialPowerMax += (newEquipment as TreasureItemObjectWeapon).MaxSpecialStat;
            }
        }

        EventManager.Instance.UpdateEquipmentStats(_damageStat, _healthStat, _specialPowerStat, _specialPowerMax);
    }
}
