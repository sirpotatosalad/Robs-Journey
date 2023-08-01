using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehaviour : MonoBehaviour
{
    public delegate void TriggerEventHandler(Collider2D collider);
    public event TriggerEventHandler OnTriggerEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            this.GetComponent<Collider2D>().enabled = false;
            Debug.Log("Disabled checkpoint collider");
            OnTriggerEvent?.Invoke(collision);
        }
    }


}