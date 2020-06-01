using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ControlScript : MonoBehaviour
{
    [SerializeField] private GameObject _character = null;
    public TileScript CurrentlySelectedTile = null;
    [SerializeField] private GameObject _camera = null;
    [HideInInspector] public List<Vector3> DesiredPositions= new List<Vector3>();
    //private Animator _anim;
    private Vector2 _touchStart = Vector2.zero;
    private Vector2 _touchEnd = Vector2.zero;
    [SerializeField] private float _minSwipeDistance = 0;

    public static ControlScript Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

#if UNITY_IOS
        Application.targetFrameRate = 60;
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        //_anim = _character.GetComponentInChildren<Animator>();
        CurrentlySelectedTile = GameObject.FindGameObjectWithTag("Spawnpoint").GetComponentInChildren<TileScript>();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnAndSelectTile();
        ResetLevel();
    }

    private void ResetLevel()
    {
        if (Input.touchCount>=2)
        {
            SceneManager.LoadScene(0);
        }
    }

    void SpawnAndSelectTile()
    {
        GameObject _newTile;
        Vector3 islandPos = _camera.GetComponent<Camera>().WorldToScreenPoint(CurrentlySelectedTile.transform.position);
        Vector3 clickPos = Input.mousePosition;

        #region EDITOR
#if UNITY_EDITOR
        if ((Input.GetMouseButtonDown(0))& !EventSystem.current.IsPointerOverGameObject())
        {

            if (CurrentlySelectedTile.LeftPoint)
            {
                if (clickPos.x < islandPos.x && clickPos.y < islandPos.y)
                {
                    if (CurrentlySelectedTile.LeftPoint.GetComponent<DetectionScript>().FreeSpace)
                    {
                        PlayJump();
                        _newTile = Instantiate(RoomSpawnManager.Instance.SpawnpointsPrefab, CurrentlySelectedTile.LeftPoint.transform.position, Quaternion.identity);
                        CurrentlySelectedTile = _newTile.GetComponentInChildren<TileScript>();
                    }
                    else
                    {
                        CurrentlySelectedTile = CurrentlySelectedTile.LeftPoint.GetComponent<DetectionScript>().ConnectedTile.GetComponentInChildren<TileScript>();
                        AddDesiredPosition(CurrentlySelectedTile.TileSpecialSpawnScript.MovementPoint.position);
                    }
                }
            }

            if (CurrentlySelectedTile.TopPoint)
            {
                if (clickPos.x < islandPos.x && clickPos.y > islandPos.y)
                {
                    if (CurrentlySelectedTile.TopPoint.GetComponent<DetectionScript>().FreeSpace)
                    {
                        PlayJump();
                        _newTile = Instantiate(RoomSpawnManager.Instance.SpawnpointsPrefab, CurrentlySelectedTile.TopPoint.transform.position, Quaternion.identity);
                        CurrentlySelectedTile = _newTile.GetComponentInChildren<TileScript>();
                    }
                    else
                    {
                        CurrentlySelectedTile = CurrentlySelectedTile.TopPoint.GetComponent<DetectionScript>().ConnectedTile.GetComponentInChildren<TileScript>();
                        AddDesiredPosition(CurrentlySelectedTile.TileSpecialSpawnScript.MovementPoint.position);
                    }
                }
            }

            if (CurrentlySelectedTile.RightPoint)
            {
                if (clickPos.x > islandPos.x && clickPos.y > islandPos.y)
                {
                    if (CurrentlySelectedTile.RightPoint.GetComponent<DetectionScript>().FreeSpace)
                    {
                        PlayJump();
                        _newTile = Instantiate(RoomSpawnManager.Instance.SpawnpointsPrefab, CurrentlySelectedTile.RightPoint.transform.position, Quaternion.identity);
                        CurrentlySelectedTile = _newTile.GetComponentInChildren<TileScript>();
                    }
                    else
                    {
                        CurrentlySelectedTile = CurrentlySelectedTile.RightPoint.GetComponent<DetectionScript>().ConnectedTile.GetComponentInChildren<TileScript>();
                        AddDesiredPosition(CurrentlySelectedTile.TileSpecialSpawnScript.MovementPoint.position);
                    }
                }
            }

            if (CurrentlySelectedTile.BotPoint)
            {
                if (clickPos.x > islandPos.x && clickPos.y < islandPos.y)
                {
                    if (CurrentlySelectedTile.BotPoint.GetComponent<DetectionScript>().FreeSpace)
                    {
                        PlayJump();
                        _newTile = Instantiate(RoomSpawnManager.Instance.SpawnpointsPrefab, CurrentlySelectedTile.BotPoint.transform.position, Quaternion.identity);
                        CurrentlySelectedTile = _newTile.GetComponentInChildren<TileScript>();
                    }
                    else
                    {
                        CurrentlySelectedTile = CurrentlySelectedTile.BotPoint.GetComponent<DetectionScript>().ConnectedTile.GetComponentInChildren<TileScript>();
                        AddDesiredPosition(CurrentlySelectedTile.TileSpecialSpawnScript.MovementPoint.position);
                    }
                }
            }
        }
#endif
        #endregion

        #region IOS
#if UNITY_IOS
        bool _swiped = false;
        if (Input.touchCount>=1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _touchStart = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                _touchEnd = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended && Vector2.Distance(_touchStart, _touchEnd) >= _minSwipeDistance)
            {
                _swiped = true;
            }
        }

        if (_swiped == true)
        {
            if (CurrentlySelectedTile.LeftPoint)
            {
                if (_touchEnd.x < _touchStart.x && _touchEnd.y < _touchStart.y)
                {
                    if (CurrentlySelectedTile.LeftPoint.GetComponent<DetectionScript>().FreeSpace)
                    {
                        PlayJump();
                        _newTile = Instantiate(RoomSpawnManager.Instance.SpawnpointsPrefab, CurrentlySelectedTile.LeftPoint.transform.position, Quaternion.identity);
                        CurrentlySelectedTile = _newTile.GetComponentInChildren<TileScript>();
                    }
                    else
                    {
                        CurrentlySelectedTile = CurrentlySelectedTile.LeftPoint.GetComponent<DetectionScript>().ConnectedTile.GetComponentInChildren<TileScript>();
                        AddDesiredPosition(CurrentlySelectedTile.TileSpecialSpawnScript.MovementPoint.position);
                    }
                }
            }

            if (CurrentlySelectedTile.TopPoint)
            {
                if (_touchEnd.x < _touchStart.x && _touchEnd.y > _touchStart.y)
                {
                    if (CurrentlySelectedTile.TopPoint.GetComponent<DetectionScript>().FreeSpace)
                    {
                        PlayJump();
                        _newTile = Instantiate(RoomSpawnManager.Instance.SpawnpointsPrefab, CurrentlySelectedTile.TopPoint.transform.position, Quaternion.identity);
                        CurrentlySelectedTile = _newTile.GetComponentInChildren<TileScript>();
                    }
                    else
                    {
                        CurrentlySelectedTile = CurrentlySelectedTile.TopPoint.GetComponent<DetectionScript>().ConnectedTile.GetComponentInChildren<TileScript>();
                        AddDesiredPosition(CurrentlySelectedTile.TileSpecialSpawnScript.MovementPoint.position);
                    }
                }
            }

            if (CurrentlySelectedTile.RightPoint)
            {
                if (_touchEnd.x > _touchStart.x && _touchEnd.y > _touchStart.y)
                {
                    if (CurrentlySelectedTile.RightPoint.GetComponent<DetectionScript>().FreeSpace)
                    {
                        PlayJump();
                        _newTile = Instantiate(RoomSpawnManager.Instance.SpawnpointsPrefab, CurrentlySelectedTile.RightPoint.transform.position, Quaternion.identity);
                        CurrentlySelectedTile = _newTile.GetComponentInChildren<TileScript>();
                    }
                    else
                    {
                        CurrentlySelectedTile = CurrentlySelectedTile.RightPoint.GetComponent<DetectionScript>().ConnectedTile.GetComponentInChildren<TileScript>();
                        AddDesiredPosition(CurrentlySelectedTile.TileSpecialSpawnScript.MovementPoint.position);
                    }
                }
            }

            if (CurrentlySelectedTile.BotPoint)
            {
                if (_touchEnd.x > _touchStart.x && _touchEnd.y < _touchStart.y)
                {
                    if (CurrentlySelectedTile.BotPoint.GetComponent<DetectionScript>().FreeSpace)
                    {
                        PlayJump();
                        _newTile = Instantiate(RoomSpawnManager.Instance.SpawnpointsPrefab, CurrentlySelectedTile.BotPoint.transform.position, Quaternion.identity);
                        CurrentlySelectedTile = _newTile.GetComponentInChildren<TileScript>();
                    }
                    else
                    {
                        CurrentlySelectedTile = CurrentlySelectedTile.BotPoint.GetComponent<DetectionScript>().ConnectedTile.GetComponentInChildren<TileScript>();
                        AddDesiredPosition(CurrentlySelectedTile.TileSpecialSpawnScript.MovementPoint.position);
                    }
                }
            }

            _swiped = false;
        }
#endif
        #endregion
    }


    public void AddDesiredPosition(Vector3 positionToAdd)
    { 
        DesiredPositions.Add(positionToAdd);
    }

    private void PlayJump()
    {
        //_anim.SetTrigger("Jump");
    }
}
