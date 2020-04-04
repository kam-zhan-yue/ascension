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

    public float lightAttackRate = 2f;
    public float heavyAttackRate = 1.2f;
    private float nextAttackTime = 0f;
    private float jumpAttackTimer = 0f;
    private bool jumpAttack = false;

    int lightDamage = 30;
    int jumpDamage = 40;
    int heavyDamage = 70;
    public bool playerAttacking;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Reset Scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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

        if(Time.time >= nextAttackTime && !this.GetComponent<PlayerController>().damaged)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if(PlayerController.grounded)
                {
                    Attack("lightAttack", 1f, lightDamage, 2f);
                    nextAttackTime = Time.time + 1f / lightAttackRate;
                }
                else if(!PlayerController.doubleJump)
                {
                    Debug.Log("Jump Attack!");
                    jumpAttack = true;
                    animator.SetBool("jumpAttack", jumpAttack);
                    Attack("jumpAttack", 1f, jumpDamage,2f);
                    nextAttackTime = Time.time + 1f / lightAttackRate;
                }
                
            }
            else if (Input.GetKeyDown(KeyCode.K) && PlayerController.grounded)
            {
                Attack("heavyAttack", 1.4f,heavyDamage,4f);
                nextAttackTime = Time.time + 1f / heavyAttackRate;
            }
        }
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
