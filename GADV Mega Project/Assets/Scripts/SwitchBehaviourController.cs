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

    private float switchSizeY;
    private Vector3 switchUpPos;
    private Vector3 switchDownPos;
    private float switchSpeed = 1f;
    private float switchDelay = 0.2f;
    bool isUsingSwitch = false;


    // Start is called before the first frame update
    void Start()
    {
        switchSizeY = transform.localScale.y / 2;
        switchUpPos = transform.position;
        switchDownPos = new Vector3(transform.position.x, transform.position.y - switchSizeY, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsingSwitch)
        {
            MoveSwitchDown();
        }
        else if (!isUsingSwitch)
        {
            MoveSwitchUp();
        }
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PhysObj"))
        {
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

    private void OnTriggerExit2D(Collider2D collision)
    {
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