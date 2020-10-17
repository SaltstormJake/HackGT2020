using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private bool isOpen = false;
    private bool isMoving = false;
    private bool playerInDoor = false;
    [SerializeField] float doorSpeed = 1.0f;
    KeyCode testKey = KeyCode.F1;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        isMoving = false;
        playerInDoor = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(testKey))
            UseDoor();
    }

    public void UseDoor()
    {
        if (!isMoving && !playerInDoor)
        {
            if (isOpen)
                StartCoroutine(CloseDoor());
            else
                StartCoroutine(OpenDoor());
        }
    }

    public void Open()
    {
        if (!isMoving && !playerInDoor)
            StartCoroutine(OpenDoor());
    }

    public void Close()
    {
        if (!isMoving && !playerInDoor)
            StartCoroutine(CloseDoor());
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

    public void SwitchOpening(bool value)
    {
        playerInDoor = value;
    }
}
