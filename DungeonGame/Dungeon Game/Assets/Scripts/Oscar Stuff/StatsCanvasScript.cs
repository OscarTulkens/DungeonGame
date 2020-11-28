using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsCanvasScript : MonoBehaviour
{
    [SerializeField] private Text _name = null;
    [SerializeField] private Slider _healthMeter = null;
    [SerializeField] private Slider _specialPowerMeter = null;

    private Transform _camera = null;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main.transform;   
    }

    // Update is called once per frame
    void Update()
    {
        LookAtCamera();
    }

    private void SetStartSettings(int health, int maxspecialpower, string name, Color nameColor)
    {
        _healthMeter.maxValue = health;
        _healthMeter.value = health;
        _name.text = name;
        _name.color = nameColor;
    }

    private void LookAtCamera()
    {
        gameObject.transform.LookAt(_camera);
    }
}
