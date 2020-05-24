using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField] private GameObject _objectToShake = null;
    [SerializeField] private float _fadeStrength =0;
    private float _shakepower =0;
    private Vector3 _objectStartPos = Vector3.zero;
    private bool _shaking = false;

    // Update is called once per frame
    void Update()
    {
        DoShake();
    }

    public void AddShake(float shakeAmount, Vector3 startPosition)
    {
        if (!_shaking)
        {
            _objectStartPos = startPosition;
            _shaking = true;
        }
        _shakepower += shakeAmount;
    }

    void DoShake()
    {
        if (_shaking)
        {
            if (_shakepower > 0)
            {
                float xShake = Random.Range(-1, 1) * _shakepower * Time.deltaTime;
                float yShake = Random.Range(-1, 1) * _shakepower * Time.deltaTime;

                _objectToShake.transform.position = _objectStartPos + new Vector3(xShake, yShake, 0f);

                _shakepower = Mathf.MoveTowards(_shakepower, 0f, _fadeStrength * Time.deltaTime);
            }
            else if (_objectToShake.transform.position != _objectStartPos)
            {
                _objectToShake.transform.position = _objectStartPos;
            }
            if (_shakepower<=0)
            {
                _shaking = false;
            }
        }
    }
}
