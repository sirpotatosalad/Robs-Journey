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
            // disables the collider component of the checkpoint, rendering the checkpoint used.
            this.GetComponent<Collider2D>().enabled = false;
            //find the GameMaster object and set the current checkpoint to the position of the current one
            GameMaster gameMaster = GameObject.FindObjectOfType<GameMaster>();
            gameMaster.currentCheckpoint.position = transform.position;
            Debug.Log("Checkpoint set to:" + transform.position);
            OnTriggerEvent?.Invoke(collision);
        }
    }


}
