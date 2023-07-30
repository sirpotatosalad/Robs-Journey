using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager inventoryManager;
    
    public List<AllItems> invItems = new List<AllItems>();

    void Awake()
    {
        inventoryManager = this;
    }

    public void AddItem(AllItems item)
    {
        if (!invItems.Contains(item))
        {
            invItems.Add(item);
        }
    }

    public void RemoveItem(AllItems item)
    {
        if (invItems.Contains(item))
        {
            invItems.Remove(item);
        }
    }

    public enum AllItems
    {
        KeyGold,
        KeySilver
    }
}
