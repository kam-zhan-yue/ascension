using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        changeHealth(maxHealth);
        changeSpeed(maxSpeed);
        facingRight = true;
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("MakeAChoice", 2.0f, 5.0f);
        state = State.Patrolling;
    }

    // Update is called once per frame
    void FixedUpdate()
    { 

        if (selfDamaged)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageTime)
            {
                selfDamaged = false;
                damageTimer = 0;
            }
        }
        else
        {
            if (!attacking)
                CheckDistToPlayer();
            if (stop)
                return;
            Debug.Log(state);
            switch (state)
            {
                case State.Patrolling:
                    Patrol(choice, CanSeeGround(groundDetection, groundCheckDistance), CanSeeWall(wallDetection, wallCheckDistance, transform, wallLayer));
                    break;

                case State.Chasing:
                    Chase(maxSpeed, CanSeeGround(groundDetection, groundCheckDistance), CanSeeWall(wallDetection, wallCheckDistance, transform, wallLayer));
                    break;

                case State.Attacking:
                    Attack("Attacking", damageTime, attackTime, attackPoint,1,0.5f);
                    break;
            }
        }
    }
}
