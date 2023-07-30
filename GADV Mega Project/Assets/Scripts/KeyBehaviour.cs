using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyBehaviour : MonoBehaviour
{
    [SerializeField]
    InventoryManager.AllItems itemType;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            InventoryManager.inventoryManager.AddItem(itemType);
            Destroy(gameObject);
        }
    }
}
