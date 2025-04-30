using System;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    private int currentLives;

    private Knockback knockback;
    private void Start()
    {
        currentLives = maxLives;
        //UIManager.Instance.UpdateLives(currentLives); // Optional UI

        knockback = GetComponent<Knockback>();
    }

    public void TakeDamage(int damage, Rigidbody2D enemy)
    {
        currentLives -= damage;
        //UIManager.Instance.UpdateLives(currentLives); // Optional UI
        Debug.Log("Player took damage! Lives left: " + currentLives);
       
        if (currentLives <= 0)
        {
            Die();
        }
        
         //Vector2 direction = (transform.position - other.transform.position).normalized;
         //direction = new Vector2(-5, 0);
         //Debug.Log(direction);
        
        // Apply the knockback force
        //GetComponent<Rigidbody2D>().AddForce(direction * 10, ForceMode2D.Impulse);
        knockback.CallKnockback(enemy);
        
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart
    }
}