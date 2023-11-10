/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
using UnityEngine;

public class Skeleton : Enemy
{
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
