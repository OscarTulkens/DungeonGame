using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionScript : MonoBehaviour
{
    public Sides RequiredOpenSide;
    public bool FreeSpace = true;
    public GameObject ConnectedTile;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CenterPoint"))
        {
            if (FreeSpace == true)
            {
                FreeSpace = false;

            }
            ConnectedTile = other.gameObject;





        }
    }

}



