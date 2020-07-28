using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyVisualUIScript : MonoBehaviour
{
    [SerializeField] private Text _currencyText = null;
    [SerializeField] private Text _tileText = null;

    private void Start()
    {
        EventManager.Instance.OnChangeCurrencyUI += UpdateCurrencyUI;
        EventManager.Instance.OnChangeTileUI += UpdateTileUI;
    }


    private void UpdateCurrencyUI(object sender, EventManager.OnChangeCurrencyUIArgs e)
    {
        _currencyText.text = e.CurrencyValue.ToString();
        LeanTween.move(_currencyText.gameObject, _currencyText.rectTransform.position, e.Time).setEaseInOutBack();
    }

    private void UpdateTileUI(object sender, EventManager.OnChangeTileUIArgs e)
    {
        _tileText.text = e.TileValue.ToString();
        LeanTween.move(_tileText.gameObject, _tileText.rectTransform.position, e.Time).setEaseInOutBack();
    }
}
