using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] float movementAmount = 1;
    [SerializeField] float jumpHeight = 0.5f;
    [SerializeField] float moveSpeed = 1;
    private bool isMoving = false;
    protected enum MoveDirection
    {
        NORTH,
        SOUTH,
        EAST,
        WEST
    }
    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected IEnumerator Move(MoveDirection direction)
    {
        if (!isMoving)
        {
            isMoving = true;
            yield return MoveVertical(true);
            yield return MoveHorizonal(direction);
            yield return MoveVertical(false);
            isMoving = false;
        }
    }

    protected IEnumerator MoveVertical(bool direction)
    {
        Vector3 position = transform.position;
        if (direction)
        {
            position.y += jumpHeight;
            while (position.y - transform.position.y > 0.01f)
            {
                transform.Translate(Vector3.up * Time.deltaTime, Space.World);
                yield return null;
            }
        }
        else
        {
            position.y -= jumpHeight;
            while(transform.position.y - position.y > 0.01f)
            {
                transform.Translate(Vector3.down * Time.deltaTime, Space.World);
                yield return null;
            }
        }
        transform.position = position;
    }

    protected IEnumerator MoveHorizonal(MoveDirection direction)
    {
        Vector3 position = transform.position;
        switch (direction)
        {
            case MoveDirection.NORTH:
                position.z += movementAmount;
                while (position.z - transform.position.z > 0.01f)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.World);
                    yield return null;
                }
                break;
            case MoveDirection.SOUTH:
                position.z -= movementAmount;
                while (transform.position.z - position.z > 0.01f)
                {
                    transform.Translate(Vector3.back * Time.deltaTime * moveSpeed, Space.World);
                    yield return null;
                }
                break;
            case MoveDirection.EAST:
                position.x += movementAmount;
                while (position.x - transform.position.x > 0.01f)
                {
                    transform.Translate(Vector3.right * Time.deltaTime * moveSpeed, Space.World);
                    yield return null;
                }
                break;
            case MoveDirection.WEST:
                position.x -= movementAmount;
                while (transform.position.x - position.x > 0.01f)
                {
                    transform.Translate(Vector3.left * Time.deltaTime * moveSpeed, Space.World);
                    yield return null;
                }
                break;
            default:
                break;
        }
            transform.position = position;
    }
}

