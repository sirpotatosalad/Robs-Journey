using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 15.0f;
    public float fallSpeed = 10.0f;
    public float frictionForce = 2.5f;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCount;
    private float jumpDelay = 0.2f;
    private float jumpDelayCount;

    private Rigidbody2D rb;

    private bool isHurt;

    private GameObject playerObj;
    private Vector2 playerPos;

    [SerializeField] private LayerMask groundLayer;
    private BoxCollider2D boxCollider;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerObj = GameObject.Find("Player");
        playerPos = playerObj.transform.position;
    }
    // Update is called once per frame
    void Update()
    {

        //moving left and right
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        // friction will be added via inspector menu in Unity. 
        Jump();
    }

    void FixedUpdate()
    {

    }


    void OnCollisionEnter2D(Collision2D obj)
    {
        // for now, just tp's player back to starting position when hitting a hazard.
        if (obj.gameObject.tag == "WorldBound")
        {
            playerObj.transform.position = playerPos;

        }
    }
    //method for jumping and aerial mechanics
    void Jump()
    {
        if (isGrounded())
        {
            coyoteTimeCount = coyoteTime;
        }
        else
        {
            coyoteTimeCount -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpDelayCount = jumpDelay;
        }
        else
        {
            jumpDelayCount -= Time.deltaTime;
        }

  
        if (jumpDelayCount > 0f && coyoteTimeCount > 0f)
        {   
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpDelayCount = 0f;
        }

        if (jumpDelayCount > 0f && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCount = 0f;
        }

        if (Input.GetKeyDown(KeyCode.S) && !isGrounded())
        {
            //need to find a better value for quick fall.
            //feels too slow at speed / 2, normal speed feels a tad too fast but i'm not sure
            rb.velocity = new Vector2(rb.velocity.x, -(fallSpeed));
        }
    }

    //this checks if the player is grounded
    private bool isGrounded()
    {
        //casts a box that extends slightly beneath the player to check if they are 'touching' the ground
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        //returns false if the collider is colliding with a gameObject 
        return raycastHit.collider != null;
    }
}
