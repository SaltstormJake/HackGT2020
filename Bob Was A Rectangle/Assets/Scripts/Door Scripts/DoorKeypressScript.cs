using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeypressScript : MonoBehaviour
{
    [SerializeField] KeyCode testKey;
    [SerializeField] DoorScript door;

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
        if (Input.GetKeyDown(testKey))
        {
            door.UseDoor();
        }
    }
}
