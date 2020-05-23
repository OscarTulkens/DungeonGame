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

    //Singleton
    public static TreasureManager Instance = null;

    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
        enabled = false;
    }
    void Start()
    {
        _controlScript = ControlScript.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        HandleTreasure();
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
            if (_amountOfTreasures>0)
            {
                GenerateTreasure(RandomTreasureType(_treasurePool.ItemChance, _treasurePool.TileChance));
                if (_firstTreasure)
                {
                    _treasureSpawnPoint.GetComponentInChildren<Animator>().SetTrigger("Open");
                    _controlScript.CurrentlySelectedTile.TileSpecialSpawnScript.SpecialSpawn.GetComponentInChildren<Animator>().SetTrigger("Open");
                    _firstTreasure = false;
                }
                else
                {
                    _treasureSpawnPoint.GetComponentInChildren<Animator>().SetTrigger("Extra Treasure");
                    _controlScript.CurrentlySelectedTile.TileSpecialSpawnScript.SpecialSpawn.GetComponentInChildren<Animator>().SetTrigger("Extra Treasure");
                }
            }
            else
            {
                _controlScript.CurrentlySelectedTile.ContainsTreasure = false;
                _treasureSpawnPoint.GetComponentInChildren<Animator>().SetTrigger("Disappear");
                _controlScript.CurrentlySelectedTile.TileSpecialSpawnScript.SpecialSpawn.GetComponentInChildren<Animator>().SetTrigger("Disappear");
                _claimed = true;
                Invoke("StopTreasure", 1f);
            }
        }
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
                InventoryManager.Instance.AddItem(RandomTreasureItem(_treasurePool));
                Debug.Log("Rewarded Item");
                break;
            case TreasureType.Coins:
                InventoryManager.Instance.ModifyCurrency(GetRandomValue(_treasurePool.MinAmountOfCoins, _treasurePool.MaxAmountOfCoins));
                Debug.Log("Rewarded Coins");
                break;
            case TreasureType.Tiles:
                InventoryManager.Instance.ModifyTileCount(GetRandomValue(_treasurePool.MinAmountOfTiles, _treasurePool.MaxAmountOfTiles));
                Debug.Log("Rewarded Tiles");
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
