/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Health Variables
    public int health;
    public int maxHealth = 3;
    
    //Speed Variables
    public float speed;
    public float jumpForce;
    private float moveInput;

    public Rigidbody2D rb;
    public Animator animator;

    //Conditional Variables
    private bool facingRight = true;
    public bool grounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;

    //Wall Sliding Variables
    public bool isTouchingWall;
    public Transform wallCheck;
    public float wallCheckDistance;
    private bool isWallSliding;
    public float wallSlideSpeed;

    //Jumping Variables
    public int extraJumps;
    public int extraJumpValue;
    public float jumpTimeCounter;
    public float jumpTime;

    //Falling Variables
    public float fallMultiplier = 5f;
    public float lowJumpMultiplier = 2f;
    public bool isJumping;
    private bool isFalling;
    public bool doubleJump;
    public bool stopGroundCheck;

    //Damaged Variables
    public bool damaged;
    private float damageTimer;
    private float damageTime = 0.6f;
    
    //UI Elements
    HealthUI healthUI;
    PlayerUI playerUI;
    public PlayerCombat combat;
    public float newJumpForce;
    public float newSpeed;

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

    //===============PROCEDURE===============//
    void Flip()
    //Purpose:          Flips the x-axis of the player
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        wallCheckDistance *= -1;
    }

    void Update()
    {
        //Power Ups
        newJumpForce = jumpForce * FindObjectOfType<GameController>().jumpMultiplier;
        newSpeed = speed * FindObjectOfType<GameController>().movementMultiplier;
        if (damaged)
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

    //===============PROCEDURE===============//
    void MoveCharacter(float moveInput)
    //Purpose:          Moves the character in the direction of key input
    {
        rb.velocity = new Vector2(moveInput * newSpeed, rb.velocity.y);
    }

    //===============PROCEDURE===============//
    void CheckSurroundings()
    //Purpose:          Checks if the player is grounded or walled
    {
        //Check if player is grounded. If not, then player is jumping or falling
        grounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        //Check if player is touching a wall
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayer);
    }

    //===============PROCEDURE===============//
    void WallSlide(bool isWallSliing)
    //Purpose:          Makes the playerslide down a wall if at a wall and not on the ground
    {
        if (isTouchingWall && !grounded && rb.velocity.y<0)
        {
            isWallSliding = true;
            extraJumps = extraJumpValue;
            doubleJump = false;
            if(rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
        else
            isWallSliding = false;
    }

    //===============PROCEDURE===============//
    void Animate(bool doubleJump, bool isJumping, bool isFalling, float moveInput, bool isWallSliding)
    //Purpose:          Controls the player's animations
    {
        animator.SetBool("doubleJump", doubleJump);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("isWallSliding", isWallSliding);
    }

    //===============PROCEDURE===============//
    void Jump(bool grounded)
    //Purpose:          Responsible for jumping and double jumping
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
            rb.velocity = Vector2.up * newJumpForce;
            isJumping = true;
            doubleJump = true;
            isFalling = false;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * newJumpForce;
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

    //===============PROCEDURE===============//
    public void TakeDamage(Transform enemy, int damage, float knockback)
    //Purpose:          Makes the player take damage and stop attack
    {
        //If blocking, then don't damage
        if (combat.playerInvulnerable)
            return;
        if (health>0 && !damaged)
        {
            StartCoroutine(FindObjectOfType<CameraControl>().Shake(.05f, .1f));
            FindObjectOfType<AudioManager>().Play("PlayerHit");
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
            FindObjectOfType<HitStop>().Stop(0.1f);
            //If health goes below zero, die 
            if (health <= 0)
            {
                Debug.Log("Player has died");
                FindObjectOfType<AudioManager>().Play("GameOver");
                FindObjectOfType<AudioManager>().Play("PlayerDeath");
                FindObjectOfType<AudioManager>().Stop("Boss");
                FindObjectOfType<AudioManager>().Stop("RegularLevel");
                FindObjectOfType<AudioManager>().Stop("Victory");
                rb.velocity = Vector2.zero;
                animator.SetBool("Dead", true);
                playerUI.gameOver = true;
                Animate(false, false, false, 0, false);
                this.enabled = false;
            }
        }
    }

    //===============PROCEDURE===============//
    public void ChargeDamage(int damage, float knockback)
    //Purpose:          Similar to take damage but the player is sent up and loses 2 health
    {
        //If blocking, then don't damage
        if (combat.playerInvulnerable)
            return;
        StartCoroutine(FindObjectOfType<CameraControl>().Shake(.05f, .15f));
        FindObjectOfType<AudioManager>().Play("PlayerHit");
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
