using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileContainedObjectScript : MonoBehaviour
{
    [SerializeField] private bool OverrideDeactivateSpawnPoints = false;

    private List<MonsterObject> _monsters = new List<MonsterObject>();
    private MonsterObject _chosenMonster;
    private List<TreasureObject> _treasures = new List<TreasureObject>();
    private TreasureObject _chosenTreasure;

    [SerializeField] private GameObject _specialObjectSpawnPoint = null;

    public Transform MovementPoint;
    private TileScript _tile = null;
    public GameObject Model;

    [HideInInspector] public GameObject SpecialSpawn;

    [SerializeField] private Vector3 _modelSpawnOffset = Vector3.zero;
    [SerializeField] private float _tileMoveUpTime = 0;

    private void Awake()
    {
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
            SetMovementPoint(MovementPoint.transform);
        }
    }

    private void Update()
    {
        //if (Vector3.Distance(Model.transform.position, new Vector3(Model.transform.position.x, 0, Model.transform.position.z)) >= 0.01f)
        //{
        //    Model.transform.position = Vector3.Lerp(Model.transform.position, new Vector3(Model.transform.position.x, 0, Model.transform.position.z), _tileMoveUpSpeed * Time.deltaTime);
        //}
        //else if (Model.transform.position != new Vector3(Model.transform.position.x, 0, Model.transform.position.z))
        //{
        //    Model.transform.position = new Vector3(Model.transform.position.x, 0, Model.transform.position.z);
        //}
    }

    void ActivateMonsterSpawn()
    {
        GameObject _instantiatedMonster = Instantiate(RandomMonster().MonsterPrefab, _specialObjectSpawnPoint.transform.position, _specialObjectSpawnPoint.transform.rotation, Model.transform);
        SpecialSpawn = _instantiatedMonster;
        SetMovementPoint(MovementPoint);
        ControlScript.Instance.AddDesiredPosition(MovementPoint.position);
        Invoke("StartCombat", 0.3f);
        ControlScript.Instance.enabled = false;
        CombatManagerScript.Instance.enabled = true;
    }

    void ActivateTreasureSpawn()
    {
        GameObject _instantiatedTreasure = Instantiate(RandomTreasure().TreasurePrefab, _specialObjectSpawnPoint.transform.position, _specialObjectSpawnPoint.transform.rotation, Model.transform);
        SpecialSpawn = _instantiatedTreasure;
        SetMovementPoint(MovementPoint);
        ControlScript.Instance.AddDesiredPosition(MovementPoint.position);
        Invoke("StartTreasure", 0.3f);
        ControlScript.Instance.enabled = false;
        TreasureManager.Instance.enabled = true;
    }

    void ActivateNormalSpawn()
    {
        Transform _movementPoint = MovementPoint.transform;
        MovementPoint = MovementPoint.transform;
        ControlScript.Instance.AddDesiredPosition(MovementPoint.position);
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

    void SetModelPosition()
    {
        Model.transform.position = new Vector3(Model.transform.position.x+_modelSpawnOffset.x, Model.transform.position.y+_modelSpawnOffset.y, Model.transform.position.z+_modelSpawnOffset.z);
        LeanTween.move(Model, new Vector3(Model.transform.position.x, 0, Model.transform.position.z), _tileMoveUpTime).setEaseOutExpo();
    }

    int RandomInt(int minvalue, int maxvalue)
    {
        return UnityEngine.Random.Range(minvalue, maxvalue);
    }

    MonsterObject RandomMonster()
    {
        _chosenMonster = _monsters[Random.Range(0, _monsters.Count)];
        return _chosenMonster;
    }

    TreasureObject RandomTreasure()
    {
        _chosenTreasure = _treasures[RandomInt(0, _treasures.Count)];
        return _chosenTreasure;
    }

    void StartCombat()
    {
        CombatManagerScript.Instance.StartCombat(_chosenMonster);
    }

    void StartTreasure()
    {
        TreasureManager.Instance.StartTreasure(_chosenTreasure);
    }
}
