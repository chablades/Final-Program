using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 2;
    private int currentHealth;
    private Knockback knockback;
    private Rigidbody2D rb;
    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<Knockback>();
    }

    public void TakeDamage(int amount, Vector2 hitDirection)
    {
        currentHealth -= amount;
        Debug.Log("Enemy took damage! Remaining: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        knockback.CallKnockback(hitDirection, Vector2.up, Input.GetAxisRaw("Horizontal"));
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject); // or play animation, etc.
    }
}
