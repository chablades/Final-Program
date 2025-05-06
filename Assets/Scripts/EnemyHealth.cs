using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 2;
    private int currentHealth;
    private Rigidbody2D rb;
    private Animator anim;
    public bool isDamaged = false;
    private KnockbackEnemy knockbackEnemy;
    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        knockbackEnemy = GetComponent<KnockbackEnemy>();
        anim = GetComponent<Animator>();
    }

    public void EnemyTakeDamage(int amount, Rigidbody2D player)
    {   
        currentHealth -= amount;
        Debug.Log("Enemy took damage! Remaining: " + currentHealth);
        
        // Play enemy damage sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayEnemyDamage();
        }

        if (currentHealth <= 0)
        {
            rb.bodyType = RigidbodyType2D.Static;
            anim.SetBool("Moving", false);
            anim.SetBool("hasTarget", false);
            anim.SetTrigger("death");
            Die();
        }
        
        knockbackEnemy.EnemyCallKnockback(player);
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        
        // Play enemy death sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayEnemyDeath();
        }
        
        Destroy(gameObject, 0.5f); //letting you know when the enemy dies
    }
}
