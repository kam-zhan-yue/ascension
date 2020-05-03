using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        FindGameController();
        type = "skeleton";
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        changeSpeed(maxSpeed);
        facingRight = true;
        player = GameObject.FindGameObjectWithTag("Player");
        changeHealth(maxHealth + GC.GetMultiplier() * 50);
        setHealth();
        InvokeRepeating("MakeAChoice", 2.0f, 5.0f);
        state = State.Patrolling;
        pointsToGive = 10 + GC.GetMultiplier() * 5;
    }
    private void Update()
    {
        HealthController();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Error Prevention
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        if (selfDamaged)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageTime)
            {
                selfDamaged = false;
                damageTimer = 0;
            }
        }
        else if (player)
        {
            if (!attacking)
                CheckDistToPlayer();
            if (stop)
                return;
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
