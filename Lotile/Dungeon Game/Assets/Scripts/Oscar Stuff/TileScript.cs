﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public GameObject TopPoint;
    public GameObject RightPoint;
    public GameObject BotPoint;
    public GameObject LeftPoint;

    [HideInInspector] public bool ContainsMonster =false;
    [HideInInspector] public bool ContainsTreasure= false;
    [SerializeField] private float _TileMoveUpSpeed;

    [SerializeField] private bool _startTile = false;
    public bool StartTile { get { return _startTile; } }
    public bool _spawned= false;
    public Sides RequiredOpenSides;
    public Sides RequiredClosedSides;
    [Tooltip("Larger number means smaller chance")]
    public int ChanceAtExtraSides;

    [HideInInspector] public TileContainedObjectScript TileSpecialSpawnScript;
    [SerializeField] private GameObject _tileObject= null;
    public GameObject TileObject { get { return _tileObject; } }

    // Start is called before the first frame update
    void Start()
    {
        if (_startTile)
        {
            TileSpecialSpawnScript = GetComponentInChildren<TileContainedObjectScript>();
        }
        else
        {
            Invoke("SpawnTile", 0.05f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_spawned == false)
        {
            if (other.CompareTag("CenterPoint"))
            {
                Destroy(this.gameObject);
            }
            else if (other.CompareTag("DetectionPoint"))
            {
                RequiredOpenSides |= other.GetComponent<DetectionScript>().RequiredOpenSide;
                other.GetComponent<DetectionScript>().ConnectedTile = this.gameObject;
            }
        }

    }

    private void SpawnTile()
    {
        AddRandomOpenSides();
        RoomSpawnManager.Instance.SpawnTile(this, RequiredOpenSides);
        _spawned = true;
        RemoveExcessDetectionPoints();
        TileSpecialSpawnScript = GetComponentInChildren<TileContainedObjectScript>();
    }

    private void RemoveExcessDetectionPoints()
    {
        if (!RequiredOpenSides.HasFlag(Sides.Top))
        {
            Destroy(TopPoint);
            TopPoint = null;
        }
        if (!RequiredOpenSides.HasFlag(Sides.Bot))
        {
            Destroy(BotPoint);
            BotPoint = null;
        }
        if (!RequiredOpenSides.HasFlag(Sides.Right))
        {
            Destroy(RightPoint);
            RightPoint = null;
        }
        if (!RequiredOpenSides.HasFlag(Sides.Left))
        {
            Destroy(LeftPoint);
            LeftPoint = null;
        }
    }

    private void AddRandomOpenSides()
    {
        if (!RequiredOpenSides.HasFlag(Sides.Top) && UnityEngine.Random.Range(0,100)<ChanceAtExtraSides)
        {
            RequiredOpenSides |= Sides.Top;
        }
        if (!RequiredOpenSides.HasFlag(Sides.Bot)&& UnityEngine.Random.Range(0,100) < ChanceAtExtraSides)
        {
            RequiredOpenSides |= Sides.Bot;
        }
        if (!RequiredOpenSides.HasFlag(Sides.Right) && UnityEngine.Random.Range(0,100) < ChanceAtExtraSides)
        {
            RequiredOpenSides |= Sides.Right;
        }
        if (!RequiredOpenSides.HasFlag(Sides.Left) && UnityEngine.Random.Range(0,100) < ChanceAtExtraSides)
        {
            RequiredOpenSides |= Sides.Left;
        }

        if (LeftPoint.GetComponent<DetectionScript>().ConnectedTile)
        {
            if (!LeftPoint.GetComponent<DetectionScript>().ConnectedTile.GetComponent<TileScript>().RequiredOpenSides.HasFlag(Sides.Right))
            {
                RequiredOpenSides &= ~Sides.Left;
            }
        }
        if (RightPoint.GetComponent<DetectionScript>().ConnectedTile)
        {
            if (!RightPoint.GetComponent<DetectionScript>().ConnectedTile.GetComponent<TileScript>().RequiredOpenSides.HasFlag(Sides.Left))
            {
                RequiredOpenSides &= ~Sides.Right;
            }
        }
        if (TopPoint.GetComponent<DetectionScript>().ConnectedTile)
        {
            if (!TopPoint.GetComponent<DetectionScript>().ConnectedTile.GetComponent<TileScript>().RequiredOpenSides.HasFlag(Sides.Bot))
            {
                RequiredOpenSides &= ~Sides.Top;
            }
        }
        if (BotPoint.GetComponent<DetectionScript>().ConnectedTile)
        {
            if (!BotPoint.GetComponent<DetectionScript>().ConnectedTile.GetComponent<TileScript>().RequiredOpenSides.HasFlag(Sides.Top))
            {
                RequiredOpenSides &= ~Sides.Bot;
            }
        }
    }
}
