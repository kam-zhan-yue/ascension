using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    Animator animator;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayer;
    public GameObject shield;

    public float lightAttackRate = 2f;
    public float heavyAttackRate = 1.2f;
    private float nextAttackTime = 0f;
    private float jumpAttackTimer = 0f;
    public bool jumpAttack = false;
    public float blockingRate = 50f;
    public float blockingTime = 0.3f;
    public float blockingTimer = 0f;
    public bool playerInvulnerable;

    int lightDamage = 30;
    int jumpDamage = 40;
    int heavyDamage = 70;

    public PlayerController controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        shield.SetActive(false);
    }
    
    void Update()
    { 
        if (jumpAttack)
        {
            jumpAttackTimer += Time.deltaTime;
            if (jumpAttackTimer >= 0.25f)
            {
                jumpAttack = false;
                animator.SetBool("jumpAttack", jumpAttack);
                jumpAttackTimer = 0;
            }
        }
        if(playerInvulnerable)
        {
            //Zero out velocity
            controller.rb.velocity = Vector2.zero;
            blockingTimer += Time.deltaTime;
            if(blockingTimer >= blockingTime)
            {
                playerInvulnerable = false;
                shield.SetActive(false);
                blockingTimer = 0f;
                nextAttackTime = Time.time + 1f /blockingRate;
            }
        }

        if(Time.time >= nextAttackTime && !this.GetComponent<PlayerController>().damaged && !playerInvulnerable)
        {
            if(Input.GetKeyDown(KeyCode.L) && controller.grounded)
            {
                Block();
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                if(controller.grounded)
                {
                    FindObjectOfType<AudioManager>().Play("PlayerLight");
                    Attack("lightAttack", 1f, lightDamage, 2f);
                    nextAttackTime = Time.time + 1f / lightAttackRate;
                }
                else if(!controller.doubleJump)
                {
                    FindObjectOfType<AudioManager>().Play("PlayerLight");
                    Debug.Log("Jump Attack!");
                    jumpAttack = true;
                    animator.SetBool("jumpAttack", jumpAttack);
                    Attack("jumpAttack", 1f, jumpDamage,2f);
                    nextAttackTime = Time.time + 1f / lightAttackRate;
                }
                
            }
            else if (Input.GetKeyDown(KeyCode.K) && controller.grounded)
            {
                FindObjectOfType<AudioManager>().Play("PlayerHeavy");
                Attack("heavyAttack", 1.4f,heavyDamage,4f);
                nextAttackTime = Time.time + 1f / heavyAttackRate;
            }
        }
    }

    void Block()
    {
        playerInvulnerable = true;
        animator.SetTrigger("Block");
        shield.SetActive(true);
    }

    void Attack(string anim, float attackRange, int damage, float knockback)
    {
        //Play an attack animation
        if (anim != "jumpAttack")
            animator.SetTrigger(anim);

        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        //Damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            FindObjectOfType<HitStop>().Stop(0.05f);
            Debug.Log("We hit " + enemy.name);
            if (enemy.tag == "Skeleton")
                enemy.GetComponent<Skeleton>().TakeDamage(damage, knockback);
            else if (enemy.tag == "FlyingEye")
                enemy.GetComponent<FlyingEye>().TakeDamage(damage, knockback);
            else if (enemy.tag == "Boss")
                enemy.GetComponent<Boss>().TakeDamage(damage, knockback);
        }
    }

    void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
