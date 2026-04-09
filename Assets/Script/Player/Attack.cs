using UnityEngine;

public class AttackComand : MonoBehaviour
{
    private Animator anim;


    private int facingDirection = 1;
    public Transform AttackPoint;
    public float AttackRange = 0.5f;
    public LayerMask EnemyLayer;
    public int AttackDamage = 20;

    public float AttackRate = 2f;
    float NextAttackTime = 0;



    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        if (Time.time >= NextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Attack();
                NextAttackTime = Time.time + 1f / AttackRate;
            }
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");

        if (GestioneSFX.Instance != null)
            GestioneSFX.Instance.PlaySFX(GestioneSFX.Instance.attack);

        facingDirection = transform.localScale.x > 0 ? 1 : -1;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {

            Debug.Log("We hit " + enemy.name);

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.TakeDamage(AttackDamage, facingDirection);

        }

    }

    private void OnDrawGizmos()
    {
        if (AttackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}
