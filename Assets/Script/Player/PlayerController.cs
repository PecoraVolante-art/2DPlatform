using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;

    [Header("Movement")]
    public bool isRunning;
    [SerializeField] private float Movespeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallSlideSpeed = 2f;
    private bool FacingRight = true;
    private int FacingDirection;
    public bool CanDoubleJump;
    public float doubleJumpForce;
    private float xImput;


    [Header("Dash")]
    [SerializeField] private bool canDash;
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCoolDolwn;
    TrailRenderer dashTrailRenderer;


    [Header("Ground Interaction")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform groundCheck;

    [Header("Wall interaction")]
    [SerializeField] private bool isWallDetected;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private float wallCheckDistance;

    [SerializeField] private float WallJumpDuration;
    [SerializeField] private Vector2 WallJumpForce;
    [SerializeField] private float wallJumpDuration = 0.2f;
    public bool isWallJumping;
    public bool isWallSliding;
    public float wallSlidingSpeed;
    private float wallJumpDirection;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    private Vector2 wallJumpPower = new Vector2(8f, 16f);

    [Header("Stats")]
    public int MaxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    [SerializeField] private GameObject damageEffect;
    public UIManager uiManager;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        // Trova il UIManager se non assegnato
        if (uiManager == null)
        {
            uiManager = UnityEngine.Object.FindAnyObjectByType<UIManager>();
            if (uiManager == null)
            {
                Debug.LogWarning("UIManager non trovato! Controlla che esista in scena.");
            }
        }
    }

    void Start()
    {
        dashTrailRenderer = GetComponent<TrailRenderer>();
        FacingDirection = 1;
        currentHealth = MaxHealth;
        uiManager.SetMaxHealth(MaxHealth);
        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (isDashing)
        {
            return;
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, wallCheckDistance, whatIsWall);

        xImput = Input.GetAxis("Horizontal");

        if (isGrounded)
        {
            CanDoubleJump = true;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump(jumpForce);
            }
            else if (CanDoubleJump)
            {
                Jump(doubleJumpForce);
                CanDoubleJump = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.TogglePause();
        }

        if (Time.timeScale == 0f)
            return;

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        float moveHorizontal = Input.GetAxisRaw("Horizontal");


        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && canDash == true)
        {
            StartCoroutine(Dash());
        }

        Wallslide();
        WallJump();

        HandleAnimation();

        if (!isWallJumping)
        {
            Flip();
        }

    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(xImput * Movespeed, rb.linearVelocity.y);
        }

        if (isDashing)
        {
            return;
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * FacingDirection * wallCheckDistance);
        }
    }

    void HandleAnimation()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallSlide", isWallDetected);


    }

    private void Flip()
    {
        if (FacingRight && xImput < 0f || !FacingRight && xImput > 0f)
        {
            FacingRight = !FacingRight;
            FacingDirection *= -1;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        }
    }


    private IEnumerator Dash()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true);

        anim.SetTrigger("Dash");
        if (GestioneSFX.Instance != null)
            GestioneSFX.Instance.PlayDash();

        canDash = false;
        isDashing = true;

        dashTrailRenderer.emitting = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0f);

        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = originalGravity;
        isDashing = false;

        dashTrailRenderer.emitting = false;
        Physics2D.IgnoreLayerCollision(7, 8, false);

        yield return new WaitForSeconds(dashCoolDolwn);

        canDash = true;

        Debug.Log("Dash di nuovo disponibile");
    }

    private void Wallslide()
    {
        if (isWallDetected && !isGrounded && xImput != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Math.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -FacingDirection;
            wallJumpCounter = wallJumpTime;
            CancelInvoke(nameof(StopWallJump));
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && wallJumpCounter > 0f)
        {
            if (GestioneSFX.Instance != null)
                GestioneSFX.Instance.PlayJump();

            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpCounter = 0f;

            if ((FacingRight && wallJumpDirection < 0) || (!FacingRight && wallJumpDirection > 0))
            {
                Flip();
            }

            Invoke(nameof(StopWallJump), wallJumpDuration);
        }


    }

    private void StopWallJump()
    {
        isWallJumping = false;
    }

    private void Jump(float force)
    {
        if (GestioneSFX.Instance != null)
            GestioneSFX.Instance.PlayJump();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
    }


    public void TakeDamage(int damage, int hitDirection)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (GestioneSFX.Instance != null)
            GestioneSFX.Instance.PlayPlayerHurt();

        uiManager.SetHealth(currentHealth);

        if (damageEffect != null)
        {
            GameObject fx = Instantiate(damageEffect, transform.position, Quaternion.identity);


            if (hitDirection < 0)
            {
                fx.transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }


    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("È morto");

        anim.SetBool("IsDead", true);

        if (GestioneSFX.Instance != null)
            GestioneSFX.Instance.PlayDeath();

        rb.linearVelocity = Vector2.zero;

        this.enabled = false;

        StartCoroutine(GameOverDelay());
    }

    IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(2f);
        uiManager.GameOver();
        if (GestoreMusica.Instance != null)
        {
            GestoreMusica.Instance.PlayMusicaGameOver();
        }
    }

    public void ResetPlayer()
    {
        currentHealth = MaxHealth;
        uiManager.SetMaxHealth(MaxHealth);
        isDead = false;

        anim.SetBool("IsDead", false);
        anim.Rebind();
        anim.Update(0f);

        this.enabled = true;
    }
}
