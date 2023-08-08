using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class DoorBehaviourController : MonoBehaviour
{

    [SerializeField]
    private float doorOffset = 5f;
    [SerializeField]
    InventoryManager.AllItems requiredKey;

    public bool doorIsOpen = false;
    public bool isInteractable;
    public bool isLocked;

    Vector3 doorClosedPos;
    Vector3 doorOpenPos;
    SpriteRenderer sr;
    BoxCollider2D boxCollider;
    private float doorSpeed = 10f;
    private bool playerCanInteract = false;


    // Start is called before the first frame update
    void Start()
    {
        doorClosedPos = transform.position;
        doorOpenPos = new Vector3(transform.position.x,transform.position.y + doorOffset, transform.position.y);
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && playerCanInteract)
        {
            if (isInteractable && HasKey(requiredKey))
            {
                isLocked = false;
                doorIsOpen = true;
                InventoryManager.inventoryManager.RemoveItem(requiredKey);
            }
            else if (isLocked)
            {
                Debug.Log("Door is locked");
            }
            else
            {
                Debug.Log("Door not interactable");
            }
        }

        if (doorIsOpen)
        {
            OpenDoor();
        }
        else if (!doorIsOpen)
        {
            CloseDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player @ door");
        playerCanInteract = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Player left door");
        playerCanInteract = false;
    }

    void OpenDoor()
    {
        //if (transform.position != doorOpenPos)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, doorOpenPos, doorSpeed * Time.deltaTime);
        //}
        Color currentColour = sr.color;
        currentColour.a = 0.3f;
        sr.color = currentColour;
        boxCollider.enabled = false;
        
    }

    void CloseDoor()
    {
        if (transform.position != doorClosedPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorClosedPos, doorSpeed * Time.deltaTime);
        }
    }

    public bool HasKey(InventoryManager.AllItems itemRequired)
    {
        if (InventoryManager.inventoryManager.invItems.Contains(itemRequired))
        {
            return true;
        }
        else 
        { 
            return false; 
        }
    }


}
