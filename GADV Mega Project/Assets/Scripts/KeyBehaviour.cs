using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyBehaviour : MonoBehaviour
{
    //allows the selection of keyType in inspector
    [SerializeField]
    InventoryManager.AllItems itemType;
    [SerializeField]
    private AudioClip keyCollect;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //adds the key to the player's inventory when they collide into it
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.instance.PlaySound(keyCollect);
            InventoryManager.inventoryManager.AddItem(itemType);
            Destroy(gameObject);
        }
    }


}
