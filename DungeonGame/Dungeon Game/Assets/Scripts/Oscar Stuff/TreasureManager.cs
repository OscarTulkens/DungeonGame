using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureManager : MonoBehaviour
{
    private ControlScript _controlScript = null;
    private bool _treasure = false;

    //Treasure Spawn Variables
    [Header("TREASURE")]
    [SerializeField] private Transform _treasureSpawnPoint = null;
    private GameObject _treasureObject = null;
    private int _treasureValue = 0;
    private bool _claimed = false;
    private TreasurePoolObject _treasurePool = null;
    private int _amountOfTreasures = 0;
    private bool _firstTreasure = true;
    [SerializeField] private float _defaultAddTime = 0;
    private float _timeRemaining = 0;

    //Singleton
    public static TreasureManager Instance = null;

    //Events
    public event EventHandler<OnGetTreasureItemEventArgs> OnGetTreasureItem;
    public class OnGetTreasureItemEventArgs : EventArgs
    {
        public string name;
        public Sprite image;
        public string subtext;
    }

    public event EventHandler OnEndTreasure;



    private void Awake()
    {
        Instance = this;
        enabled = false;
    }
    void Start()
    {
        _controlScript = ControlScript.Instance;
    }

    void Update()
    {
        HandleTreasure();
        DoTapTimer();
    }

    public void StartTreasure(TreasureObject treasurePrefab)
    {
        //Instantiate Treasure Prefab
        _treasureObject = Instantiate(treasurePrefab.TreasurePrefab, _treasureSpawnPoint);

        //Get TreasurePool
        _treasurePool = treasurePrefab.TreasurePool;

        //Get random amount of treasure
        _amountOfTreasures = GetRandomValue(1, _treasurePool.MaxAmountOfTreasures);
    }

    private void HandleTreasure()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CheckTapTimer())
            {
                if (_amountOfTreasures > 0)
                {
                    GenerateTreasure(RandomTreasureType(_treasurePool.ItemChance, _treasurePool.TileChance));
                    if (_firstTreasure)
                    {
                        _treasureSpawnPoint.GetComponentInChildren<Animator>().SetTrigger("Open");
                        _controlScript.CurrentlySelectedTile.TileSpecialSpawnScript.SpecialSpawn.GetComponentInChildren<Animator>().SetTrigger("Open");
                        _firstTreasure = false;
                        AddTimeToTapTimer();
                    }
                    else
                    {
                        _treasureSpawnPoint.GetComponentInChildren<Animator>().SetTrigger("Extra Treasure");
                        _controlScript.CurrentlySelectedTile.TileSpecialSpawnScript.SpecialSpawn.GetComponentInChildren<Animator>().SetTrigger("Extra Treasure");
                        AddTimeToTapTimer();
                    }
                }
                else
                {
                    _controlScript.CurrentlySelectedTile.ContainsTreasure = false;
                    _treasureSpawnPoint.GetComponentInChildren<Animator>().SetTrigger("Disappear");
                    _controlScript.CurrentlySelectedTile.TileSpecialSpawnScript.SpecialSpawn.GetComponentInChildren<Animator>().SetTrigger("Disappear");
                    _claimed = true;
                    OnEndTreasure?.Invoke(this, EventArgs.Empty);
                    Invoke("StopTreasure", 0.5f);
                    AddTimeToTapTimer(1);
                }
            }
        }
    }

    //Run timer to see whether treasure visual is ready
    private bool CheckTapTimer()
    {
        if (_timeRemaining>0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void DoTapTimer()
    {
        if (_timeRemaining>0)
        {
            _timeRemaining -= Time.deltaTime;
        }
    }

    //Add time to tap timer
    private void AddTimeToTapTimer(float timeToAdd)
    {
        _timeRemaining += timeToAdd;
    }

    private void AddTimeToTapTimer()
    {
        _timeRemaining += _defaultAddTime;
    }

    //Stop the treasure Code
    private void StopTreasure()
    {
        Destroy(_treasureObject);
        Destroy(_controlScript.CurrentlySelectedTile.TileSpecialSpawnScript.SpecialSpawn);
        ControlScript.Instance.enabled = true;
        this.enabled = false;
    }

    //Get a random Int between min and max
    private int GetRandomValue(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }

    //Return which type of treasure the next one will be
    private TreasureType RandomTreasureType(int chanceForItem, int chanceForTiles)
    {
        int itemCalc = UnityEngine.Random.Range(1, 101);
        int tilesCalc = UnityEngine.Random.Range(1, 101);
        if (chanceForItem >= itemCalc)
        {
            return TreasureType.Item;
        }
        else if (chanceForTiles >= tilesCalc)
        {
            return TreasureType.Tiles;
        }
        else
        {
            return TreasureType.Coins;
        }
    }

    public enum TreasureType
    {
        Item,
        Coins,
        Tiles
    }

    private void GenerateTreasure(TreasureType type)
    {
        switch (type)
        {
            case TreasureType.Item:
                TreasureItemObject _randomTreasureItem = RandomTreasureItem(_treasurePool);
                InventoryManager.Instance.AddItem(_randomTreasureItem);
                OnGetTreasureItem.Invoke(this, new OnGetTreasureItemEventArgs { image = _randomTreasureItem.InventoryImage, name = _randomTreasureItem.name, subtext = _randomTreasureItem.Description});
                break;



            case TreasureType.Coins:
                int _randomCurrencyAmount = GetRandomValue(_treasurePool.MinAmountOfCoins, _treasurePool.MaxAmountOfCoins);
                CurrencyManager.Instance.ModifyCurrency(_randomCurrencyAmount);

                Sprite coinImage = null;
                string coinName = null;
                foreach  (ImageValue imageValue in _treasurePool.CoinImageMilestones)
                {
                    if (imageValue.ValueMileStone<_randomCurrencyAmount)
                    {
                        coinImage = imageValue.Image;
                        coinName = imageValue.Name;
                    }
                    else
                    {
                        break;
                    }
                }
                OnGetTreasureItem.Invoke(this, new OnGetTreasureItemEventArgs { image = coinImage, name = coinName, subtext = _randomCurrencyAmount.ToString()});
                break;



            case TreasureType.Tiles:
                int _randomTileAmount = GetRandomValue(_treasurePool.MinAmountOfTiles, _treasurePool.MaxAmountOfTiles);
                CurrencyManager.Instance.ModifyTileCount(_randomTileAmount);

                Sprite tileImage = null;
                string tileName = null;
                foreach (ImageValue imageValue in _treasurePool.TileImageMilestones)
                {
                    if (imageValue.ValueMileStone < _randomTileAmount)
                    {
                        tileImage = imageValue.Image;
                        tileName = imageValue.Name;
                    }
                    else
                    {
                        break;
                    }
                }
                OnGetTreasureItem.Invoke(this, new OnGetTreasureItemEventArgs { image = tileImage, name = tileName, subtext = _randomTileAmount.ToString() });
                break;



            default:
                break;
        }
        //Reduce amount of treasure left
        _amountOfTreasures -= 1;
    }

    private TreasureItemObject RandomTreasureItem(TreasurePoolObject treasurePoolObject)
    {
        return treasurePoolObject.PossibleTreasures[UnityEngine.Random.Range(0, treasurePoolObject.PossibleTreasures.Count)];
    }
}
