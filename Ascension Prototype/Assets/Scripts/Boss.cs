using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public enum BossState
    {
        Attack,
        Chase,
        Charge,
        Retreat,
    }

    public BossState bossState;

    public Transform lightAttackPoint;
    public Transform heavyAttackPoint;
    public float lightAttackRange;
    public float heavyAttackRange;
    public float runningSpeed;
    public float chargingSpeed;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        maxHealth = 1000;
        changeHealth(maxHealth);
        maxSpeed = runningSpeed;
        changeSpeed(runningSpeed);
        facingRight = false;
        player = GameObject.FindGameObjectWithTag("Player");
        //InvokeRepeating("MakeAChoice", 2.0f, 5.0f);
        state = State.Patrolling;
        bossState = BossState.Chase;
    }

    private void FixedUpdate()
    {
        Chase(maxSpeed, CanSeeWall(wallDetection, wallCheckDistance, transform, wallLayer));
    }

    public new void CheckDistToPlayer()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distToPlayer < attackRange)
        {
            bossState = BossState.Attack;
        }
        else
        {
            bossState = BossState.Chase;
        }
    }

    public void Chase(float maxSpeed, bool wallInfo)
    {
        //If boss is on the left of the player, turn right and chase
        if (transform.position.x < player.transform.position.x && VerticalDistToPlayer() < 1.2f)
        {
            //If not at a wall, chase
            if (!wallInfo)
            {
                //Flip right and move right
                Debug.Log("Player is to the right, move right");
                animator.SetBool("Walking", true);
                transform.eulerAngles = new Vector3(0, -180, 0);
                changeSpeed(Mathf.Abs(runningSpeed));
                facingRight = true;
                rb.velocity = new Vector2(getSpeed(), rb.velocity.y);
            }
            //If at a wall and not facing right, then turn and chase
            else if (!facingRight)
            {
                //Flip right and move right
                Debug.Log("Player is to the right, move right");
                animator.SetBool("Walking", true);
                transform.eulerAngles = new Vector3(0, -180, 0);
                changeSpeed(Mathf.Abs(runningSpeed));
                facingRight = true;
                rb.velocity = new Vector2(getSpeed(), rb.velocity.y);
            }
        }
        //If boss is on the right of the player, turn left and chase
        else if (transform.position.x > player.transform.position.x && VerticalDistToPlayer() < 1.2f)
        {
            if (!wallInfo)
            {
                //Flip to the left and move left 
                Debug.Log("Player is to the left, move left");
                animator.SetBool("Walking", true);
                transform.eulerAngles = new Vector3(0, 0, 0);
                changeSpeed(-Mathf.Abs(runningSpeed));
                facingRight = false;
                rb.velocity = new Vector2(getSpeed(), rb.velocity.y);
            }
            else if (!facingRight)
            {
                //Flip to the left and move left 
                Debug.Log("Player is to the left, move left");
                animator.SetBool("Walking", true);
                transform.eulerAngles = new Vector3(0, 0, 0);
                changeSpeed(-Mathf.Abs(runningSpeed));
                facingRight = false;
                rb.velocity = new Vector2(getSpeed(), rb.velocity.y);
            }
        }
        else
            animator.SetBool("Walking", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(lightAttackPoint.position, lightAttackRange);
        Gizmos.DrawWireSphere(heavyAttackPoint.position, heavyAttackRange);
        Gizmos.DrawLine(wallDetection.position, new Vector3(wallDetection.position.x - wallCheckDistance, wallDetection.position.y, wallDetection.position.z));
    }
}
