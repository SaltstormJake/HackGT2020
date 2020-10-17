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
        if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(Move(MoveDirection.WEST));
        if (Input.GetKeyDown(KeyCode.D))
            StartCoroutine(Move(MoveDirection.EAST));
        if (Input.GetKeyDown(KeyCode.W))
            StartCoroutine(Move(MoveDirection.NORTH));
        if (Input.GetKeyDown(KeyCode.S))
            StartCoroutine(Move(MoveDirection.SOUTH));
    }
}
