using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Health : MonoBehaviour
{
    [SerializeField] public float maxHealth = 10;
    [SerializeField] public FloatValueSO currentHealth;

    [SerializeField] private GameObject bloodParticle;

    [SerializeField] private Renderer renderer;
    [SerializeField] private float flashTime = 0.2f;
    public Animator deathAnimation;
    public Animator hurtAnimation;
    public bool isDead;
 
    public playerController player;
    public playerController playerMovement;

    public int baseDefense;
    public int baseAgility;

    public gameManagerScript gameManager;
    public Transform respawnPoint;

    void Update()
    {
  
    }
    
    private void Start()
    {
        currentHealth.Value = 1;
        deathAnimation = GetComponent<Animator>();
        hurtAnimation = GetComponent<Animator>();

        isDead = false;
    }

   
    public void Reduce(int damage)
    {
        if (!isDead)
        {
            playerMovement.knockBackCounter = playerMovement.knockBackTotalTime;

            float defenseMultiplier = 0.01f;
            float agilityMultiplier = 0.01f;

            float actualDamage = damage - (baseDefense * defenseMultiplier);
            actualDamage = Mathf.Max(0, actualDamage);

            float dodgeChance = baseAgility * agilityMultiplier;
            float randomValue = Random.value;

            if (randomValue < dodgeChance)
            {
                Debug.Log("Attack Dodged");
                return;
            }

            currentHealth.Value -= actualDamage / maxHealth;

            CreateHitFeedback();

            if (currentHealth.Value >= 0 && !isDead)
            {
                hurtAnimation.Play("Hurt");
            }
            if (currentHealth.Value <= 0 && !isDead)
            {
                Die();
                isDead = true;
            }
            Debug.Log(currentHealth.Value);
        }
    }
 
    public void addHealth(int healthBoost)
    {
        int health = Mathf.RoundToInt(currentHealth.Value * maxHealth);
        int val = health + healthBoost;
        currentHealth.Value = (val > maxHealth ? maxHealth : val / maxHealth);
        if (currentHealth.Value > 1)
            currentHealth.Value = 1;
        Debug.Log(currentHealth.Value);
    }
 

    public void CreateHitFeedback()
    {
        Instantiate(bloodParticle, transform.position, Quaternion.identity);
        StartCoroutine(FlashFeedback());
    }

    private IEnumerator FlashFeedback()
    {
        renderer.material.SetInt("_Flash", 1);
        yield return new WaitForSeconds(flashTime);
        renderer.material.SetInt("_Flash", 0);
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true; // Set isDead to true here to prevent subsequent calls
            Debug.Log("Died");
            if (deathAnimation != null)
            {
                deathAnimation.SetTrigger("Dead");
            }
           // GetComponent<Rigidbody2D>().simulated = false;
            StartCoroutine(deathDelay());
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

    }

    public void Respawn()
    {
        isDead = false;
        currentHealth.Value = 1;
        player.currentStamina.Value = 1;
       /// GetComponent<Rigidbody2D>().simulated = true;
        if (player.animator != null)
        {
            player.animator.Play("Idle");
        }
        transform.position = respawnPoint.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (collision.gameObject.tag == "Projectiles" || collision.gameObject.tag == "Enemy")
        {
            playerMovement.knockBackCounter = playerMovement.knockBackTotalTime;
            if (collision.transform.position.x <= transform.position.x)
            {
                playerMovement.knockFromRight = false;
            }
            if (collision.transform.position.x > transform.position.x)
            {
                playerMovement.knockFromRight = true;
            }
        }
    }

    IEnumerator deathDelay()
    {
        yield return new WaitForSeconds(2f);
        gameManager.gameOver();
    }

}

