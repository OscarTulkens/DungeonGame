using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFloorTile : MonoBehaviour
{
    [SerializeField] private List<GameObject> _floorTiles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Instantiate<GameObject>(_floorTiles[Random.Range(0, _floorTiles.Count)], this.transform.position, Quaternion.Euler(-90, Random.Range(1,5)*90, 0), this.transform);
    }
}
