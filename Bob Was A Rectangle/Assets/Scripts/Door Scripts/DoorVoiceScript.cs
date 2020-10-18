﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorVoiceScript : MonoBehaviour
{
    string command;
    DoorScript door;

    private void Awake()
    {
        door = gameObject.GetComponent<DoorScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        command = "";
    }

    private void OnTriggerExit(Collider other)
    {
        command = "";
    }


}
