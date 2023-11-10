/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform popUp;
    public string type;
    public Animator animator;
    public Rigidbody2D rb;
    public bool dead;
    public DestroyMe destroyScript;
    public ParticleSystem attackPS;

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
    public float maxSpeed;
    private float speed;
    public int maxHealth;
    public int currentHealth;

    private int counter;
    public int choice;

    //Attacking Variables
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
    public bool invulnerable;

    //Health Bar Variables
    public Vector3 localScale;
    public GameObject bar;
    public GameObject barForeground;

    GameObject GameController;
    public GameController GC;

    public int pointsToGive = 0;


    //===============PROCEDURE===============//
    public void HealthController()
    //Purpose:          Adjusts the colour and size of the HP bar according to its value
    {
        //Error prevention
        if (bar != null)
        {
            bar.transform.position = new Vector3(transform.position.x, transform.position.y + 0.8f, -1);
        }
        Vector3 temp = barForeground.transform.localScale;
        temp.x = (float)currentHealth / maxHealth;
        barForeground.transform.localScale = temp;
    }

    //===============PROCEDURE===============//
    public void FindGameController()
    //Purpose:          To find the game controller game object and store it as an instance in the script
    {
        GameController = GameObject.FindGameObjectWithTag("GameController");
        GC = GameController.GetComponent<GameController>();
    }

    //===============PROCEDURE===============//
    public void changeHealth(int newHealth)
    //Purpose:          Changes the encapsulated variable of maxHealth
    {
        maxHealth = newHealth;
    }

    //===============PROCEDURE===============//
    public void setHealth()
    //Purpose:          Changes the encapsulated variable of currentHealth
    {
        currentHealth = maxHealth;
    }

    //===============PROCEDURE===============//
    public void changeSpeed(float newSpeed)
    //Purpose:          Changes the encapsulated variable of speed
    {
        speed = newSpeed;
    }

    //===============FUNCTION===============//
    public float getSpeed()
    //Purpose:          Accessor method for speed
    {
        return speed;
    }

    //===============FUNCTION===============//
    public int getHealth()
    //Purpose:          Accessor method for current health
    {
        return currentHealth;
    }

    //===============PROCEDURE===============//
    public void Attack(string attackAnim, float damageFrame, float attackTime, Transform attackPoint, int damage, float knockback)
    //Purpose:          Makes the enemy attack. The attack is registered on the damage frame for a few milliseconds
    {
        attacking = true;
        attackTimer += Time.deltaTime;
        //Debug.Log("Player spotted, attacking...");
        animator.SetBool("Walking", false);
        animator.SetBool(attackAnim, attacking);

        //If not damaged yet and at the attack frame, then damage
        if (!playerDamaged && attackTimer >= damageFrame && attackTimer <= damageFrame + 0.2f)
        {
            Debug.Log("Player is in Range");
            if (type == "skeleton")
                FindObjectOfType<AudioManager>().Play("SkeletonAttack");
            if (type == "boss")
                FindObjectOfType<AudioManager>().Play("BossAttack");
            Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
            if (hitPlayer != null)
            {
                //Damage the player
                player.GetComponent<PlayerController>().TakeDamage(transform, damage, knockback);
                Debug.Log("Player is hit!");
            }
            playerDamaged = true;
        }

        //If the attack animation is finished
        if (attackTimer >= attackTime)
        {
            Debug.Log("Reset attack timer");
            attacking = false;
            animator.SetBool(attackAnim, false);
            attackTimer = 0;
        }
    }

    //===============PROCEDURE===============//
    public void MakeAChoice()
    //Purpose:          Returns a variable of 1 or 2
    {
        choice = (int)Random.Range(1, 3);
    }

    //===============PROCEDURE===============//
    public void CheckDistToPlayer()
    //Purpose:          Finds distance to player and determines the enemy's behaviour
    {
        animator.SetBool("Attacking", attacking);
        playerDamaged = false;
        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
        //Debug.Log(distToPlayer);
        //Attack player if within range
        if (distToPlayer < attackRange)
            state = State.Attacking;
        //Chase player if within range, but not attacking range
        else if (distToPlayer < aggroRange && VerticalDistToPlayer() < 0.8f)
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

    //===============FUNCTION===============//
    public static bool CanSeeGround(Transform groundDetection, float groundCheckDistance)
    //Purpose:          Check whether the enemy can see the ground
    {
        bool groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, groundCheckDistance);
        return groundInfo;
    }

    //===============FUNCTION===============//
    public static bool CanSeeWall(Transform wallDetection, float wallCheckDistance, Transform transform, LayerMask wallLayer)
    //Purpose:          Check whether the enemy can see the wall
    {
        bool wallInfo = Physics2D.Raycast(wallDetection.position, transform.right, wallCheckDistance, wallLayer);
        return wallInfo;
    }

    //===============FUNCTION===============//
    public float VerticalDistToPlayer()
    //Purpose:          Calculates the vertical distance to the player in absolute terms
    {
        float distToPlayer = Mathf.Abs(transform.position.y - player.transform.position.y);
        return distToPlayer;
    }

    //===============PROCEDURE===============//
    public void Chase(float maxSpeed, bool groundInfo, bool wallInfo)
    //Purpose:          Makes the enemy chase the player. Flips when at a wall or at the edge of a platform
    {
        //If enemy is on the left of the player
        if (transform.position.x < player.transform.position.x && VerticalDistToPlayer() < 1.5f)
        {
            //If not at the edge or not at a wall, chase
            if (groundInfo && !wallInfo)
            {
                //Flip right and move right
                //Debug.Log("Player is to the right, move right");
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
                //Debug.Log("Player is to the left, move left");
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
                //Debug.Log("Player is to the right, move right");
                animator.SetBool("Walking", true);
                transform.eulerAngles = new Vector3(0, -180, 0);
                speed = -Mathf.Abs(maxSpeed);
                facingRight = false;
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else if (facingRight)
            {
                //Flip to the Left and move left 
                //Debug.Log("Player is to the left, move left");
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

    //===============PROCEDURE===============//
    public void Patrol(int choice, bool groundInfo, bool wallInfo)
    //Purpose:          Makes the enemy walk around looking for the player
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
                        //Debug.Log("Turn left!");
                        transform.eulerAngles = new Vector3(0, -180, 0);
                        speed *= -1;
                        facingRight = false;
                    }
                    else
                    {
                        //Debug.Log("Turn right!");
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

    //===============PROCEDURE===============//
    public void TakeDamage(int damage, float knockback)
    //Purpose:          Makes the enemy take damage and a certain amount of knockback
    {
        if (invulnerable)
            return;
        Vector3 pos = transform.position;
        pos += new Vector3(0, 0.2f);
        popUp.GetComponent<TextPopUp>().DamagePopUp(damage);
        Instantiate(popUp, pos, Quaternion.identity);
        StartCoroutine(FindObjectOfType<CameraControl>().Shake(.05f, .05f));
        FindObjectOfType<AudioManager>().Play("EnemyHit");
        //Error Prevention
        if (attackPS != null)
            AttackParticles();
        animator.SetBool("Walking", false);
        //Take damage
        currentHealth -= damage;
        //Apply knockback force to the left if player is on the right
        if (transform.position.x < player.transform.position.x)
        {
            if (!attacking)
            {
                selfDamaged = true;
                Debug.Log("Enemy - Add force to left");
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
                selfDamaged = true;
                Debug.Log("Enemy - Add force to right");
                rb.velocity = Vector2.zero;
                rb.velocity += new Vector2(knockback, rb.velocity.y);
            }
            else
                rb.velocity += new Vector2(2, rb.velocity.y);
        }

        //Check if the enemy is dead
        if (currentHealth <= 0)
            Die();
        //Play hurt animation
        else if (!attacking)
            animator.SetTrigger("Hurt");
    }

    //===============PROCEDURE===============//
    public void AttackParticles()
    //Purpose:          Instantiates attack particles
    {
        attackPS.Play();
        Debug.Log("Attack particles");
    }

    //===============PROCEDURE===============//
    public void Die()
    //Purpose:          Plays audio, destroys the game object, and deactivates the script
    {
        currentHealth = 0;
        HealthController();
        Vector3 pos = transform.position;
        pos += new Vector3(0, 0.7f);
        popUp.GetComponent<TextPopUp>().PointsPopUp(pointsToGive);
        Instantiate(popUp, pos, Quaternion.identity);
        if (type == "skeleton")
        {
            FindObjectOfType<AudioManager>().Play("SkeletonDeath");
            animator.SetBool("Walking", false);
        }
        if (type == "eye")
            FindObjectOfType<AudioManager>().Play("EyeDeath");
        if (type == "boss")
        {
            FindObjectOfType<AudioManager>().Play("BossDeath");
            FindObjectOfType<AudioManager>().Stop("Boss");
            FindObjectOfType<AudioManager>().Play("Victory");
            animator.SetBool("lightAttack", false);
            animator.SetBool("Walking", false);
            animator.SetBool("heavyAttack", false);
        }
        Debug.Log("Enemy died");
        animator.SetTrigger("Dead");
        animator.SetBool("Attacking", false);
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().isKinematic = true;
        dead = true;
        GC.AddPoints(pointsToGive);
        if (destroyScript != null)
            destroyScript.death = true;
        if (bar != null)
            bar.SetActive(false);
        this.enabled = false;
    }
}