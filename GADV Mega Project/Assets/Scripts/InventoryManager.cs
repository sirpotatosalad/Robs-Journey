using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // making a static variable for inventoryManger instance
    public static InventoryManager inventoryManager { get; private set; }
    
    // making a public list of all items currently in the player's inventory
    public List<AllItems> invItems = new List<AllItems>();

    void Awake()
    {
        // making inventoryManager a singleton pattern
        if (inventoryManager == null)
        {
            inventoryManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    // method other scripts can call from inventoryManager instance to add specific items to the player's inventory
    public void AddItem(AllItems item)
    {
        if (!invItems.Contains(item))
        {
            invItems.Add(item);
        }
    }

    // method other scripts can call from inventoryManager instance to remove an item from the player's inventory
    public void RemoveItem(AllItems item)
    {
        if (invItems.Contains(item))
        {
            invItems.Remove(item);
        }
    }

    // list of enums that contain all key types
    public enum AllItems
    {
        KeyGold,
        KeySilver
    }
}
