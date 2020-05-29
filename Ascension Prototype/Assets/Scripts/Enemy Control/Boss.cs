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
        Vulnerable,
    }

    public BossState bossState;
    public GameObject shield;

    //Attacking Variables;
    public Transform lightAttackPoint;
    public Transform heavyAttackPoint;
    public float lightAttackRange;
    public float heavyAttackRange;
    public float lightDamageTime;
    public float heavyDamageTime;
    public float lightAttackTime;
    public float heavyAttackTime;

    //Running and Charging Variables
    public float runningSpeed;
    public float chargingSpeed;
    public bool charging;
    public bool chargeRight;
    private float chargingTime;
    
    //Vulnerable Variables
    public bool makeVulnerable;
    public float vulnerableTimer;
    public float vulnerableTime = 5f;

    private bool lastPhase = false;

    //TextController References
    public GameObject TextController;
    public TextController TC;

    private void Start()
    {
        FindGameController();
        //Error Prevention
        if (GC != null)
        {
            changeHealth(1000 + GC.GetMultiplier() * 100);
            setHealth();
        }
        else
        {
            changeHealth(1000);
            setHealth();
        }

        type = "boss";
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        maxSpeed = runningSpeed;
        changeSpeed(runningSpeed);
        facingRight = false;
        player = GameObject.FindGameObjectWithTag("Player");
        bossState = BossState.Chase;
        pointsToGive = 100 + GC.GetMultiplier() * 50;
        chargingSpeed = chargingSpeed * (1+0.2f*GC.GetMultiplier());
        TextController = GameObject.FindGameObjectWithTag("TextController");
        TC = TextController.GetComponent<TextController>();
        shield.SetActive(false);
    }

    private void Update()
    {
        HealthController();
        TC.UpdateBoss(currentHealth *100/ maxHealth);
    }

    private void FixedUpdate()
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
        else if(player!=null)
        {
            if (!attacking)
                CheckDistToPlayer();
            Debug.Log(bossState);
            switch (bossState)
            {
                case BossState.Chase:
                    Chase(maxSpeed, CanSeeWall(wallDetection, wallCheckDistance, transform, wallLayer));
                    break;
                case BossState.Attack:
                    BossAttack();
                    break;
                case BossState.Charge:
                    Charge(CanSeeWall(wallDetection, wallCheckDistance, transform, wallLayer));
                    break;
                case BossState.Vulnerable:
                    MakeVulnerable();
                    break;
            }
        }
    }

    //Boss has a different health controller as it is in the UI
    public new void HealthController()
    {
        Vector3 temp = barForeground.transform.localScale;
        temp.x = (float)currentHealth / maxHealth;
        barForeground.transform.localScale = temp;
    }

    public void MakeVulnerable()
    {
        Debug.Log("Boss is vulnerable");
        shield.SetActive(false);
        invulnerable = false;
        animator.SetBool("lightAttack", false);
        animator.SetBool("heavyAttack", false);
        animator.SetBool("Walking", false);
        vulnerableTimer += Time.deltaTime;
        //When vulnerable time is reached, go back to whatever phase
        if(vulnerableTimer>=vulnerableTime)
        {
            Debug.Log("Boss is no longer vulnerable");
            makeVulnerable = false;
            vulnerableTimer = 0;
        }
    }

    public void Charge(bool wallInfo)
    {
        lastPhase = true;
        animator.SetBool("lightAttack", false);
        animator.SetBool("heavyAttack", false);
        chargingTime += Time.deltaTime;
        if (chargingTime < 1)
            return;
        shield.SetActive(true);
        //If not charging and player is to the right
        if (!charging && transform.position.x < player.transform.position.x)
        {
            //Become invulnerable and charge right
            invulnerable = true;
            chargeRight = true;
            charging = true;
        }
        //If not charging and player is to the left
        else if (!charging && transform.position.x > player.transform.position.x)
        {
            //Become invulnerable and charge left
            invulnerable = true;
            chargeRight = false;
            charging = true;
        }

        //If charging and charge right, go right until boss hits a wall
        if (charging && chargeRight)
        {
            Debug.Log("Boss is charging right");
            animator.SetBool("Walking", true);
            transform.eulerAngles = new Vector3(0, -180, 0);
            changeSpeed(Mathf.Abs(chargingSpeed));
            facingRight = true;
            rb.velocity = new Vector2(getSpeed(), rb.velocity.y);
            //Avoid a bug where he goes back to vulnerable when turning
            if (wallInfo && chargingTime>1.5)
            {
                Debug.Log("Boss hit a wall and is now vulnerable");
                FindObjectOfType<AudioManager>().Play("BossCrash");
                charging = false;
                makeVulnerable = true;
                chargingTime = 0;
            }
        }

        //If charging and charge left, go left until boss hits a wall
        if(charging &&!chargeRight)
        {
            Debug.Log("Boss is charging left");
            animator.SetBool("Walking", true);
            transform.eulerAngles = new Vector3(0, 0, 0);
            changeSpeed(-Mathf.Abs(chargingSpeed));
            facingRight = false;
            rb.velocity = new Vector2(getSpeed(), rb.velocity.y);
            //Avoid a bug where he goes back to vulnerable when turning
            if (wallInfo && chargingTime > 1.5)
            {
                Debug.Log("Boss hit a wall and is now vulnerable");
                FindObjectOfType<AudioManager>().Play("BossCrash");
                charging = false;
                makeVulnerable = true;
                chargingTime = 0;
            }
        }
    }

    public new static bool CanSeeWall(Transform wallDetection, float wallCheckDistance, Transform transform, LayerMask wallLayer)
    {
        bool wallInfo = Physics2D.Raycast(wallDetection.position, transform.right, -wallCheckDistance, wallLayer);
        return wallInfo;
    }
    
    public void BossAttack()
    {
        attacking = true;
        rb.velocity = Vector2.zero;
        if(!lastPhase)
            Attack("lightAttack", lightDamageTime, lightAttackTime, lightAttackPoint,1,0.5f);
        else
        {
            Attack("heavyAttack", heavyDamageTime, heavyAttackTime, heavyAttackPoint,2,1f);
            shield.SetActive(false);
        }
    }

    public new void CheckDistToPlayer()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
        playerDamaged = false;
        if (distToPlayer < attackRange)
            bossState = BossState.Attack;
        else
            bossState = BossState.Chase;
        //If health is between 70%-30%, charge
        if (currentHealth >= (maxHealth / 10)*3 && currentHealth <= (maxHealth/10)*7)
        {
            bossState = BossState.Charge;
            if (makeVulnerable)
                bossState = BossState.Vulnerable;
        }
    }

    public void Chase(float maxSpeed, bool wallInfo)
    {
        animator.SetBool("lightAttack", false);
        animator.SetBool("heavyAttack", false);
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
            else if (facingRight)
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
