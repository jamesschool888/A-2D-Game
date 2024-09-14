using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class playerController : MonoBehaviour
{
    //Inspector mode
    public static float movementSpeed = 5.0f;

    //non-accessible
    public static float inputDirection;

    public Rigidbody2D rb;
    public Animator animator;

    public bool isRunning;
    private bool isFacingRight = true;

    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundCheckCircle;
    public bool isGrounded;

    public float jumpForce = 8.0f;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public int availableJumps = 1;
    private int availableJumpsLeft;

    private bool canJump;
    public bool isFalling = false;
    // Start is called before the first frame update

    public float knockBackForce;
    public float knockBackCounter;
    public float knockBackTotalTime;

    public bool knockFromRight;

    public Health player;

    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 10f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer tr;

    public float maxStamina = 1f;
    public float dashStaminaCost = .15f;
    public float staminaRegenRate = .10f;
    public FloatValueSO currentStamina;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        availableJumpsLeft = availableJumps;
        currentStamina.Value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentStamina.Value > 1)
        {
            currentStamina.Value = 1;
        }

        if (!isDashing && currentStamina.Value < maxStamina)
        {
            currentStamina.Value += staminaRegenRate * Time.deltaTime;
            currentStamina.Value = Mathf.Clamp(currentStamina.Value, 0f, maxStamina);
        }

        if (isDashing)
        {
            return;
        }

        checkInput();
        checkMovementDirection();
        updateAnimation();
        checkIfCanJump();

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * 
                (fallMultiplier - 1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * Time.deltaTime;
        }

        float currentVelocity = rb.velocity.y;
        if (isGrounded)
        {
            currentVelocity = 0f;
            isFalling = false;
        }
        if (currentVelocity < 0f)
        {
            isFalling = true;
        }
    }

    /*IEnumerator stunTimer()
    {
        if (isHit == true)
        {
            yield return new WaitForSeconds(0.3f);
            Debug.Log("Stun");
            isHit = false;
            if (isHit == false)
            {
                rb.velocity = new Vector2(movementSpeed * inputDirection, rb.velocity.y);
            }
        }
    }
    */

     void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        applyKnockback();
        checkEnvironment();
    }

    private void checkInput()
    {
        if(!player.isDead)
        {
            inputDirection = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            if (Input.GetButtonDown("Dash") && canDash && isRunning)
            {
                StartCoroutine(Dash());
            }
        }
    }

    private void Jump()
    {
        if (!player.isDead)
        {
            if (canJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                availableJumpsLeft--;
            }
        }

    }

    private void checkIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0)
        {
            availableJumpsLeft = availableJumps;
        }
        if(availableJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    public void applyKnockback()
    {
        if (!player.isDead)
        {
            if (knockBackCounter <= 0)
            {
                rb.velocity = new Vector2(movementSpeed * inputDirection, rb.velocity.y);
            }
            else
            {
                if (knockFromRight == true)
                {
                    rb.velocity = new Vector2(-knockBackForce, knockBackForce);

                }
                if (knockFromRight == false)
                {
                    rb.velocity = new Vector2(knockBackForce, knockBackForce);

                }
                knockBackCounter -= Time.deltaTime;
            }

            if (player.isDead == true)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    private void checkMovementDirection()
    {
        if(isFacingRight && inputDirection < 0 && !player.isDead)
            Flip();
        else if(!isFacingRight && inputDirection > 0 && !player.isDead)
            Flip();

        if (rb.velocity.x <= -0.5f | rb.velocity.x >= 0.5f)
            isRunning = true;
        else
            isRunning = false;
    }

    private void updateAnimation()
    {
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isDashing", isDashing);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void checkEnvironment()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckCircle, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckCircle);
    }

    private IEnumerator Dash()
    {
        if(canDash && currentStamina.Value >= dashStaminaCost)
        {
            if (currentStamina.Value >= dashStaminaCost)
            {
                currentStamina.Value -= dashStaminaCost;
            }
            else
            {
                currentStamina.Value = 0;
            }

            canDash = false;
            isDashing = true;
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(inputDirection * dashingPower, 0f);
            tr.emitting = true;
            GetComponent<CapsuleCollider2D>().enabled = false;
            yield return new WaitForSeconds(dashingTime);
            tr.emitting = false;
            rb.gravityScale = originalGravity;
            isDashing = false;
            GetComponent<CapsuleCollider2D>().enabled = true;
            yield return new WaitForSeconds(dashingCooldown);
            canDash = true;
        }
        else
            Debug.Log("Not enough stamina for dashing!");

    }
}

