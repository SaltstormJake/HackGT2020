using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    [SerializeField] GameObject target = null;
    [SerializeField] Vector2 objectDestination;
    [SerializeField] float moveObjectSpeed;
    [SerializeField] bool yFirst = false;
    private bool pulled = false;
    private float leverSpeed;
    private bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        pulled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PullLever()
    {
        if (!pulled)
        {
            StartCoroutine(RotateLever());
            StartCoroutine(MoveObject(objectDestination, yFirst));
        }
    }

    private IEnumerator RotateLever()
    {
        if (!isMoving)
        {
            isMoving = true;
            Vector3 finalRot = gameObject.transform.eulerAngles;
            if (!pulled)
                finalRot.z = -90.0f;
            else
                finalRot.z = 0.0f;
            while (Mathf.Abs(finalRot.z - gameObject.transform.eulerAngles.z) > 0.1f)
            {
                finalRot.z += leverSpeed * Time.deltaTime;
                gameObject.transform.eulerAngles = finalRot;
                yield return null;
            }
            pulled = true;
            isMoving = false;
        }
    }

    private IEnumerator MoveObject(Vector2 destination, bool yFirst)
    {
        Vector3 position = target.gameObject.transform.position;
        if (!yFirst)
        {
            Vector3 dir = new Vector3(destination.x - target.gameObject.transform.position.x, 0, 0);
            while(Mathf.Abs(destination.x - target.gameObject.transform.position.x) > 0.1f)
            {
                target.gameObject.transform.Translate(dir * Time.deltaTime * moveObjectSpeed, Space.World);
                yield return null;
            }
            dir = new Vector3(0, destination.y - target.gameObject.transform.position.y, 0);
            while (Mathf.Abs(destination.y - target.gameObject.transform.position.y) > 0.1f)
            {
                target.gameObject.transform.Translate(dir * Time.deltaTime * moveObjectSpeed, Space.World);
                yield return null;
            }
        }
        else
        {
            Vector3 dir = new Vector3(0, destination.y - target.gameObject.transform.position.y, 0);
            while(Mathf.Abs(destination.y - target.gameObject.transform.position.y) > 0.1f)
            {
                target.gameObject.transform.Translate(dir * Time.deltaTime * moveObjectSpeed, Space.World);
                yield return null;
            }
            dir = new Vector3(destination.x - target.gameObject.transform.position.x, 0, 0);
            while(Mathf.Abs(destination.x - target.gameObject.transform.position.x) > 0.1f)
            {
                target.gameObject.transform.Translate(dir * Time.deltaTime * moveObjectSpeed, Space.World);
                yield return null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            PullLever();
    }
}
