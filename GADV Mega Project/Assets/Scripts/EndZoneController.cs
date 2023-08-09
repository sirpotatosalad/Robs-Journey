using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZoneController : MonoBehaviour
{
    [SerializeField]
    InventoryManager.AllItems requiredKey;
    private bool gateInteractable;

    //these are for handling the LevelEndEvent where the player unlocks the gate at the end of the level
    // the delegate will send over a boolean whether or not the player has done so
    public delegate void TriggerEventHandler(bool isCompleted);
    public event TriggerEventHandler OnLevelEndEvent;

    void Update()
    {
        // this block checks whether the player has the required Key to open the gate
        // it uses the same function in DoorBehaviourController
        if (Input.GetKeyDown(KeyCode.E) && gateInteractable)
        {
            if (HasKey(requiredKey))
            {
                Debug.Log("You did it! Get outta here.");
                OnLevelEndEvent?.Invoke(true);
            }
            else if (!HasKey(requiredKey))
            {
                Debug.Log("Missing required Key");
            }

        }
    }


    // same use as the DoorBehaviourController script, where these two Trigger functions check whether the player is in the gate's trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           gateInteractable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            gateInteractable = false;
        }
    }


    // Checks the player's inventory from inventoryManager singleton for the required key.
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
