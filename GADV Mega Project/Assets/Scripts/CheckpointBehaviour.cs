using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehaviour : MonoBehaviour
{

    [SerializeField] private AudioClip checkpointSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.instance.PlaySound(checkpointSound);
            // disables the collider component of the checkpoint, rendering the checkpoint used.
            this.GetComponent<Collider2D>().enabled = false;
            //find the GameMaster singleton and set currentCheckpoint to the position of this checkpoint.
            GameMaster.gm.currentCheckpoint.position = transform.position;
            Debug.Log("Player crossed checkpoint @:" + transform.position);
        }
    }


}
