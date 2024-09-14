using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase
{
    public int damage = 1;

    public bool flip;
    public float speed;
    public bool isRunning = false;

    public playerController playerMovement;
    public Transform originalPosition;

    public float detectionRange;
    public float stoppingDistance = 0.5f;
    public float stoppingDistanceToPlayer = 2;

    public Transform player;
    public Animator animator;

    [SerializeField]
    private bool isCooldown = false;
    public float cooldownDuration = 0.43f;
    private float cooldownTimer = 0;

    public float knockbackForce = 10;
    public float knockbackDuration = 0.5f;
    private float knockbackTimer = 0;
    
    public bool inCombat = false;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        enemyMovement();
        updateAnimation();
    }

    void enemyMovement()
    {
        if (isCooldown)
        {
            if (knockbackTimer > 0)
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                Vector2 knockbackDirection = (transform.position - player.position).normalized;
                rb.velocity = knockbackDirection * knockbackForce;
                knockbackTimer -= Time.deltaTime;
                if (knockbackTimer <= 0)
                    rb.velocity = Vector2.zero;
                else
                    return;
            }

            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
                isCooldown = false;
            else
                return;
        }

       

        Vector3 scale = transform.localScale;
        float distance = Mathf.Abs(player.transform.position.x - transform.position.x);
        float distanceOriginalPos = Mathf.Abs(originalPosition.transform.position.x - transform.position.x);
        EnemyBase enemy = GetComponent<EnemyBase>();

        if (!enemy.isDead){
            if (Vector3.Distance(player.position, transform.position) <= detectionRange)
            {
                if (distance > stoppingDistanceToPlayer)
                {
                    if (player.transform.position.x > transform.position.x)
                    {
                        scale.x = Mathf.Abs(scale.x) * -1 * (flip ? -1 : 1);
                        transform.Translate(speed * Time.deltaTime, 0, 0);
                        isRunning = true;
                    }

                    else
                    {
                        scale.x = Mathf.Abs(scale.x) * (flip ? -1 : 1);
                        transform.Translate(speed * Time.deltaTime * -1, 0, 0);
                        isRunning = true;
                    }
                }
                else
                {
                    isRunning = false;
                    inCombat = true;
                }

            }
            else
            {
                if (distanceOriginalPos > stoppingDistance)
                {
                    if (originalPosition.transform.position.x > transform.position.x)
                    {
                        scale.x = Mathf.Abs(scale.x) * -1 * (flip ? -1 : 1);
                        transform.Translate(speed * Time.deltaTime, 0, 0);
                        isRunning = true;
                    }

                    else
                    {
                        scale.x = Mathf.Abs(scale.x) * (flip ? -1 : 1);
                        transform.Translate(speed * Time.deltaTime * -1, 0, 0);
                        isRunning = true;
                    }
                }

                else
                {
                    isRunning = false;
                    inCombat = false;
                }
            }

            transform.localScale = scale;
        }
        else
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector3.zero;
        }
            
    }

    private void updateAnimation()
    {
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("inCombat", inCombat);
    }
    public override void takeDamage(float damage)
    {
        healthBar.updateHealthBar(currentHealth, maxHealth);

        base.takeDamage(damage);
        knockbackTimer = knockbackDuration;

        cooldownTimer = cooldownDuration;
        isCooldown = true;
    }
}
