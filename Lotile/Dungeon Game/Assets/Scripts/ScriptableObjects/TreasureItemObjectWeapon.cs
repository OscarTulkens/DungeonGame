using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TreasureItemObjectWeapon", menuName = "Inventory/TreasureItemWeapon", order = 2)]
public class TreasureItemObjectWeapon : TreasureItemObject
{
    [Header("Weapon Specific Stats")]
    public int SpecialDamage;
    public int MaxSpecialStat;
}
