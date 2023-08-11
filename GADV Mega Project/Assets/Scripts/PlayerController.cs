using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float jumpForce = 15.0f;
    [SerializeField] private AudioClip jumpSound;
    

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

        // shorthand to set isRunning to true when horizontal axis changes (i.e., player moves left or right)
        anim.SetBool("isRunning", horizontal != 0);



        Jump();
        Flip();

        // allows player to fall through one-way platforms when pressing the S key.
        if (Input.GetKeyDown(KeyCode.S))
        {
            // starts the corotine to fall through one-way platform if player is currently standing on one.
            if (currentPlatform != null)
            {
                StartCoroutine(DisablePlatformCollision());
            }
        }

    }

    // left and right movement in fixed update to mitigate any changes in movement due to frame rates
    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }


    // OnCollisionEnter and OnCollisionExit to check if the player is currently standing on a one-way platform
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
        
        // checks if the player is grounded, 
        // starts counting down the coyote timer once player leaves the ground.
        if (isGrounded())
        {
            coyoteTimeCount = coyoteTime;
        }
        else
        {
            coyoteTimeCount -= Time.deltaTime;
        }

        // similar logic to coyote time, 
        // instead this counts down the jump delay timer once the player jumps
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpDelayCount = jumpDelay;
        }
        else
        {
            jumpDelayCount -= Time.deltaTime;
        }


        // checks if the jump delay and coyote time have reset,
        // allows the player to jump if so by adding velocity to the player's rb via jumpForce
        if (jumpDelayCount > 0f && coyoteTimeCount > 0f)
        {
            SoundManager.instance.PlaySound(jumpSound);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpDelayCount = 0f;
        }

        // this allows the player to control the height of their jump by holding the jump key.
        // it checks if the space key is held down and if the player is already jumping (i.e., has a velocity > 0f on the y axis)
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            // the jumping height is modified by adding extra velocity to the player's rb
            // in this case, it will be by 0.5f * the player's current upward velocity
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCount = 0f;
        }



    }

    //method allows the player sprite to flip left and right
    private void Flip()
    {
        // checks if the input along the horizontal axis != isFacingRight bool
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f )
        {
            // switches the bool for isFacingRight
            isFacingRight = !isFacingRight;
            // transforming the player's localScale along x-axis by multiplying it to -1
            // this inverts the sprite to face the other direction
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    //this checks if the player is grounded
    private bool isGrounded()
    {
        //casts a box the size of player's box collider beneath player gameobject 
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        //returns false if the collider is colliding with a gameObject that is not the groundLayer
        return raycastHit.collider != null;
    }

    // coroutine to allow player to fall through one-way platforms
    private IEnumerator DisablePlatformCollision()
    {
        BoxCollider2D platformCollider = currentPlatform.GetComponent<BoxCollider2D>();

        // ignores collision between the player's box collider and the one-way platform's collider
        // yields for 1 second to allow player to fully fall through before re-enabling collision between the player and platform
        Physics2D.IgnoreCollision(boxCollider, platformCollider);
        yield return new WaitForSeconds(1);
        Physics2D.IgnoreCollision(boxCollider, platformCollider, false);
    }

}
