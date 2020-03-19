﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public bool dead;
    public DestroyMe destroyScript;

    public enum State
    {
        Patrolling,
        Chasing,
        Attacking,
    }

    public State state;

    //Detection Variables
    public Transform groundDetection;
    public float groundCheckDistance;
    public Transform wallDetection;
    public float wallCheckDistance;
    public LayerMask wallLayer;

    public bool facingRight;
    public float maxSpeed = 1;
    private float speed;
    public int maxHealth = 100;
    private int currentHealth;

    private int counter;
    public int choice;
    
    public GameObject player;
    public float aggroRange;
    public float attackRange;
    public bool attacking;
    public bool playerDamaged;
    public float attackTime;
    public float attackTimer;
    public Transform attackPoint;
    public LayerMask playerLayer;

    public bool selfDamaged;
    public float damageTimer;
    public float damageTime = 0.7f;

    public bool stop;

    //private void Start()
    //{
    //    animator = GetComponent<Animator>();
    //    rb = GetComponent<Rigidbody2D>();
    //    currentHealth = maxHealth;
    //    speed = maxSpeed;
    //    facingRight = true;
    //    player = GameObject.FindGameObjectWithTag("Player");
    //    InvokeRepeating("MakeAChoice", 2.0f, 5.0f);
    //    state = State.Patrolling;
    //}

    public void changeHealth(int newHealth)
    {
        currentHealth = newHealth;
    }
    public void changeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public float getSpeed()
    {
        return speed;
    }

    //void Update()
    //{
    //    if(selfDamaged)
    //    {
    //        damageTimer += Time.deltaTime;
    //        if(damageTimer>=damageTime)
    //        {
    //            selfDamaged = false;
    //            damageTimer = 0;
    //        }
    //    }
    //    else
    //    {
    //        if (!attacking)
    //            CheckDistToPlayer();

    //        Debug.Log(state);
    //        switch (state)
    //        {
    //            case State.Patrolling:
    //                Patrol(choice, CanSeeGround(groundDetection, groundCheckDistance), CanSeeWall(wallDetection, wallCheckDistance, transform, wallLayer));
    //                break;

    //            case State.Chasing:
    //                Chase(maxSpeed, CanSeeGround(groundDetection, groundCheckDistance), CanSeeWall(wallDetection, wallCheckDistance, transform, wallLayer));
    //                break;

    //            case State.Attacking:
    //                Attack();
    //                break;
    //        }
    //    }
    //}

    public void Attack()
    {
        attacking = true;
        attackTimer += Time.deltaTime;
        //Debug.Log("Player spotted, attacking...");
        animator.SetBool("Walking", false);
        animator.SetBool("Attacking",attacking);

        //If not damaged yet and at the attack frame, then damage
        if(!playerDamaged && attackTimer >= 0.7f)
        {
            Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
            if(hitPlayer != null)
            {
                //Damage the player
                player.GetComponent<PlayerController>().TakeDamage(transform,2f, 0.5f);
                Debug.Log("Player is hit!");
            }
            playerDamaged = true;
        }

        //If the attack animation is finished
        if(attackTimer >= attackTime)
        {
            attacking = false;
            attackTimer = 0;
        }
    }

    public void MakeAChoice()
    {
        choice = (int)Random.Range(1, 3);
    }

    public void CheckDistToPlayer()
    {
        animator.SetBool("Attacking", attacking);
        playerDamaged = false;
        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
        //Debug.Log(distToPlayer);
        //Attack player if within range
        if (distToPlayer < attackRange)
            state = State.Attacking;
        //Chase player if within range, but not attacking range
        else if (distToPlayer < aggroRange)
            state = State.Chasing;
        //Patrol around if no player is within range
        else if (distToPlayer < 15f)
        {
            state = State.Patrolling;
            stop = false;
        }
        else
            stop = true;
    }

    //Check whether the enemy can see the ground
    public static bool CanSeeGround(Transform groundDetection, float groundCheckDistance)
    {
        bool groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, groundCheckDistance);
        return groundInfo;
    }

    //Check whether the enemy can see the wall
    public static bool CanSeeWall(Transform wallDetection, float wallCheckDistance, Transform transform, LayerMask wallLayer)
    {
        bool wallInfo = Physics2D.Raycast(wallDetection.position, transform.right, wallCheckDistance, wallLayer);
        return wallInfo;
    }

    public float VerticalDistToPlayer()
    {
        float distToPlayer = Mathf.Abs(transform.position.y - player.transform.position.y);
        return distToPlayer;
    }

    public void Chase(float maxSpeed, bool groundInfo, bool wallInfo)
    {
        //If enemy is on the left of the player
        if (transform.position.x < player.transform.position.x && VerticalDistToPlayer() < 1.5f)
        {
            //If not at the edge or not at a wall, chase
            if (groundInfo && !wallInfo)
            {
                //Flip right and move right
                Debug.Log("Player is to the right, move right");
                animator.SetBool("Walking", true);
                transform.eulerAngles = new Vector3(0, 0, 0);
                speed = Mathf.Abs(maxSpeed);
                facingRight = true;
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            //If at the edge of a platform or at a wall and not facing right, then turn and chase
            else if (!facingRight)
            {
                //Flip right and move right
                Debug.Log("Player is to the left, move left");
                animator.SetBool("Walking", true);
                transform.eulerAngles = new Vector3(0, 0, 0);
                speed = Mathf.Abs(maxSpeed);
                facingRight = true;
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
        }
        //If enemy is on the right of the player
        else if (transform.position.x > player.transform.position.x && VerticalDistToPlayer() < 1.5f)
        {
            //If not at the edge or not at a wall, chase
            if (groundInfo && !wallInfo)
            {
                //Flip to the left and move left
                Debug.Log("Player is to the right, move right");
                animator.SetBool("Walking", true);
                transform.eulerAngles = new Vector3(0, -180, 0);
                speed = -Mathf.Abs(maxSpeed);
                facingRight = false;
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else if(facingRight)
            {
                //Flip to the Left and move left 
                Debug.Log("Player is to the left, move left");
                animator.SetBool("Walking", true);
                transform.eulerAngles = new Vector3(0, -180, 0);
                speed = -Mathf.Abs(maxSpeed);
                facingRight = false;
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
        }
        else
            animator.SetBool("Walking", false);
    }
    
    public void Patrol(int choice, bool groundInfo, bool wallInfo)
    {
        switch (choice)
        {
            case 1:
                animator.SetBool("Walking", true);
                rb.velocity = new Vector2(speed, rb.velocity.y);
                if (!groundInfo || wallInfo)
                {
                    if (facingRight)
                    {
                        Debug.Log("Turn left!");
                        transform.eulerAngles = new Vector3(0, -180, 0);
                        speed *= -1;
                        facingRight = false;
                    }
                    else
                    {
                        Debug.Log("Turn right!");
                        transform.eulerAngles = new Vector3(0, -0, 0);
                        speed *= -1;
                        facingRight = true;
                    }
                }
                break;
            case 2:
                animator.SetBool("Walking", false);
                break;
        }
    }

    public void TakeDamage(int damage, float knockback)
    {
        animator.SetBool("Walking", false);
        selfDamaged = true;
        //Take damage
        currentHealth -= damage;
        //Apply knockback force to the left if player is on the right
        if (transform.position.x < player.transform.position.x)
        {
            if (!attacking)
            {
                Debug.Log("Add force to left");
                rb.velocity = Vector2.zero;
                rb.velocity += new Vector2(-knockback, rb.velocity.y);
            }
            else
                rb.velocity += new Vector2(-2, rb.velocity.y);
        }
        else
        {
            if (!attacking)
            {
                Debug.Log("Add force to right");
                rb.velocity = Vector2.zero;
                rb.velocity += new Vector2(knockback, rb.velocity.y);
            }
            else
                rb.velocity += new Vector2(2, rb.velocity.y);
        }

        //Check if the enemy is dead
        if (currentHealth<=0)
            Die();
        //Play hurt animation
        else if(!attacking)
            animator.SetTrigger("Hurt");
    }

    public void Die()
    {
        Debug.Log("Enemy died");
        animator.SetTrigger("Dead");
        animator.SetBool("Walking", false);
        animator.SetBool("Attacking", false);
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().isKinematic = true;
        dead = true;
        if (destroyScript != null)
            destroyScript.death = true;
        this.enabled = false;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(groundDetection.position, new Vector3(groundDetection.position.x, groundDetection.position.y-groundCheckDistance, groundDetection.position.z));
        //Gizmos.DrawLine(wallDetection.position, new Vector3(wallDetection.position.x + wallCheckDistance, wallDetection.position.y, wallDetection.position.z));
        //Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
