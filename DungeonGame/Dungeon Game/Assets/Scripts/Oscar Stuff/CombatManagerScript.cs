using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CombatManagerScript : MonoBehaviour
{

    //general variables
    private bool _combat = false;
    [SerializeField] private float _movementSpeed = 0;
    [SerializeField] private float _attackMovementSpeed = 0;
    private ControlScript _controlScript = null;

    //Player variables
    [Header("PLAYER")]
    [SerializeField] private Transform _playerSpawnPoint = null;
    [SerializeField] private Transform _playerFightPoint = null;
    [SerializeField] private GameObject _playerObject = null;


    //Monster variables
    [Header("MONSTER")]
    [SerializeField] private Transform _monsterSpawnPoint = null;
    [SerializeField] private Transform _monsterFightPoint = null;
    [SerializeField] private GameObject _monsterObject = null;
    private GameObject _monsterModel = null;
    private int _monsterHealth = 0;

    //Singleton
    public static CombatManagerScript Instance = null;

    //Combat Variables
    private bool _slideInDone = false;

    private bool _playerAttacked = false;
    private bool _playerHitMonster = false;

    private void Awake()
    {
        Instance = this;
        enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _controlScript = ControlScript.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        SlideInCombat();
        ManageCombat();
    }

    public void StartCombat(MonsterObject monsterobject)
    {
        _monsterHealth = monsterobject.MonsterHealth;
        _monsterModel = Instantiate(monsterobject.MonsterPrefab, _monsterObject.transform.position, _monsterObject.transform.rotation, _monsterObject.transform);
        _monsterObject.transform.position = _monsterSpawnPoint.position;
        _playerObject.transform.position = _playerSpawnPoint.position;
        _combat = true;
    }

    private void SlideInCombat()
    {
        if (_combat && !_slideInDone)
        {
            if (Vector3.Distance(_playerObject.transform.position, _playerFightPoint.transform.position) >= 0.02f)
            {
                _playerObject.transform.position = Vector3.Lerp(_playerObject.transform.position, _playerFightPoint.position, Time.deltaTime * _movementSpeed);
            }
            if (Vector3.Distance(_monsterObject.transform.position, _monsterFightPoint.transform.position) >= 0.02f)
            {
                _monsterObject.transform.position = Vector3.Lerp(_monsterObject.transform.position, _monsterFightPoint.position, Time.deltaTime * _movementSpeed);
            }
            else
            {
                _slideInDone = true;
            }
        }

        else if (!_combat && _slideInDone)
        {
            if (Vector3.Distance(_playerObject.transform.position, _playerSpawnPoint.transform.position) >= 0.02f)
            {
                _playerObject.transform.position = Vector3.Lerp(_playerObject.transform.position, _playerSpawnPoint.position, Time.deltaTime * _movementSpeed);
            }
            if (Vector3.Distance(_monsterObject.transform.position, _monsterSpawnPoint.transform.position) >= 0.02f)
            {
                _monsterObject.transform.position = Vector3.Lerp(_monsterObject.transform.position, _monsterSpawnPoint.position, Time.deltaTime * _movementSpeed);
            }
            else
            {
                _slideInDone = false;
                Destroy(_monsterModel);
                _monsterObject.transform.position = _monsterSpawnPoint.position;
                _playerObject.transform.position = _playerSpawnPoint.position;
                this.enabled = false;
            }
        }
    }

    private void EndCombat()
    {
        _controlScript.enabled = true;
        _combat = false;
        _controlScript.CurrentlySelectedTile.TileSpecialSpawnScript.SpecialSpawn.GetComponentInChildren<Animator>().SetTrigger("Disappear");
        _controlScript.CurrentlySelectedTile.ContainsMonster = false;
    }

    private void ManageCombat()
    {
        if (_combat)
        {
            InputPlayerAttack();
            DoPlayerAttack();
        }
    }

    private void InputPlayerAttack()
    {
        if (Input.GetMouseButtonDown(0) && _slideInDone && _monsterHealth>0)
        {
            _playerHitMonster = false;
            _playerAttacked = true;
            _monsterHealth -= 1;
        }
        else if (_monsterHealth<=0 && !_playerAttacked)
        {
            EndCombat();
        }
    }

    private void DoPlayerAttack()
    {
        if (_playerAttacked)
        {
            if (!_playerHitMonster)
            {
                _playerObject.transform.position = Vector3.MoveTowards(_playerObject.transform.position,_monsterFightPoint.position, _attackMovementSpeed*5 *Time.deltaTime);
                if (Vector3.Distance(_playerObject.transform.position, _monsterObject.transform.position) <= 0.1f)
                {
                    _monsterObject.GetComponent<Shake>().AddShake(1.5f, _monsterFightPoint.position);
                    _playerHitMonster = true;
                }
            }
            else if (_playerHitMonster)
            {
                _playerObject.transform.position = Vector3.Lerp(_playerObject.transform.position, _playerFightPoint.position, _attackMovementSpeed * Time.deltaTime);
                if (Vector3.Distance(_playerObject.transform.position, _playerFightPoint.position) <= 0.1f)
                {
                    _playerObject.transform.position = _playerFightPoint.position;
                    _playerAttacked = false;
                }
            }
        }
    }
}
