using UnityEngine;
using UnityEngine.XR;

public class EnemyBase : MonoBehaviour
{
    public float maxHealth = .50f;
    public float currentHealth;
    public bool isDead;
    public Enemy enemy;
    public playerCombat playerHit;
    [SerializeField] public enemyHealthBar healthBar;

    protected void Awake()
    {
        healthBar = GetComponentInChildren<enemyHealthBar>();
    }
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        healthBar.updateHealthBar(currentHealth, maxHealth);
        enemy.animator = GetComponent<Animator>(); // Initialize the animator reference
    }

    protected virtual void Die()
    {
        enemy.animator.SetBool("isDead", true); // Set isDead to true in the animator
        float delayBeforeDestroy = 2.25f;
        Destroy(gameObject, delayBeforeDestroy);
        Debug.Log("Enemy died");
    }

    public virtual void takeDamage(float damage)
    {
        enemy.animator.SetTrigger("isHurt"); // Trigger the "isHurt" animation
        currentHealth -= damage;

        healthBar.updateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0 && !isDead)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<enemyCombat>().enabled = false;
            isDead = true;
            Die();
            healthBar.gameObject.SetActive(false);
        }
    }
}