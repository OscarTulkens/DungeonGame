using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    public float BaseDamage = 0;
    public float BaseHealth = 0;
    public float BaseSpecialPower = 0;
    public float BaseSpecialPowerMax = 0;

    private float _equipmentDamage = 0;
    private float _equipmentHealth = 0;
    private float _equipmentSpecialPower = 0;
    private float _equipmentSpecialPowerMax = 0;

    [HideInInspector] public static float TotalDamage = 0;
    [HideInInspector] public static float TotalHealth = 0;
    [HideInInspector] public static float TotalSpecialPower = 0;
    [HideInInspector] public static float TotalSpecialPowerMax = 0;

    // Start is called before the first frame update
    void Start()
    {
        CalculateFinalStats();
        EventManager.Instance.OnUpdateEquipmentStats += UpdateStats;
    }

    private void UpdateStats(object sender, EventManager.OnUpdateEquipmentStatsArgs e)
    {
        _equipmentDamage = e.EquipmentDamage;
        _equipmentHealth = e.EquipmentHealth;
        _equipmentSpecialPower = e.EquipmentSpecialPower;
        _equipmentSpecialPowerMax = e.EquipmentSpecialPowerMax;
        CalculateFinalStats();
    }

    private void CalculateFinalStats()
    {
        TotalDamage = BaseDamage + _equipmentDamage;
        TotalHealth = BaseHealth + _equipmentHealth;
        TotalSpecialPower = BaseSpecialPower + _equipmentSpecialPower;
        TotalSpecialPowerMax = BaseSpecialPowerMax + _equipmentSpecialPowerMax;
    }
}
