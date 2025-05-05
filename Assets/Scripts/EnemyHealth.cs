using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 2;
    private int currentHealth;
    private Rigidbody2D rb;
    public bool isDamaged = false;
    private KnockbackEnemy knockbackEnemy;
    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        knockbackEnemy = GetComponent<KnockbackEnemy>();
    }

    public void TakeDamage(int amount, Rigidbody2D player)
    {   
        currentHealth -= amount;
        Debug.Log("Enemy took damage! Remaining: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        
        knockbackEnemy.EnemyCallKnockback(player);
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject); //letting you know when the enemy dies
    }
}
