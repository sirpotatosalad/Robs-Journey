using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class LadderBehaviour : MonoBehaviour
{

    private float vertical;
    private float speed = 10f;
    private bool isLadder;
    private bool isClimbing;

    [SerializeField] private Rigidbody2D rb;
    private ObjectInteractionController interactionController;

    // Start is called before the first frame update
    void Start()
    {
        interactionController = this.GetComponent<ObjectInteractionController>();
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");

        if (isLadder && Mathf.Abs(vertical) > 0f && !interactionController.isGrabbing)
        {
            isClimbing = true;
        }
    }

    void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2 (rb.velocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = 5f;
        }
    }


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