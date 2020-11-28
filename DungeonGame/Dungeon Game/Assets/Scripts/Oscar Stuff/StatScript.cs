using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatScript : MonoBehaviour
{
    private List<int> _activeUITweens = new List<int>();
    private List<int> _BarTweens = new List<int>();
    [SerializeField] private bool _isPlayer;
    [SerializeField] private Text _name;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _specialBar;

    private float _desiredHealth = 3;
    private float _desiredSpecial = 5;

    private float _currentHealth = 2;
    private float _currentSpecial = 7;
    float val = 0;

    private void Start()
    {
        Disable();
    }

    public void Enable()
    {
        _activeUITweens.Add(LeanTween.moveLocal(this.gameObject, Vector3.zero, 0.75f).setEaseOutBack().id);
    }

    public void Disable()
    {
        CancelTweens(_activeUITweens);
        if (_isPlayer)
        {
            _activeUITweens.Add(LeanTween.moveLocal(this.gameObject, new Vector3(Screen.width,0), 0.75f).setEaseInBack().id);
        }
        else
        {
            _activeUITweens.Add(LeanTween.moveLocal(this.gameObject, new Vector3(-Screen.width, 0), 0.75f).setEaseInBack().id);
        }

    }

    public void SetStartStats(int maxhealth, float maxspecial, string name, int currenthealth, int currentspecial)
    {
        _healthBar.maxValue = maxhealth;
        _specialBar.maxValue = maxspecial;
        _name.text = name;
        UpdateHealth(currenthealth);
    }

    public void UpdateHealth(float newHealth)
    {
        CancelTweens(_BarTweens);
        _desiredHealth = newHealth;
        _BarTweens.Add(LeanTween.value(_healthBar.gameObject, _currentHealth, newHealth, 0.5f).setOnUpdate((float _currentHealth) => { _healthBar.value = _currentHealth; }).setEaseOutQuint().id);
        _currentHealth = newHealth;
    }

    public void UpdateSpecial(float currentSpecial)
    {
        _currentSpecial = currentSpecial;
        _specialBar.value = _currentSpecial;
    }

    private void CancelTweens(List<int> tweenlist)
    {
        foreach (int tween in tweenlist)
        {
            LeanTween.cancel(tween);
        }
    }
}
