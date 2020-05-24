using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private string _currencySaveName = null;
    [SerializeField] private string _tileSaveName = null;

    private int _currencyCount = 0;
    private int _tileCount = 0;

    private int _desiredCurrencyCount = 0;
    private int _desiredTileCount = 0;

    private float _timer;
    [SerializeField] private float _timeBetweenTriggers;


    public static CurrencyManager Instance;

    public event EventHandler<OnChangeCurrencyUIArgs> OnChangeCurrencyUI;

    public class OnChangeCurrencyUIArgs : EventArgs
    {
        public int CurrencyValue;
        public float Time;
    }

    public event EventHandler<OnChangeTileUIArgs> OnChangeTileUI;

    public class OnChangeTileUIArgs : EventArgs
    {
        public int TileValue;
        public float Time;
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey(_currencySaveName))
        {
            PlayerPrefs.SetInt(_currencySaveName, 0);
        }
        else
        {
            _desiredCurrencyCount = PlayerPrefs.GetInt(_currencySaveName);
            _currencyCount = _desiredCurrencyCount;
        }
        if (!PlayerPrefs.HasKey(_tileSaveName))
        {
            PlayerPrefs.SetInt(_tileSaveName, 0);
        }
        else
        {
            _desiredTileCount = PlayerPrefs.GetInt(_tileSaveName);
            _tileCount = _desiredTileCount;
        }

        OnChangeTileUI?.Invoke(this, new OnChangeTileUIArgs { TileValue = _tileCount, Time = _timeBetweenTriggers });
        OnChangeCurrencyUI?.Invoke(this, new OnChangeCurrencyUIArgs { CurrencyValue = _currencyCount, Time = _timeBetweenTriggers });
    }

    //Change Currency
    public void ModifyCurrency(int amount)
    {
        _desiredCurrencyCount += amount;
        PlayerPrefs.SetInt(_currencySaveName, _desiredCurrencyCount);
    }


    //Change TileCount
    public void ModifyTileCount(int amount)
    {
        _desiredTileCount += amount;
        PlayerPrefs.SetInt(_tileSaveName, _desiredTileCount);
    }

    private void Update()
    {
        if (Timer())
        {
            if (_currencyCount != _desiredCurrencyCount)
            {
                if (_currencyCount > _desiredCurrencyCount)
                {
                    UpdateCurrencyCount(-1);
                }
                else
                {
                    UpdateCurrencyCount(1);
                }
            }
            if (_tileCount != _desiredTileCount)
            {
                if (_tileCount > _desiredTileCount)
                {
                    UpdateTileCount(-1);
                }
                else
                {
                    UpdateTileCount(1);
                }
            }
            _timer = _timeBetweenTriggers;
        }
    }

    private void UpdateCurrencyCount(int changeAmount)
    {
        _currencyCount += changeAmount;
        OnChangeCurrencyUI?.Invoke(this, new OnChangeCurrencyUIArgs { CurrencyValue = _currencyCount, Time =_timeBetweenTriggers});
    }

    private void UpdateTileCount(int changeAmount)
    {
        _tileCount += changeAmount;
        OnChangeTileUI?.Invoke(this, new OnChangeTileUIArgs { TileValue = _tileCount, Time = _timeBetweenTriggers });
    }

    private bool Timer()
    {
        if (_timer>=0)
        {
            _timer -= Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }
}
