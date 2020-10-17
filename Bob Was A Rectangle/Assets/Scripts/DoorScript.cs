﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private bool isOpen = false;
    private bool isMoving = false;
    [SerializeField] float doorSpeed = 1.0f;
    KeyCode testKey = KeyCode.F1;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(testKey))
            UseDoor();
    }

    public void UseDoor()
    {
        //print("opening");
        if (!isMoving)
        {
            if (isOpen)
                StartCoroutine(CloseDoor());
            else
                StartCoroutine(OpenDoor());
        }
    }

    private IEnumerator OpenDoor()
    {
        isMoving = true;
        Vector3 newRot = transform.eulerAngles;
        while (transform.eulerAngles.y <= 90)
        {
            newRot.y += 100.0f * Time.deltaTime * doorSpeed;
            transform.eulerAngles = newRot;
            yield return null;
        }
        newRot.y = 90;
        transform.eulerAngles = newRot;
        isOpen = true;
        isMoving = false;
    }

    private IEnumerator CloseDoor()
    {
        isMoving = true;
        Vector3 newRot = transform.eulerAngles;
        while(transform.eulerAngles.y <= 90)
        {
            newRot.y -= 100.0f * Time.deltaTime * doorSpeed;
            transform.eulerAngles = newRot;
            yield return null;
        }
        newRot.y = 0;
        transform.eulerAngles = newRot;
        isOpen = false;
        isMoving = false;
    }
}
