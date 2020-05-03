using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class TreasureManager : MonoBehaviour
{
    [SerializeField] private Camera _treasureCam = null;
    [SerializeField] private RawImage _treasureRenderTextureImage = null;
    private ControlScript _controlScript = null;
    private bool _treasure = false;
    [SerializeField] private Volume _treasurePPVolume = null;
    [SerializeField] private float _postproLerpSpeed = 0;

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
    void Start()
    {
        if (!PlayerPrefs.HasKey("Currency"))
        {
            PlayerPrefs.SetInt("Currency", 0);
        }
        UpdateText();
        Instance = this;
        _treasureCam.enabled = false;
        _treasureRenderTextureImage.enabled = false;
        _controlScript = ControlScript.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeTreasurePostPro();
        HandleTreasure();
    }

    public void StartTreasure(TreasureObject treasurePrefab)
    {
        _claimed = false;
        _treasure = true;
        _treasureObject = Instantiate(treasurePrefab.TreasurePrefab, _treasureSpawnPoint);
        SetRandomValue(treasurePrefab.MinValue, treasurePrefab.MaxValue);
        _treasureCam.enabled = true;
        _treasureRenderTextureImage.enabled = true;
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
        _treasure = false;
        Destroy(_treasureObject);
        _treasureCam.enabled = false;
        _treasureRenderTextureImage.enabled = false;
        _controlScript.enabled = true;
    }

    private void ChangeTreasurePostPro()
    {
        if (_treasure && _treasurePPVolume.weight < 1)
        {
            _treasurePPVolume.weight = Mathf.Lerp(_treasurePPVolume.weight, 1, Time.deltaTime * _postproLerpSpeed);
            if (_treasurePPVolume.weight >= 0.95)
            {
                _treasurePPVolume.weight = 1;
            }
        }
        else if (!_treasure && _treasurePPVolume.weight > 0)
        {
            _treasurePPVolume.weight = Mathf.Lerp(_treasurePPVolume.weight, 0, Time.deltaTime * _postproLerpSpeed);
            if (_treasurePPVolume.weight <= 0.05)
            {
                _treasurePPVolume.weight = 0;
            }
        }
    }
}
