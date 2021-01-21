using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatScript : MonoBehaviour
{
    private List<int> _activeUITweens = new List<int>();
    private List<int> _healthBarTweens = new List<int>();
    private List<int> _specialBarTweens = new List<int>();
    [SerializeField] private bool _isPlayer;
    [SerializeField] private Text _name;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _specialBar;

    private float _currentHealth = 0;
    private float _currentSpecial = 0;

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
        _healthBar.value = _currentHealth = maxhealth;
        _specialBar.value = _currentSpecial = 0;
    }

    public void UpdateHealth(float newHealth)
    {
        CancelTweens(_healthBarTweens);
        //_desiredHealth = newHealth;
        _healthBarTweens.Add(LeanTween.value(_healthBar.gameObject, _currentHealth, newHealth, 0.5f).setOnUpdate((float _currentHealth) => { _healthBar.value = _currentHealth; }).setEaseOutQuint().id);
        _currentHealth = newHealth;
    }

    public void UpdateSpecial(float currentSpecial)
    {
        if (!_isPlayer)
        {
            _currentSpecial = currentSpecial;
            _specialBar.value = _currentSpecial;
        }
        if (_isPlayer)
        {
            CancelTweens(_specialBarTweens);
            _specialBarTweens.Add(LeanTween.value(_specialBar.gameObject, _currentSpecial, currentSpecial, 0.5f).setOnUpdate((float _currentSpecial) => { _specialBar.value = _currentSpecial; }).setEaseOutQuint().id);
            _currentSpecial = currentSpecial;
            Debug.Log("HONEY IM HOOMEE");
        }

    }

    private void CancelTweens(List<int> tweenlist)
    {
        foreach (int tween in tweenlist)
        {
            LeanTween.cancel(tween);
        }
    }
}
