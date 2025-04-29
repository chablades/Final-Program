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

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        currentLives -= damage;
        //UIManager.Instance.UpdateLives(currentLives); // Optional UI
        Debug.Log("Player took damage! Lives left: " + currentLives);
       
        if (currentLives <= 0)
        {
            Die();
        }

        knockback.CallKnockback(hitDirection, Vector2.up, Input.GetAxisRaw("Horizontal"));
        
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart
    }
}