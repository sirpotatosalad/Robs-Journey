using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class SwitchBehaviourController : MonoBehaviour
{

    [SerializeField] DoorBehaviourController doorBehaviour;

    [SerializeField] bool isDoorOpenSwitch;
    [SerializeField] bool isDoorCloseSwitch;
    [SerializeField] bool isPlayerInteractable;

    private float switchSizeY;
    private Vector3 switchUpPos;
    private Vector3 switchDownPos;
    private float switchSpeed = 1f;
    private float switchDelay = 0.2f;
    private bool isUsingSwitch = false;
    private bool isPhysObjOnSwitch = false;



    // Start is called before the first frame update
    void Start()
    {
        switchSizeY = transform.localScale.y / 2;
        switchUpPos = transform.position;
        // this is the position the switch will be in when it is presed
        switchDownPos = new Vector3(transform.position.x, transform.position.y - switchSizeY, transform.position.z);
    }


    void Update()
    {
        // moves the switch to its up and down pos depending on whether it is being used
        if (isUsingSwitch)
        {
            MoveSwitchDown();
        }
        // the switch will stay down if there is a physobj on the switch
        else if (!isUsingSwitch && !isPhysObjOnSwitch)
        {
            MoveSwitchUp();
        }
    }

    // MoveSwitchDown() and MoveSwitchUp() share the same logic
    // basically, when the siwtch is not already in its position, it will move either downward or upward to simulate it being pressed down
    void MoveSwitchDown()
    {

        if (transform.position != switchDownPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, switchDownPos, switchSpeed * Time.deltaTime);
        }
    }

    void MoveSwitchUp()
    {
        if (transform.position != switchUpPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, switchUpPos, switchSpeed * Time.deltaTime);
        }
    }

    // here it will handle the logic when it comes to opening the door it is attached to
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the switch is pressed down using a physObj or throwableObj, it will trigger the isPhysObj on switch bool
        // afterward, based on how it is set up in the inspector, it can open and close the door it is attached to.
        // this is done by accessing the doorIsOpen bool in the attached door.
        if (collision.CompareTag("PhysObj") || collision.CompareTag("Throwable"))
        {
            Debug.Log("Obj on switch");
            isPhysObjOnSwitch = true;
            isUsingSwitch = !isUsingSwitch;

            if (isDoorOpenSwitch && !doorBehaviour.doorIsOpen)
            {
                doorBehaviour.doorIsOpen = !doorBehaviour.doorIsOpen;
            }
            else if (isDoorCloseSwitch && doorBehaviour.doorIsOpen)
            {
                doorBehaviour.doorIsOpen = !doorBehaviour.doorIsOpen;
            }

            return;
        }

        // the only difference here is that the switch must be set to interactable in order for the player to use the switch
        // this feature won't be showcased in the game itself as i couldn't find the time to design another segment to implement this in.
        if (collision.CompareTag("Player") && isPlayerInteractable)
        {

            if (!isPhysObjOnSwitch)
            {
                // the same logic here applies as above for lines 79-88
                isUsingSwitch = !isUsingSwitch;

                if (isDoorOpenSwitch && !doorBehaviour.doorIsOpen)
                {
                    doorBehaviour.doorIsOpen = !doorBehaviour.doorIsOpen;
                }
                else if (isDoorCloseSwitch && doorBehaviour.doorIsOpen)
                {
                    doorBehaviour.doorIsOpen = !doorBehaviour.doorIsOpen;
                }
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // sets the isPhysObjOnSwitch to false once the object is taken off.
        if (collision.CompareTag("PhysObj") || collision.CompareTag("Throwable"))
        {
            Debug.Log("Obj off switch");
            isPhysObjOnSwitch = false;
        }


        // when the player leaves the switch, has a short delay via a coroutine to allow the switch to move back up
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(SwitchPressDelay(switchDelay));
        }
    }

    IEnumerator SwitchPressDelay(float delayTime) 
    { 
        yield return new WaitForSeconds(delayTime);
        isUsingSwitch = false;
    }

}
