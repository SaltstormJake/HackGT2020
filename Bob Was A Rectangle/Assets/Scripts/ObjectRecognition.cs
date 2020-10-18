using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRecognition : MonoBehaviour
{
    string command;

    void OnTriggerEnter2D(Collider2D other)
    {
        command = "";
    }

    void OnTriggerExit2D(Collider2D other)
    {
        command = "";
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.tag == "Door" && command == "OPEN")
        {
            other.gameObject.GetComponent<DoorScript>().UseDoor();
        }

        if (other.gameObject.tag == "Lever" && command == "PULL")
        {
            other.gameObject.GetComponent<LeverScript>().PullLever();
        }
    }

    public void SetCommand(string com)
    {
        command = com;
    }
}

