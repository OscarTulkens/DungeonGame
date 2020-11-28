using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStatsManagerScript : MonoBehaviour
{
    [SerializeField] private StatScript _monsterStats;
    [SerializeField] private StatScript _playerStats;
    private string _name = "Player";
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.OnStartCombat += EnableStats;
        EventManager.Instance.OnEndCombat += DisableStats;
    }

    public void SetStartStats(string _monsterName, int _monsterHealth, float _monsterAttackTimer)
    {
        _monsterStats.SetStartStats((int)_monsterHealth, _monsterAttackTimer, _monsterName, _monsterHealth, 0);
        _playerStats.SetStartStats((int)CharacterStatsManager.TotalHealth, (int)CharacterStatsManager.TotalSpecialPower, _name, (int)CharacterStatsManager.TotalHealth, 0);
    }

    public void UpdateHealthStats(int _monsterCurrentHealth, int _playerCurrentHealth)
    {
        _monsterStats.UpdateHealth(_monsterCurrentHealth);
        _playerStats.UpdateHealth(_playerCurrentHealth);
    }

    public void UpdateMonsterSpecial(float _monsterAttackTimer)
    {
        _monsterStats.UpdateSpecial(_monsterAttackTimer);
    }

    public void UpdatePlayerSpecial(float _playerSpecial)
    {

    }

    private void EnableStats(object sender, EventArgs e)
    {
        _monsterStats.Enable();
        _playerStats.Enable();
    }

    private void DisableStats(object sender, EventArgs e)
    {
        _monsterStats.Disable();
        _playerStats.Disable();
    }
}
