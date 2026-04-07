using UnityEngine;

public class Trap : MonoBehaviour
{
    public int trapDamage = 10;
    public float damageInterval = 1f; 

    private float lastDamageTime = 0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageInterval)
            {
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(trapDamage,0);
                lastDamageTime = Time.time;
                
            }
        }
    }
}