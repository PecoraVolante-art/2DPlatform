using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private Animator anim;

    [Header("Movement")]
    public float MovementSpeed;
    private int FacingDirection;
    private bool FacingLeft;
    public float chaseSpeed = 15f;
    public float PatrolRange = 10f;
    public bool IsInRange = false;
    public float retrieveDistance = 2.5f;

    [Header("Stats")]
    public int MaxHealth;
    public int currentHealth;
    public float AttackRange = 0.5f;
    public int AttackDamage = 10;
    [SerializeField] private GameObject damageEffect;

    [Header("Combat Control")]
    public int hitsToHurt = 3;
    private int hitCounter = 0;
    private bool isAttacking = false;
    private bool isHurt = false;

    [Header("ColliderCheck")]
    [SerializeField] private float EnemyGroundCheckDistance;
    [SerializeField] private LayerMask EnemyWhatIsGround;
    [SerializeField] private LayerMask EnemyWhatIsWall;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform WallCheckDistance;
    [SerializeField] Transform player;
    [SerializeField] private Transform EnemyAttackPoint;
    [SerializeField] private float EnemywallCheckDistance;
    public LayerMask Layerplayer;

    [Header("HealthBar")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

  
    public enum EnemyType
    {
        Enemy1,
        Enemy2
    }
    public EnemyType enemyType;



    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;


        currentHealth = MaxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = MaxHealth;
            healthSlider.value = currentHealth;

            fill.color = gradient.Evaluate(1f);
        }

        FacingDirection = -1;
        FacingLeft = true;
    }

    private void Update()
    {
        if (isHurt) return;


        IsInRange = Vector2.Distance(transform.position, player.position) <= PatrolRange;

        if (IsInRange)
        {

            if (player.position.x > transform.position.x && FacingLeft)
                EnemyFlip();
            else if (player.position.x < transform.position.x && !FacingLeft)
                EnemyFlip();

            if (!isAttacking)
            {
                if (Vector2.Distance(transform.position, player.position) > retrieveDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
                }
                else
                {
                    StartAttack();
                }
            }
        }
        else
        {

            if (!isAttacking)
            {
                Patrol();
            }
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        anim.SetBool("Attack", true);
    }

    public void EndAttack()
    {
        isAttacking = false;
        anim.SetBool("Attack", false);
    }

    void Patrol()
    {
        transform.Translate(Vector2.right * FacingDirection * Time.deltaTime * MovementSpeed);

        RaycastHit2D Wallhit = Physics2D.Raycast(transform.position, Vector2.right * FacingDirection, EnemywallCheckDistance, EnemyWhatIsWall);
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, EnemyGroundCheckDistance, EnemyWhatIsGround);

        if (hit == false || Wallhit)
        {
            EnemyFlip();
        }
    }

    public void TakeDamage(int damage, int hitDirection)
    {
        if (GestioneSFX.Instance != null)
        {
            switch (enemyType)
            {
                case EnemyType.Enemy1:
                    GestioneSFX.Instance.PlayHurtEnemy1();
                    break;
            }

            switch (enemyType)
            {
                case EnemyType.Enemy2:
                    GestioneSFX.Instance.PlayHurtEnemy2();
                    break;
            }
        }

            currentHealth -= damage;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;

            float normalized = healthSlider.normalizedValue;
            fill.color = gradient.Evaluate(normalized);
        }

        hitCounter++;

        if (damageEffect != null)
        {
            GameObject fx = Instantiate(damageEffect, transform.position, Quaternion.identity);


            if (hitDirection < 0)
            {
                fx.transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        if (hitCounter >= hitsToHurt)
        {
            isHurt = true;

            isAttacking = false;
            anim.SetBool("Attack", false);

            anim.SetTrigger("Hurt");
            hitCounter = 0;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void EndHurt()
    {
        isHurt = false;
    }

    public void Die()
    {
        if (GestioneSFX.Instance != null)
        {
            switch (enemyType)
            {
                case EnemyType.Enemy1:
                    GestioneSFX.Instance.PlayDestroyEnemy1();
                    break;
            }

            switch (enemyType)
            {
                case EnemyType.Enemy2:
                    GestioneSFX.Instance.PlayDestroyEnemy2();
                    break;

            }
        }

                    anim.SetBool("IsDead", true);
        isAttacking = false;

        Physics2D.IgnoreLayerCollision(7, 8, true);

        this.enabled = false;


        Destroy(gameObject, 3f);
    }

 

    private void EnemyFlip()
    {
        FacingLeft = !FacingLeft;
        FacingDirection *= -1;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        if (healthSlider != null)
        {
            Vector3 sliderScale = healthSlider.transform.localScale;
            sliderScale.x *= -1;
            healthSlider.transform.localScale = sliderScale;
        }
    }

    void Attack()
    {
        if (GestioneSFX.Instance != null)
            GestioneSFX.Instance.PlayEnemyAttack();

        Collider2D collInfo = Physics2D.OverlapCircle(EnemyAttackPoint.position, AttackRange, Layerplayer);

        if (collInfo)
        {
            PlayerController player = collInfo.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(AttackDamage, FacingDirection);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * EnemyGroundCheckDistance);
        }

        Gizmos.DrawWireSphere(transform.position, PatrolRange);


        if (EnemyAttackPoint == null) return;
        Gizmos.DrawWireSphere(EnemyAttackPoint.position, AttackRange);

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (EnemywallCheckDistance * FacingDirection), transform.position.y));

    }

}



