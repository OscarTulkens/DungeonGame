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

    //Currency Variables
    [SerializeField] private Text _currencyText = null;

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
        if (!PlayerPrefs.HasKey("Currency"))
        {
            PlayerPrefs.SetInt("Currency", 0);
        }
        UpdateText();
        _controlScript = ControlScript.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        HandleTreasure();
    }

    public void StartTreasure(TreasureObject treasurePrefab)
    {
        _claimed = false;
        _treasure = true;
        _treasureObject = Instantiate(treasurePrefab.TreasurePrefab, _treasureSpawnPoint);
        SetRandomValue(treasurePrefab.MinValue, treasurePrefab.MaxValue);
    }

    private void SetRandomValue(int min, int max)
    {
        _treasureValue = Random.Range(min, max + 1);
    }

    private void HandleTreasure()
    {
        if (_treasure)
        {
            if (Input.GetMouseButtonDown(0) && _claimed == false)
            {
                PlayerPrefs.SetInt("Currency", PlayerPrefs.GetInt("Currency") + _treasureValue);
                PlayerPrefs.Save();
                UpdateText();
                _treasureSpawnPoint.GetComponentInChildren<Animator>().SetTrigger("Open");
                _controlScript.CurrentlySelectedTile.TileSpecialSpawnScript.SpecialSpawn.GetComponentInChildren<Animator>().SetTrigger("Open");
                _controlScript.CurrentlySelectedTile.ContainsTreasure = false;
                Invoke("StopTreasure", 1f);
                _claimed = true;
            }
        }
    }

    private void UpdateText()
    {
        _currencyText.text = PlayerPrefs.GetInt("Currency").ToString();
    }

    private void StopTreasure()
    {
        Destroy(_treasureObject);
        _controlScript.enabled = true;
        _treasure = false;
        this.enabled = false;
    }
}
