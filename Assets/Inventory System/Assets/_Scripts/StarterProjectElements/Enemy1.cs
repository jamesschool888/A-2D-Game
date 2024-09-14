using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy1: MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;

    [SerializeField] private Animator animator;

    [SerializeField] private string animationAttackTrigger = "Attack";

    [SerializeField] private float attackInterval = 2;

    [SerializeField] private ThrowableWeapon weaponPrefab;

    [SerializeField] private Transform throwPoint;

    [SerializeField] private AudioSource throwAudioSource;

    public static int damage = 1;
    public playerController playerMovement;

    private void Start()
    {
        StartCoroutine(PerformAttack());
        currentHealth = maxHealth;
    }

    private IEnumerator PerformAttack()
    {
        yield return new WaitForSeconds(attackInterval);
        animator.SetTrigger(animationAttackTrigger);
        ThrowWeapon();
    }

    private void ThrowWeapon()
    {
        ThrowableWeapon throwable = Instantiate(weaponPrefab.gameObject, throwPoint.position, Quaternion.identity)
            .GetComponent<ThrowableWeapon>();
        throwable.ThrowInDirection(transform.right * -1);
        throwAudioSource.Play();
        StartCoroutine(PerformAttack());
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        Debug.Log("Enemy died");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (collision.gameObject.tag == "Player")
        {
            playerMovement.knockBackCounter = playerMovement.knockBackTotalTime;
            if (collision.transform.position.x <= transform.position.x)
            {
             playerMovement.knockFromRight = true;
            }
            if (collision.transform.position.x > transform.position.x)
            {
             playerMovement.knockFromRight = false;
            }
           health.Reduce(damage);
        }  
        
    }
}


        
 
