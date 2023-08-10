using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LadderBehaviour : MonoBehaviour
{

    private float vertical;
    private float speed = 10f;
    private bool isLadder;
    private bool isClimbing;

    [SerializeField] private Rigidbody2D rb;
    private ObjectInteractionController interactionController;

    void Start()
    {
        interactionController = this.GetComponent<ObjectInteractionController>();
    }

    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        //if the player is "using" a ladder, set isClimbing bool to true
        if (isLadder && Mathf.Abs(vertical) > 0f && !interactionController.isGrabbing)
        {
            isClimbing = true;
        }
    }

    void FixedUpdate()
    {

        if (isClimbing)
        {
            //when isClimbing, remove gravity acting on player
            //simulate climbing up by floating player up by adding velocity upward.
            rb.gravityScale = 0f;
            rb.velocity = new Vector2 (rb.velocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = 5f;
        }
    }

    // the OnTriggerEnter and OnTriggerExit check if the player is inside the ladder's trigger, rendering them to be "using" the ladder
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }


}
