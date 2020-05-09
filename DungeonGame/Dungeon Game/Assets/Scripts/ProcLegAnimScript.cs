using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcLegAnimScript : MonoBehaviour
{
    [SerializeField] private GameObject _rFoot = null;
    [SerializeField] private GameObject _lFoot = null;
    [SerializeField] private GameObject _rPoint = null;
    [SerializeField] private GameObject _lPoint = null;
    [SerializeField] private float _Speed = 0;
    [SerializeField] private float _maxDistance = 0;
    [SerializeField] private float _minDistance = 0;

    [SerializeField] private Transform _rLegPoint;
    [SerializeField] private Transform _lLegPoint;

    private LineRenderer _rLeg;
    private LineRenderer _lLeg;

    private bool _rBool = false;
    private bool _lBool = false;

    private void Start()
    {
        if (_rLegPoint && _lLegPoint)
        {
            _rLeg = _rLegPoint.GetComponent<LineRenderer>();
            _lLeg = _lLegPoint.GetComponent<LineRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
        MoveToPoint();
        DrawLines();
    }

    private void CheckDistance()
    {
        if (Vector3.Distance(_rFoot.transform.position, _rPoint.transform.position) >= _maxDistance&&!_lBool)
        {
            _rBool = true;
        }
        if (Vector3.Distance(_lFoot.transform.position, _lPoint.transform.position)>=_maxDistance &&!_rBool)
        {
            _lBool = true;
        }
    }

    private void MoveToPoint()
    {
        if (_rBool &&!_lBool)
        {
            _rFoot.transform.position = Vector3.MoveTowards(_rFoot.transform.position, _rPoint.transform.position, _Speed * Time.deltaTime);
            if (Vector3.Distance(_rFoot.transform.position, _rPoint.transform.position) <= _minDistance)
            {
                _rBool = false;
            }
        }
        if (_lBool&&!_rBool)
        {
            _lFoot.transform.position = Vector3.MoveTowards(_lFoot.transform.position, _lPoint.transform.position, _Speed * Time.deltaTime);
            if (Vector3.Distance(_lFoot.transform.position, _lPoint.transform.position) <= _minDistance)
            {
                _lBool = false;
            }
        }
    }

    private void DrawLines()
    {
        _rLeg.SetPosition(0, _rFoot.transform.position);
        _rLeg.SetPosition(1, _rLegPoint.position);
        _lLeg.SetPosition(0, _lFoot.transform.position);
        _lLeg.SetPosition(1, _lLegPoint.position);
    }
}
