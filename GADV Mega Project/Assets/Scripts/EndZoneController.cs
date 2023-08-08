using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZoneController : MonoBehaviour
{
    [SerializeField]
    InventoryManager.AllItems requiredKey;

    public delegate void TriggerEventHandler(Collider2D collision);
    public event TriggerEventHandler OnLevelEndEvent;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            if (HasKey(requiredKey))
            {
                Debug.Log("You did it! Get outta here.");
                OnLevelEndEvent?.Invoke(collision);
            }
            else if (!HasKey(requiredKey))
            {
                Debug.Log("Missing required Key");
            }

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
