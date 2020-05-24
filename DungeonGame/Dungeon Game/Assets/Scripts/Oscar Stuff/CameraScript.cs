using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Camera _camera = null;
    [SerializeField] private GameObject _character = null;
    [SerializeField] private Vector3 _camOffset = Vector3.zero;
    [SerializeField] private float _cameraSpeed = 0;
    [SerializeField] private Transform _pointToLookAt = null;
    // Start is called before the first frame update
    void Start()
    {
        _camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        CameraLookAtPoint(_pointToLookAt);
    }
    void MoveCamera()
    {
        if (Vector3.Distance(_camera.transform.position, _character.transform.position + _camOffset) >= 0.05f)
        {
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _character.transform.position + _camOffset, _cameraSpeed * Time.deltaTime);
        }
    }

    private void CameraLookAtPoint(Transform pointToLookAt)
    {
        _camera.transform.rotation = Quaternion.Slerp(_camera.transform.rotation, Quaternion.LookRotation(pointToLookAt.transform.position - _camera.transform.position), Time.deltaTime * _cameraSpeed);
    }
}
