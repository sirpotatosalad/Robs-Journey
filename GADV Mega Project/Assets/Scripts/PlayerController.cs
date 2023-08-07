using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 15.0f;
    public float fallSpeed = 5.0f;
    public float frictionForce = 2.5f;

    private float horizontal;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCount;
    private float jumpDelay = 1f;
    private float jumpDelayCount;
    private bool isFacingRight = true;
    private Animator anim;

    private Rigidbody2D rb;
    private GameObject playerObj;

    [SerializeField] private LayerMask groundLayer;
    private BoxCollider2D boxCollider;
    private GameObject currentPlatform;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerObj = GameObject.Find("Player");
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        anim.SetBool("isRunning", horizontal != 0);



        Jump();
        Flip();

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentPlatform != null)
            {
                StartCoroutine(DisablePlatformCollision());
            }
        }

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentPlatform = null;
        }
    }

    //method for jumping and aerial mechanics
    void Jump()
    {

        // the next two if checks are for jump delay and coyote time
        // this is just to make the jumping feel more responsive and forgiving

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

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCount = 0f;
        }



    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f )
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    //this checks if the player is grounded
    private bool isGrounded()
    {
        //casts a box the size of player's box collider beneath player gameobject 
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.05f, groundLayer);
        //returns false if the collider is colliding with a gameObject 
        return raycastHit.collider != null;
    }

    private IEnumerator DisablePlatformCollision()
    {
        BoxCollider2D platformCollider = currentPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(boxCollider, platformCollider);
        yield return new WaitForSeconds(1);
        Physics2D.IgnoreCollision(boxCollider, platformCollider, false);
    }

}
