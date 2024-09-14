using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyCombat : MonoBehaviour
{
    public int baseAttack = 10;
    public float attackCooldown = 2.0f; // Time between enemy attacks
    private float nextAttackTime = 0.0f;

    public Transform EnemyAttackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;

    private Animator animator;
    private Rigidbody2D rb;

    public GameObject attackPoint;

    public bool isAttacking = false;
    public Transform player;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); 
    }

    private void Update()
    {
        Enemy enemy = GetComponent<Enemy>();
        if (Vector3.Distance(player.position, transform.position) <= enemy.detectionRange)
        {
            Debug.DrawRay(transform.position, (player.position - transform.position), Color.red);
        }

        if (Vector3.Distance(player.position, transform.position) <= Vector3.Distance(EnemyAttackPoint.position, transform.position))
        {
            if (!isAttacking && rb.velocity.sqrMagnitude < 0.001f)
            {
                if (Time.time >= nextAttackTime)
                {
                    AttackPlayer();
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }
    }

    private void AttackPlayer()
    {
        isAttacking = true;
        animator.SetTrigger("Attack1");
        StartCoroutine(AttackDelay());
    }

    //PAGOD NA AKO

    private IEnumerator AttackDelay()
    {
        // Wait for a delay before dealing damage
        float delayDuration = .3f;
        yield return new WaitForSeconds(delayDuration);

        // After the delay, find and damage the player
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(EnemyAttackPoint.position, attackRange, playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
            // Deal damage to the player
            player.GetComponent<Health>().Reduce(baseAttack);
            playerController playerMovement = player.GetComponent<playerController>();

            // Determine knockback direction based on relative positions
            Vector3 attackPointPosition = attackPoint.transform.position;

            if (transform.position.x <= attackPointPosition.x)
            {
                playerMovement.knockFromRight = false;
            }
            else
            {
                playerMovement.knockFromRight = true;
            }

            // Apply knockback to the player
            player.GetComponent<playerController>().applyKnockback();
        }

        // Wait for a short time before resetting the attack flag
        float attackDuration = 0.5f;
        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
    }

    // Draw the attack range for debugging purposes
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(EnemyAttackPoint.position, attackRange);
        }
    }
}
