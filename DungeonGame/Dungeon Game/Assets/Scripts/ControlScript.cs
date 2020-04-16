using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlScript : MonoBehaviour
{
    [SerializeField] private GameObject _character = null;
    public TileScript CurrentlySelectedTile = null;
    [SerializeField] private GameObject _camera = null;
    [SerializeField] private float _movementSpeed = 0;
    [SerializeField] private float _cameraSpeed = 0;
    [SerializeField] private Vector3 _camOffset = new Vector3(0,0,0);
    [SerializeField] private Transform _pointToLookAt = null;
    [Tooltip("Multiplier when there's multiple tiles in the Queue")]
    [SerializeField] private float _movementSpeedMultiplier;
    private Vector3 _desiredPos = new Vector3(0,0,0);
    private List<Vector3> _desiredPositions= new List<Vector3>();
    private Animator _anim;

#if UNITY_IOS
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
#endif

    // Start is called before the first frame update
    void Start()
    {
        _anim = _character.GetComponentInChildren<Animator>();
        _desiredPos = _character.transform.position;
        CurrentlySelectedTile = GameObject.FindGameObjectWithTag("Spawnpoint").GetComponentInChildren<TileScript>();
        _desiredPositions.Add(CurrentlySelectedTile.transform.Find("MovePoint").position);
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTile();
        MoveCamera();
        CameraLookAtPoint(_pointToLookAt);
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
        if ((Input.GetMouseButtonDown(0)))
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
                    }
                    _desiredPositions.Add(CurrentlySelectedTile.transform.Find("MovePoint").position);
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
                    }
                    _desiredPositions.Add(CurrentlySelectedTile.transform.Find("MovePoint").position);
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
                    }
                    _desiredPositions.Add(CurrentlySelectedTile.transform.Find("MovePoint").position);
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
                    }
                    _desiredPositions.Add(CurrentlySelectedTile.transform.Find("MovePoint").position);
                }
            }
        }
    }



    void MoveToTile()
    {
        if (_desiredPositions.Count>=2)
        {
            if (Vector3.Distance(_character.transform.position, _desiredPositions[0]) >= 0.1f)
            {
                _character.transform.position = Vector3.MoveTowards(_character.transform.position, _desiredPositions[0], _movementSpeed*_movementSpeedMultiplier * Time.deltaTime);
            }
            else
            {
                _desiredPositions.RemoveAt(0);
            }
        }
        else if (_desiredPositions.Count == 1)
        {
            if (Vector3.Distance(_character.transform.position, _desiredPositions[0]) >= 0.1f)
            {
                _character.transform.position = Vector3.Lerp(_character.transform.position, _desiredPositions[0], _movementSpeed * Time.deltaTime);
            }
            else
            {
                _desiredPositions.RemoveAt(0);
            }
        }
    }

    void MoveCamera()
    {
        if (Vector3.Distance(_camera.transform.position, _character.transform.position+_camOffset) >=0.05f)
        {
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _character.transform.position + _camOffset, _cameraSpeed * Time.deltaTime);
        }
    }

    private void CameraLookAtPoint(Transform pointToLookAt)
    {
        _camera.transform.rotation = Quaternion.Slerp(_camera.transform.rotation, Quaternion.LookRotation(pointToLookAt.transform.position - _camera.transform.position), Time.deltaTime * _cameraSpeed);
    }

    private void PlayJump()
    {
        _anim.SetTrigger("Jump");
    }
}
