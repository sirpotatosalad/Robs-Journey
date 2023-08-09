using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class DoorBehaviourController : MonoBehaviour
{

    [SerializeField]
    //used in inspector to set the required key (if any) to open the door
    InventoryManager.AllItems requiredKey;

    public bool doorIsOpen = false;
    [SerializeField]
    private bool isInteractable;

    SpriteRenderer sr;
    BoxCollider2D boxCollider;
    private bool playerCanInteract = false;


    // Start is called before the first frame update
    void Start()
    {
        // set variables to components
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if the player can interact, will check if they have the required key in their inventory.
        if (Input.GetKeyDown(KeyCode.E) && playerCanInteract)
        {
            if (isInteractable && HasKey(requiredKey))
            {
                doorIsOpen = true;
                InventoryManager.inventoryManager.RemoveItem(requiredKey);
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
    }


    //These OnTriggerEnter and OnTriggerExit functions checks if the player is at the door's box trigger
    //allows the player to interact when they are at the door
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player @ door");
            playerCanInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player @ door");
            playerCanInteract = false;
        }
    }

    // to open the door, i simply change the alpha value to "gray" it out and disable the collider
    void OpenDoor()
    {
        Color currentColour = sr.color;
        currentColour.a = 0.3f;
        sr.color = currentColour;
        boxCollider.enabled = false;
        
    }

    //boolean function to check if the player has a given key in their inventory
    //this is taken from the inventory manager singleton
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
