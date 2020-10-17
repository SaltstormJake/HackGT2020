using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] float movementAmount = 0.25f;
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
            //yield return MoveVertical(true);
            yield return MoveHorizonal(direction);
            //yield return MoveVertical(false);
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
        Vector3 dir;
        RaycastHit rayInfo;
        bool inTheWay;
        switch (direction)
        {
            case MoveDirection.NORTH:
                dir = Vector3.up;
                inTheWay = Physics.Linecast(transform.position, transform.position + dir, out rayInfo);
                if (inTheWay)
                    break;
                position.y += movementAmount;
                while (position.y - transform.position.y > 0.01f)
                {
                    transform.Translate(dir * Time.deltaTime * moveSpeed, Space.World);
                    yield return null;
                }
                break;
            case MoveDirection.SOUTH:
                dir = Vector3.down;
                inTheWay = Physics.Linecast(transform.position, transform.position + dir, out rayInfo);
                if (inTheWay)
                    break;
                position.y -= movementAmount;
                while (transform.position.y - position.y > 0.01f)
                {
                    transform.Translate(dir * Time.deltaTime * moveSpeed, Space.World);
                    yield return null;
                }
                break;
            case MoveDirection.EAST:
                dir = Vector3.right;
                inTheWay = Physics.Linecast(transform.position, transform.position + dir, out rayInfo);
                if (inTheWay)
                    break;
                position.x += movementAmount;
                while (position.x - transform.position.x > 0.01f)
                {
                    transform.Translate(dir * Time.deltaTime * moveSpeed, Space.World);
                    yield return null;
                }
                break;
            case MoveDirection.WEST:
                dir = Vector3.left;
                inTheWay = Physics.Linecast(transform.position, transform.position + dir, out rayInfo);
                if (inTheWay)
                    break;
                position.x -= movementAmount;
                while (transform.position.x - position.x > 0.01f)
                {
                    transform.Translate(dir * Time.deltaTime * moveSpeed, Space.World);
                    yield return null;
                }
                break;
            default:
                break;
        }
            transform.position = position;
    }
}

