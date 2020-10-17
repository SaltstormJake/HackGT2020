using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledPlayerScript : MovementScript
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            StartCoroutine(Move(MoveDirection.WEST));
        if (Input.GetKey(KeyCode.D))
            StartCoroutine(Move(MoveDirection.EAST));
        if (Input.GetKey(KeyCode.W))
            StartCoroutine(Move(MoveDirection.NORTH));
        if (Input.GetKey(KeyCode.S))
            StartCoroutine(Move(MoveDirection.SOUTH));
    }
}
