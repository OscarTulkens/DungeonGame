using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public event EventHandler<OnChangeEquipmentArgs> OnChangeEquipment;
    public class OnChangeEquipmentArgs: EventArgs
    {
        public ItemType itemtype;
        public GameObject itemModel;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
