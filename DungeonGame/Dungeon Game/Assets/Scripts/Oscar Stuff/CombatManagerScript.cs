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
    private TreasureObject _pickedTreasure;

    //Singleton
    public static CombatManagerScript Instance = null;

    //Combat Variables
    private bool _slideInDone = false;

    private bool _playerAttacked = false;
    private bool _playerHitMonster = false;

    private List<int> _ongoingTweens = new List<int>();


    private void Awake()
    {
        Instance = this;
        enabled = false;
    }

    private Action ActionOnSlideInDone;
    private Action ActionOnSlideOutDone;
    private Action ActionOnAttackHitDone;
    private Action ActionOnAttackRecallDone;

    public event EventHandler OnStartCombat;
    public event EventHandler OnEndCombat;

    // Start is called before the first frame update
    void Start()
    {
        ActionOnSlideInDone += SlideInDone;
        ActionOnSlideOutDone += SlideOutDone;
        ActionOnAttackHitDone += OnAttackComplete;
        ActionOnAttackRecallDone += SlideOut;
        _controlScript = ControlScript.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    public void StartCombat(MonsterObject monsterobject)
    {
        Destroy(_monsterModel);
        _monsterHealth = monsterobject.MonsterHealth;
        _monsterModel = Instantiate(monsterobject.MonsterPrefab, _monsterObject.transform.position, _monsterObject.transform.rotation, _monsterObject.transform);
        _monsterObject.transform.position = _monsterSpawnPoint.position;
        _playerObject.transform.position = _playerSpawnPoint.position;
        _combat = true;
        SlideIn();
        OnStartCombat?.Invoke(this, EventArgs.Empty);
        _pickedTreasure = pickTreasure(monsterobject);
    }

    private void SlideIn()
    {
        _ongoingTweens.Add(LeanTween.move(_monsterObject, _monsterFightPoint.transform, 1).setEaseOutQuint().id);
        _ongoingTweens.Add(LeanTween.move(_playerObject, _playerFightPoint.transform, 0.75f).setEaseOutQuint().setOnComplete(ActionOnSlideInDone).id);
    }

    private void SlideInDone()
    {
        _slideInDone = true;
    }

    private void SlideOut()
    {
        if (_monsterHealth <= 0)
        {
            _controlScript.CurrentlySelectedTile.TileSpecialSpawnScript.SpecialSpawn.GetComponentInChildren<Animator>().SetTrigger("Disappear");
            _controlScript.CurrentlySelectedTile.ContainsMonster = false;
            CancelAllTweensInList();
            _ongoingTweens.Add(LeanTween.move(_monsterObject, _monsterSpawnPoint.transform, 1).setEaseOutQuint().id);
            _ongoingTweens.Add(LeanTween.move(_playerObject, _playerSpawnPoint.transform, 0.5f).setEaseOutQuint().setOnComplete(ActionOnSlideOutDone).id);
            OnEndCombat?.Invoke(this, EventArgs.Empty);
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

    private void DoAttackMovement()
    {
        CancelAllTweensInList();
        _ongoingTweens.Add(LeanTween.move(_playerObject, _monsterFightPoint.transform, 0.1f).setOnComplete(OnAttackComplete).id);
    }

    private void OnAttackComplete()
    {
        _ongoingTweens.Add(LeanTween.move(_playerObject, _playerFightPoint.transform, 0.2f).setEaseOutQuint().setOnComplete(ActionOnAttackRecallDone).id);
        _monsterHealth -= 1;
        _monsterObject.GetComponent<Shake>().AddShake(1.5f, _monsterFightPoint.position);
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0) && _slideInDone&& _monsterHealth>0)
        {
            DoAttackMovement();
        }
    }

    private void CancelAllTweensInList()
    {
        foreach (int tween in _ongoingTweens)
        {
            LeanTween.cancel(tween);
        }
    }

    private TreasureObject pickTreasure(MonsterObject monsterObject)
    {
        return monsterObject._treasureObjects[UnityEngine.Random.Range(0, monsterObject._treasureObjects.Count)];
    }
}
