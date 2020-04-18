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

    public Transform MovementPoint;
    private TileScript _tile = null;
    public GameObject Model;

    [SerializeField] private Vector3 _modelSpawnOffset;
    [SerializeField] private float _tileMoveUpSpeed;

    private void Awake()
    {
        DeactivateAllSpawnPoints();
        _tile = GetComponentInParent<TileScript>();

    }

    // Start is called before the first frame update
    void Start()
    {
        if(!_tile.StartTile)
        {
            _monsters = RoomSpawnManager.Instance.MonsterPrefabs;
            _treasures = RoomSpawnManager.Instance.TreasurePrefabs;
            ActivateTileSpawnPoints();
            SetModelPosition();
        }
        else
        {
            SetMovementPoint(_normalSpawnPoint.transform);
        }
    }

    private void Update()
    {
        if (Vector3.Distance(Model.transform.position, new Vector3(Model.transform.position.x, 0, Model.transform.position.z)) >= 0.01f)
        {
            Model.transform.position = Vector3.Lerp(Model.transform.position, new Vector3(Model.transform.position.x, 0, Model.transform.position.z), _tileMoveUpSpeed * Time.deltaTime);
        }
        else if (Model.transform.position != new Vector3(Model.transform.position.x, 0, Model.transform.position.z))
        {
            Model.transform.position = new Vector3(Model.transform.position.x, 0, Model.transform.position.z);
        }
    }

    void ActivateMonsterSpawn()
    {
        _monsterSpawnPoints.SetActive(true);
        Transform _monsterPoint = _monsterSpawnPoints.transform.Find("MonsterPoint");
        Transform _monsterMovementPoint = _monsterSpawnPoints.transform.Find("MovePoint");
        GameObject _instantiatedMonster = Instantiate<GameObject>(_monsters[RandomInt(0, _monsters.Count)], _monsterPoint.position, _monsterPoint.rotation, Model.transform);
        SetMovementPoint(_monsterMovementPoint);
        ControlScript.Instance.AddDesiredPosition(_monsterMovementPoint.position);
    }

    void ActivateTreasureSpawn()
    {
        _treasureSpawnPoints.SetActive(true);
        Transform _treasurepoint = _treasureSpawnPoints.transform.Find("TreasurePoint").transform;
        Transform _treasureMovementPoint = _treasureSpawnPoints.transform.Find("MovePoint").transform;
        GameObject _instantiatedTreasure = Instantiate<GameObject>(_treasures[RandomInt(0, _treasures.Count)], _treasurepoint.position, _treasurepoint.rotation, Model.transform);
        SetMovementPoint(_treasureMovementPoint);
        ControlScript.Instance.AddDesiredPosition(_treasureMovementPoint.position);
    }

    void ActivateNormalSpawn()
    {
        Transform _movementPoint = _normalSpawnPoint.transform;
        _normalSpawnPoint.SetActive(true);
        MovementPoint = _normalSpawnPoint.transform;
        ControlScript.Instance.AddDesiredPosition(_movementPoint.position);
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
        MovementPoint = chosenSpawnPoint;
    }

    void DeactivateAllSpawnPoints()
    {
        if (!OverrideDeactivateSpawnPoints)
        {
            _treasureSpawnPoints.SetActive(false);
            _monsterSpawnPoints.SetActive(false);
            _normalSpawnPoint.SetActive(false);
        }
    }

    void SetModelPosition()
    {
        Model.transform.position = new Vector3(Model.transform.position.x+_modelSpawnOffset.x, Model.transform.position.y+_modelSpawnOffset.y, Model.transform.position.z+_modelSpawnOffset.z);
    }

    int RandomInt(int minvalue, int maxvalue)
    {
        return UnityEngine.Random.Range(minvalue, maxvalue);
    }
}
