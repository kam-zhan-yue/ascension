using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Movement Variables
    public float speed;
    private float moveInput;
    private Rigidbody2D rb;
    private bool facingRight = true;

    //Ground Check Variables
    public static bool grounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;

    //Wall Check Variables
    public bool isTouchingWall;
    public Transform wallCheck;
    public float wallCheckDistance;
    private bool isWallSliding;
    public float wallSlideSpeed;

    //Jumping Variables
    public float jumpForce;
    public int extraJumps;
    public int extraJumpValue;
    public float jumpTimeCounter;
    public float jumpTime;
    public float fallMultiplier = 5f;
    public float lowJumpMultiplier = 2f;
    private bool isJumping;
    private bool isFalling;
    public static bool doubleJump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //Set movement to horizontal input
        moveInput = Input.GetAxis("Horizontal");
        //Debug.Log(moveInput);
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        //Flip character where necessary
        if (!facingRight && moveInput > 0)
            Flip();
        else if (facingRight && moveInput < 0)
            Flip();
    }
    
    void Update()
    {
        CheckSurroundings();
        Jump();
        WallSlide();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        wallCheckDistance *= -1;
        Debug.Log("Player flipped");
    }

    void CheckSurroundings()
    {
        //Check if player is grounded. If not, then player is jumping or falling
        grounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        //Check if player is touching a wall
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayer);
    }

    void WallSlide()
    {
        if (isTouchingWall && !grounded && rb.velocity.y < 0)
        {
            Debug.Log("Player is wall sliding");
            isWallSliding = true;
            extraJumps = extraJumpValue;
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
        else
            isWallSliding = false;
    }

    void Jump()
    {
        if (grounded)
        {
            extraJumps = extraJumpValue;
            doubleJump = false;
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            Debug.Log("Player Jump");
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0)
        {
            Debug.Log("Double Jump");
            extraJumps--;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
            isJumping = true;
            doubleJump = true;
            isFalling = false;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
                isJumping = false;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        if (!isJumping && !grounded && !doubleJump)
        {
            isFalling = true;
            Debug.Log("Player is falling");
        }
        else
            isFalling = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
