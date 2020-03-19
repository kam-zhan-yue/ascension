using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;

    public float lightAttackRate = 2f;
    public float heavyAttackRate = 1.2f;
    private float nextAttackTime = 0f;
    private float jumpAttackTimer = 0f;
    private bool jumpAttack = false;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if(jumpAttack)
        {
            jumpAttackTimer += Time.deltaTime;
            if (jumpAttackTimer >= 0.25f)
            {
                jumpAttack = false;
                animator.SetBool("jumpAttack", jumpAttack);
                jumpAttackTimer = 0;
            }
        }

        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if(PlayerController.grounded)
                {
                    Attack("lightAttack", 0.3f);
                    nextAttackTime = Time.time + 1f / lightAttackRate;
                }
                else if(!PlayerController.doubleJump)
                {
                    Debug.Log("Jump Attack!");
                    jumpAttack = true;
                    animator.SetBool("jumpAttack", jumpAttack);
                    nextAttackTime = Time.time + 1f / lightAttackRate;
                }
                
            }
            else if (Input.GetKeyDown(KeyCode.K) && PlayerController.grounded)
            {
                Attack("heavyAttack", 0.6f);
                nextAttackTime = Time.time + 1f / heavyAttackRate;
            }
        }
    }

    void Attack(string anim, float attackRange)
    {
        //Play an attack animation
        animator.SetTrigger(anim);

        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        //Damage enemies
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit enemy");
        }
    }

    void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
