using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    public float BaseDamage;
    public float BaseHealth;
    public float BaseSpecialPower;

    private float _equipmentDamage;
    private float _equipmentHealth;
    private float _equipmentSpecialPower;

    private float _totalDamage;
    private float _totalHealth;
    private float _totalSpecialPower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateStats(object sender, EventManager.OnUpdateEquipmentStatsArgs e)
    {
        _equipmentDamage = e.EquipmentDamage;
        _equipmentHealth = e.EquipmentHealth;
        _equipmentSpecialPower = e.EquipmentSpecialPower;
        CalculateFinalStats();
    }

    private void CalculateFinalStats()
    {
        _totalDamage = BaseDamage + _equipmentDamage;
        _totalHealth = BaseHealth + _equipmentHealth;
        _totalSpecialPower = BaseSpecialPower + _equipmentSpecialPower;
    }
}
