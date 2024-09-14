using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class playerCombat : MonoBehaviour
{
    public int CountAttackClick;
    public Animator animator;

    public Transform attackPoint;
    public Transform attackPoint2;
    public float attackRange = .5f;
    public float attackRange2 = .5f;
    public LayerMask enemyLayers;
    public float baseAttack = .10f;
    
    public float cooldownTime = 0.6f;
    float cooldownUntilPress;
    public playerController player;
    public Health health;

    public float critChance = 0.01f;
    public float critDamageMultiplier = .5f;
    public bool isCritical;
    public float attackStaminaCost = .20f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && player.isRunning == false && cooldownUntilPress < Time.time && player.isGrounded == true && !health.isDead)
        {
            if (player.currentStamina.Value >= attackStaminaCost)
            {
                player.currentStamina.Value -= attackStaminaCost;
                cooldownUntilPress = Time.time + cooldownTime;
                buttonAttack();
            }
            else
            {
                Debug.Log("Not enough stamina to attack!");
            }
        }
}

    public void Attack()
    {
     float attackDamage = baseAttack / 2;
        animator.SetTrigger("Attack");

        isCritical = UnityEngine.Random.value < critChance;
        if (isCritical)
        {
            float critMultiplier = 1 + critDamageMultiplier;
            attackDamage = attackDamage * critMultiplier;
            Debug.Log("Critical Hit!");
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Enemy hit!");
            enemy.GetComponent<Enemy>().takeDamage(attackDamage);
            damagePopup.Create(enemy.transform.position, attackDamage, isCritical);
        }
    }

    void OnDrawGizmos()
    {
        if (attackPoint == null) 
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        if (attackPoint2 == null)
            return;
        Gizmos.DrawWireSphere(attackPoint2.position, attackRange2);
    }

    public void Attack2()
    {
        float attackDamage = baseAttack / 2;
        float attackDamage2 = attackDamage / 2;

        isCritical = UnityEngine.Random.value < critChance;
        if (isCritical)
        {
            float critMultiplier = 1 + critDamageMultiplier;
            attackDamage2 = attackDamage2 * critMultiplier;
            Debug.Log("Critical Hit!");

        }
        animator.SetTrigger("Attack2");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint2.position, attackRange2, enemyLayers);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Enemy hit!");
            enemy.GetComponent<Enemy>().takeDamage(attackDamage2);
            damagePopup.Create(enemy.transform.position, attackDamage2, isCritical);
        }

    }

    private void buttonAttack()
    {
        CountAttackClick++;
        if(CountAttackClick == 1)
        {
            Attack();
        }

        else if(CountAttackClick == 2)
        {
            Attack2();
            CountAttackClick = 0;
        }

    }
}
