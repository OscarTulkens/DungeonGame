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
    private CombatStatsManagerScript _combatStatsManagerScript;

    //Player variables
    [Header("PLAYER")]
    [SerializeField] private Transform _playerSpawnPoint = null;
    [SerializeField] private Transform _playerFightPoint = null;
    [SerializeField] private GameObject _playerObjectTransform = null;
    [SerializeField] private GameObject _playerModel = null;
    private int _playerDamage = 0;
    private int _playerHealth = 0;
    private int _playerCurrentSpecialPower = 0;
    private int _playerMaxSpecialPower = 0;
    private int _playerSpecialPowerStat = 0;


    //Monster variables
    [Header("MONSTER")]
    [SerializeField] private Transform _monsterSpawnPoint = null;
    [SerializeField] private Transform _monsterFightPoint = null;
    [SerializeField] private GameObject _monsterObjectTransform = null;
    private MonsterObject _monsterObject = null;
    private int _monsterHealth = 0;
    private GameObject _monsterModel = null;
    private TreasureObject _pickedTreasure;

    //Singleton
    public static CombatManagerScript Instance = null;

    //Combat Variables
    private bool _slideInDone = false;

    private bool _playerAttacked = false;
    private bool _playerHitMonster = false;

    private List<int> _activePlayerTweens = new List<int>();
    private List<int> _activeMonsterTweens = new List<int>();

    private bool _isAbleToAttack = true;

    private void Awake()
    {
        Instance = this;
        enabled = false;
    }

    private Action ActionOnSlideInDone;
    private Action ActionOnSlideOutDone;
    private Action ActionOnPlayerAttackHitDone;
    private Action ActionOnPlayerAttackRecallDone;

    //Monster Combat Variables
    private float _timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        _combatStatsManagerScript = GetComponent<CombatStatsManagerScript>();
        ActionOnSlideInDone += SlideInDone;
        ActionOnSlideOutDone += SlideOutDone;
        ActionOnPlayerAttackHitDone += OnPlayerAttackComplete;
        ActionOnPlayerAttackRecallDone += SlideOut;
        _controlScript = ControlScript.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput();
        CheckMonsterAttackTimer();
    }

    public void StartCombat(MonsterObject monsterobject)
    {
        ResetMonsterTimer();
        ResetMonsterObject();
        _playerHealth = (int)CharacterStatsManager.TotalHealth;
        _monsterObject = monsterobject;
        _monsterHealth = _monsterObject.MonsterHealth;
        _monsterModel = Instantiate(monsterobject.MonsterPrefab, _monsterObjectTransform.transform.position, _monsterObjectTransform.transform.rotation, _monsterObjectTransform.transform);
        _playerObjectTransform.transform.position = _playerSpawnPoint.position;
        _playerDamage = (int)CharacterStatsManager.TotalDamage;
        _playerMaxSpecialPower = (int)CharacterStatsManager.TotalSpecialPowerMax;
        _playerCurrentSpecialPower = 0;
        _playerSpecialPowerStat = (int)CharacterStatsManager.TotalSpecialPower;
        _combat = true;
        SlideIn();
        EventManager.Instance.StartCombat();
        _pickedTreasure = pickTreasure(monsterobject);
        _combatStatsManagerScript.SetStartStats(monsterobject.MonsterName, _monsterHealth, monsterobject.MonsterTimeBetweenAttacks);
    }

    private void SlideIn()
    {
        _activePlayerTweens.Add(LeanTween.move(_monsterObjectTransform, _monsterFightPoint.transform, 1).setEaseOutQuint().id);
        _activePlayerTweens.Add(LeanTween.move(_playerObjectTransform, _playerFightPoint.transform, 0.75f).setEaseOutQuint().setOnComplete(ActionOnSlideInDone).id);
    }

    private void SlideInDone()
    {
        _isAbleToAttack = true;
        _slideInDone = true;
    }

    private void SlideOut()
    {
        if (_monsterHealth <= 0)
        {
            _controlScript.CurrentlySelectedTile.ContainsMonster = false;
            CancelAllTweensInList(_activePlayerTweens);
            CancelAllTweensInList(_activeMonsterTweens);
            _activePlayerTweens.Add(LeanTween.scale(_monsterObjectTransform, Vector3.zero, 0.25f).setEaseInBack().id);
            _activePlayerTweens.Add(LeanTween.move(_playerObjectTransform, _playerSpawnPoint.transform, 0.5f).setEaseOutQuint().setDelay(0.4f).setOnComplete(ActionOnSlideOutDone).id);
            EventManager.Instance.EndCombat();
            TreasureManager.Instance.enabled = true;
            TreasureManager.Instance.StartTreasure(_pickedTreasure);
        }
    }

    private void SlideOutDone()
    {
        _slideInDone = false;
        EndCombat();
    }

    private void EndCombat()
    {
        Destroy(_monsterModel);
        _combat = false;
    }

    private void DoPlayerAttackMovement()
    {
        CancelAllTweensInList(_activePlayerTweens);
        _activePlayerTweens.Add(LeanTween.move(_playerObjectTransform, _monsterObjectTransform.transform, 0.1f).setOnComplete(OnPlayerAttackComplete).id);
    }

    private void OnPlayerAttackComplete()
    {
        if (_playerCurrentSpecialPower <_playerMaxSpecialPower)
        {
            _monsterHealth -= _playerDamage;
            _playerCurrentSpecialPower += _playerSpecialPowerStat;
            _combatStatsManagerScript.UpdatePlayerSpecial(_playerCurrentSpecialPower);
        }

        else if (_playerCurrentSpecialPower >= _playerMaxSpecialPower)
        {
            _monsterHealth -= (_playerDamage + 5);
            _playerCurrentSpecialPower = 0;
            _combatStatsManagerScript.UpdatePlayerSpecial(_playerCurrentSpecialPower);
        }

        _combatStatsManagerScript.UpdateHealthStats(_monsterHealth, _playerHealth);
        _activePlayerTweens.Add(LeanTween.move(_playerObjectTransform, _playerFightPoint.transform, 0.2f).setEaseOutQuint().setOnComplete(ActionOnPlayerAttackRecallDone).id);
        LeanTween.scale(_monsterModel, _monsterModel.transform.localScale * 1.3f, 0.5f).setEasePunch();

    }

    private void CheckPlayerInput()
    {
        if (Input.GetMouseButtonDown(0) && _slideInDone && _monsterHealth>0 && _isAbleToAttack)
        {
            DoPlayerAttackMovement();
        }
    }

    private void CheckMonsterAttackTimer()
    {
        _combatStatsManagerScript.UpdateMonsterSpecial(_timer);
        if (_monsterObject!=null && _monsterObject.MonsterTimeBetweenAttacks!=0 && _slideInDone && _isAbleToAttack)
        {
            _timer += Time.deltaTime;
            if (_timer >= _monsterObject.MonsterTimeBetweenAttacks && _monsterHealth > 0)
            {
                DoMonsterAttackMovement();
                ResetMonsterTimer();
            }
        }
    }

    private void DoMonsterAttackMovement()
    {
        _isAbleToAttack = false;
        CancelAllTweensInList(_activeMonsterTweens);
        CancelAllTweensInList(_activePlayerTweens);
        _activePlayerTweens.Add(LeanTween.move(_playerObjectTransform, _playerFightPoint, 0.1f).id);
        _activeMonsterTweens.Add(LeanTween.move(_monsterObjectTransform, _playerObjectTransform.transform, 0.3f).setEaseInQuint().setOnComplete(OnMonsterAttackComplete).id);
    }

    private void OnMonsterAttackComplete()
    {
        _playerHealth -= _monsterObject.MonsterAttack;
        _combatStatsManagerScript.UpdateHealthStats(_monsterHealth, _playerHealth);
        EnableAttacking();
        _activeMonsterTweens.Add(LeanTween.move(_monsterObjectTransform, _monsterFightPoint.transform, 0.3f).setEaseOutQuint().setOnComplete(EnableAttacking).id);
        LeanTween.scale(_playerModel, _playerModel.transform.localScale * 1.5f, 0.5f).setEasePunch();
    }

    private void EnableAttacking()
    {
        _isAbleToAttack = true;
    }

    private void ResetMonsterTimer()
    {
        _timer = 0;
    }

    private void CancelAllTweensInList(List<int> tweenlist)
    {
        foreach (int tween in tweenlist)
        {
            LeanTween.cancel(tween);
        }
    }

    private TreasureObject pickTreasure(MonsterObject monsterObject)
    {
        return monsterObject._treasureObjects[UnityEngine.Random.Range(0, monsterObject._treasureObjects.Count)];
    }

    private void ResetMonsterObject()
    {
        Destroy(_monsterModel);
        _monsterObjectTransform.transform.position = _monsterSpawnPoint.position;
        _monsterObjectTransform.transform.localScale = Vector3.one;
    }
}
