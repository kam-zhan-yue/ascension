using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : Enemy
{
    float distanceToPlayer;
    // Start is called before the first frame update
    void Start()
    {
        FindGameController();
        type = "eye";
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        changeHealth(maxHealth + GC.GetMultiplier() * 20);
        setHealth();
        changeSpeed(maxSpeed);
        facingRight = false;
        player = GameObject.FindGameObjectWithTag("Player");
        //InvokeRepeating("MakeAChoice", 2.0f, 5.0f);
        state = State.Patrolling;
        localScale = transform.localScale;
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
        else if (player != null)
        {
            if(!attacking)
                CheckDistToPlayer();
            if (stop)
                return;
            switch (state)
            {
                case State.Patrolling:
                    Idle();
                    break;
                case State.Chasing:
                    Chase();
                    break;
                case State.Attacking:
                    Attack();
                    break;
            }
        }

    }

    public new void CheckDistToPlayer()
    {
        attacking = false;
        animator.SetBool("Attacking", false);
        playerDamaged = false;
        distanceToPlayer = Vector2.Distance(transform.position,player.transform.position);
        if (distanceToPlayer < attackRange)
            state = State.Attacking;
        else if (distanceToPlayer < aggroRange)
            state = State.Chasing;
        else if (distanceToPlayer < 15)
        {
            state = State.Patrolling;
            stop = false;
        }
        else
            stop = true;
    }

    private void Idle()
    {
        animator.SetBool("Attacking", false);
        //Debug.Log("Player is not in aggro range");
    }

    public void Attack()
    {
        attacking = true;
        rb.velocity = Vector2.zero;
        attackTimer += Time.deltaTime;
        //Debug.Log("Eye is attacking Player");
        animator.SetBool("Attacking", attacking);
        //If not damaged yet and at the attack frame, then damage
        if (!playerDamaged && attackTimer >= 0.7f)
        {
            Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
            FindObjectOfType<AudioManager>().Play("EyeAttack");
            if (hitPlayer != null)
            {
                //Damage the player
                player.GetComponent<PlayerController>().TakeDamage(transform, 1, 0.5f);
                Debug.Log("Player is hit!");
            }
            playerDamaged = true;
        }
        //If the attack animation is finished
        if (attackTimer >= attackTime)
        {
            Debug.Log("Reset attack timer");
            attacking = false;
            animator.SetBool("Attacking", false);
            attackTimer = 0;
        }
    }

    private void Chase()
    {
        //Debug.Log("Chase player");
        //If player is on the right
        if(transform.position.x<player.transform.position.x)
        {
            //Flip to the right
            Debug.Log("Flip right");
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingRight = true;
        }
        //If player is on the left
        else if(transform.position.x>player.transform.position.x)
        {
            Debug.Log("Flip left");
            transform.eulerAngles = new Vector3(0, -180, 0);
            facingRight = false;
        }
        //gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * getSpeed() * Time.deltaTime);
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        rb.velocity = getSpeed() * direction * 4;
        //Vector2 velocity = new Vector2((transform.position.x - player.transform.position.x) * getSpeed(), (transform.position.y - player.transform.position.y) * getSpeed());
    }
}
