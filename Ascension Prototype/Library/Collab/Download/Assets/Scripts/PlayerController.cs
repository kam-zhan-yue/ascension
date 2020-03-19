using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;

    private Rigidbody2D rb;
    public Animator animator;

    private bool facingRight = true;

    public static bool grounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;

    public bool isTouchingWall;
    public Transform wallCheck;
    public float wallCheckDistance;
    private bool isWallSliding;
    public float wallSlideSpeed;

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
        MoveCharacter(moveInput);

        //Flip character where necessary
        if (!facingRight && moveInput > 0)
            Flip();
        else if (facingRight && moveInput < 0)
            Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        wallCheckDistance *= -1;
    }

    void Update()
    {
        CheckSurroundings();
        Animate(doubleJump, isJumping, isFalling, moveInput, isWallSliding);
        Jump(grounded);
        WallSlide(isWallSliding);
    }

    void MoveCharacter(float moveInput)
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    void CheckSurroundings()
    {
        //Check if player is grounded. If not, then player is jumping or falling
        grounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        //Check if player is touching a wall
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayer);
    }

    void WallSlide(bool isWallSliing)
    {
        if (isTouchingWall && !grounded && rb.velocity.y<0)
        {
            isWallSliding = true;
            extraJumps = extraJumpValue;
            if(rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
        else
            isWallSliding = false;
    }

    void Animate(bool doubleJump, bool isJumping, bool isFalling, float moveInput, bool isWallSliding)
    {
        animator.SetBool("doubleJump", doubleJump);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("isWallSliding", isWallSliding);
    }

    void Jump(bool grounded)
    {
        if (grounded)
        {
            extraJumps = extraJumpValue;
            doubleJump = false;
            isJumping = false;
            Animate(doubleJump, isJumping, isFalling, moveInput, isWallSliding);
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            Debug.Log("Player Jump");
            isJumping = true;
            jumpTimeCounter = jumpTime;
            //rb.velocity = Vector2.up * jumpForce;
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
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x+wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
