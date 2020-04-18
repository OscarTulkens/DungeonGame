using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileContainedObjectScript : MonoBehaviour
{
    [SerializeField] private bool OverrideDeactivateSpawnPoints = false;

    private List<GameObject> _monsters = new List<GameObject>();
    private List<GameObject> _treasures = new List<GameObject>();

    [SerializeField] private GameObject _normalSpawnPoint = null;
    [SerializeField] private GameObject _monsterSpawnPoints = null;
    [SerializeField] private GameObject _treasureSpawnPoints = null;

    [HideInInspector] public Transform MovementPoint;
    private TileScript _tile = null;

    private void Awake()
    {
        DeactivatedAllSpawnPoints();
        _tile = GetComponentInParent<TileScript>();

    }

    // Start is called before the first frame update
    void Start()
    {
        _monsters = RoomSpawnManager.Instance.MonsterPrefabs;
        _treasures = RoomSpawnManager.Instance.TreasurePrefabs;
        ActivateTileSpawnPoints();
    }

    void ActivateMonsterSpawn()
    {
        _monsterSpawnPoints.SetActive(true);
        Transform _monsterPoint = _monsterSpawnPoints.transform.Find("MonsterPoint");
        Transform _movementPoint = _monsterSpawnPoints.transform.Find("MovePoint");
        GameObject _instantiatedMonster = Instantiate<GameObject>(_monsters[RandomInt(0, _monsters.Count)], _monsterPoint.position, _monsterPoint.rotation, _monsterPoint);
        SetMovementPoint(_movementPoint);
    }

    void ActivateTreasureSpawn()
    {
        _treasureSpawnPoints.SetActive(true);
        Transform _treasurepoint = _monsterSpawnPoints.transform.Find("TreasurePoint");
        Transform _movementPoint = _monsterSpawnPoints.transform.Find("MovePoint");
        GameObject _instantiatedTreasure = Instantiate<GameObject>(_treasures[RandomInt(0, _treasures.Count)], _treasurepoint.position, _treasurepoint.rotation, _treasurepoint);
        SetMovementPoint(_movementPoint);
    }

    void ActivateNormalSpawn()
    {
        Transform _movementPoint = _normalSpawnPoint.transform;
        _normalSpawnPoint.SetActive(true);
        MovementPoint = _normalSpawnPoint.transform;
    }

    void ActivateTileSpawnPoints()
    {
        if (_tile.ContainsMonster)
        {
            ActivateMonsterSpawn();
        }
        else if (_tile.ContainsTreasure)
        {
            ActivateTreasureSpawn();
        }
        else
        {
            ActivateNormalSpawn();
        }
    }

    void SetMovementPoint(Transform chosenSpawnPoint)
    {
        MovementPoint = chosenSpawnPoint.transform.Find("MovePoint");
    }

    void DeactivatedAllSpawnPoints()
    {
        if (!OverrideDeactivateSpawnPoints)
        {
            _treasureSpawnPoints.SetActive(false);
            _monsterSpawnPoints.SetActive(false);
            _normalSpawnPoint.SetActive(false);
        }
    }

    int RandomInt(int minvalue, int maxvalue)
    {
        return UnityEngine.Random.Range(minvalue, maxvalue);
    }
}
