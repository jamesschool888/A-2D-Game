using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ThrowableWeapon : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;
    
    [SerializeField] private float speed = 2;
    [SerializeField] private float selfDestructionDistance = 4;
    [SerializeField] private int damage = 1;
    public playerController playerMovement;
    public void ThrowInDirection(Vector3 direction)
    {
        rb2d.velocity = direction * speed;

        StartCoroutine(DestroyAfterDistance());
    }

    private IEnumerator DestroyAfterDistance()
    {
        yield return new WaitForSeconds(selfDestructionDistance);
        DestroyThrowable();
    }

    private void DestroyThrowable()
    {
        StopAllCoroutines();
        Destroy(gameObject);
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
                DestroyThrowable();
            }
            if (collision.transform.position.x > transform.position.x)
            {
                playerMovement.knockFromRight = false;
                DestroyThrowable();
            }
            health.Reduce(damage);
        }

    }
}
