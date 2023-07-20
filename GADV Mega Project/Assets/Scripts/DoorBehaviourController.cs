using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviourController : MonoBehaviour
{

    [SerializeField]
    private float doorOffset = 5f;

    public bool doorIsOpen = false;
    Vector3 doorClosedPos;
    Vector3 doorOpenPos;
    private float doorSpeed = 10f;


    // Start is called before the first frame update
    void Start()
    {
        doorClosedPos = transform.position;
        doorOpenPos = new Vector3(transform.position.x,transform.position.y + doorOffset, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (doorIsOpen)
        {
            OpenDoor();
        }
        else if (!doorIsOpen)
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        if (transform.position != doorOpenPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorOpenPos, doorSpeed * Time.deltaTime);
        }
    }

    void CloseDoor()
    {
        if (transform.position != doorClosedPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorClosedPos, doorSpeed * Time.deltaTime);
        }
    }

    
}
