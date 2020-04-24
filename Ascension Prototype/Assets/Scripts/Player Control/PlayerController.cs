using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health;
    public int maxHealth = 3;
    
    public float speed;
    public float jumpForce;
    private float moveInput;

    public Rigidbody2D rb;
    public Animator animator;

    private bool facingRight = true;

    public bool grounded;
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
    public bool isJumping;
    private bool isFalling;
    public bool doubleJump;
    public bool stopGroundCheck;

    public bool damaged;
    private float damageTimer;
    private float damageTime = 0.6f;
    
    //UI Elements
    HealthUI healthUI;
    PlayerUI playerUI;
    public PlayerCombat combat;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        healthUI = GameObject.FindGameObjectWithTag("HealthUI").GetComponent<HealthUI>();
        playerUI = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<PlayerUI>();
    }

    void FixedUpdate()
    {
        if(!damaged)
        {
            if (combat.playerInvulnerable)
                return;
            //Set movement to horizontal input
            moveInput = Input.GetAxis("Horizontal");
            MoveCharacter(moveInput);

            //Flip character where necessary
            if (!facingRight && moveInput > 0)
                Flip();
            else if (facingRight && moveInput < 0)
                Flip();
        }
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
        if(damaged)
        {
            animator.SetBool("Hurt", true);
            Animate(false, false, false, 0, false);
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageTime)
            {
                animator.SetBool("Hurt", false);
                damaged = false;
                damageTimer = 0;
            }
        }
        else
        {
            CheckSurroundings();
            if (combat.playerInvulnerable)
                return;
            Jump(grounded);
            Animate(doubleJump, isJumping, isFalling, moveInput, isWallSliding);
            WallSlide(isWallSliding);
        }
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
        if (grounded && !stopGroundCheck)
        {
            extraJumps = extraJumpValue;
            doubleJump = false;
            isJumping = false;
            Animate(doubleJump, isJumping, isFalling, moveInput, isWallSliding);
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            //Debug.Log("Player Jump");
            isJumping = true;
            jumpTimeCounter = jumpTime;
            //rb.velocity = Vector2.up * jumpForce;
            if (grounded)
            {
                extraJumps--;
                stopGroundCheck = true;
            }
            else
                extraJumps-=2;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0)
        {
            //Debug.Log("Double Jump");
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
                if(jumpTimeCounter<0.165)
                    stopGroundCheck = false;
            }
            else
                isJumping = false;
        }
        //Catch the error if get key happens before jumptimecounter = 2
        else if(Input.GetKey(KeyCode.Space)== false && isJumping)
        {
            stopGroundCheck = false;
            isJumping = false;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        if (!isJumping && !grounded && !doubleJump)
        {
            isFalling = true;
            //Debug.Log("Player is falling");
        }
        else
            isFalling = false;
    }

    public void TakeDamage(Transform enemy, int damage, float knockback)
    {
        //If blocking, then don't damage
        if (combat.playerInvulnerable)
            return;
        if (health>0 && !damaged)
        {
            healthUI.Hurt(health);
            if(damage==2)
                healthUI.Hurt(health-1);
            health -= damage;
            damaged = true;
            rb.velocity = Vector2.zero;
            //Decrease health
            Debug.Log("Player took damage");
            //If enemy is to the left of the player
            if (enemy.position.x < transform.position.x)
            {
                Debug.Log("Player - Add force to right");
                //Apply a force to the right
                rb.velocity += new Vector2(knockback, rb.velocity.y);
            }
            //If enemy is to the right of the player
            else
            {
                Debug.Log("Player - Add force to left");
                //Apply a force to the left
                rb.velocity += new Vector2(-knockback, rb.velocity.y);
            }
            //If health goes below zero, die 
            if (health <= 0)
            {
                Debug.Log("Player has died");
                rb.velocity = Vector2.zero;
                animator.SetBool("Dead", true);
                playerUI.gameOver = true;
                Animate(false, false, false, 0, false);
                this.enabled = false;
            }
        }
    }

    public void ChargeDamage(int damage, float knockback)
    {
        //If blocking, then don't damage
        if (combat.playerInvulnerable)
            return;
        rb.velocity = Vector2.zero;
        rb.velocity += new Vector2(0, knockback);
        healthUI.Hurt(health);
        healthUI.Hurt(health-1);
        health -= damage;
        damaged = true;
        if (health <= 0)
        {
            Debug.Log("Player has died");
            animator.SetBool("Dead", true);
            playerUI.gameOver = true;
            Animate(false, false, false, 0, false);
            this.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x+wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
