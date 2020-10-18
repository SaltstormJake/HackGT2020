using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlledGoalScript : MovementScript
{
    public void ProcessVoiceCommand(string command) {
        if (command == "LEFT")
            StartCoroutine(Move(MoveDirection.WEST));
        if (command == "RIGHT")
            StartCoroutine(Move(MoveDirection.EAST));
        if (command == "UP")
            StartCoroutine(Move(MoveDirection.NORTH));
        if (command == "DOWN")
            StartCoroutine(Move(MoveDirection.SOUTH));
    }
}
