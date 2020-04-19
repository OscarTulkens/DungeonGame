using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Camera _camera = null;
    [SerializeField] private float _fadeStrength =0;
    private float _shakepower =0;
    private Vector3 _cameraStartPos = Vector3.zero;

    public static CameraShake Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _cameraStartPos = _camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Shake();
    }

    public void AddShake(float shakeAmount)
    {
        _shakepower += shakeAmount;
    }

    void Shake()
    {
        if (_shakepower>0)
        {
            float xShake = Random.Range(-1, 1) *_shakepower*Time.deltaTime;
            float yShake = Random.Range(-1, 1) * _shakepower*Time.deltaTime;

            _camera.transform.position = _cameraStartPos + new Vector3(xShake, yShake, 0f);

            _shakepower = Mathf.MoveTowards(_shakepower, 0f, _fadeStrength * Time.deltaTime);
        }
        else if(_camera.transform.position != _cameraStartPos)
        {
            _camera.transform.position = _cameraStartPos;
        }
    }
}
