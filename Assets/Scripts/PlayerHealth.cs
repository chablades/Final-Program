using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    private int currentLives;

    private Knockback knockback;
    private Rigidbody2D rb;
    private Animator anim;
    private void Start()
    {
        currentLives = maxLives;
        rb= GetComponent<Rigidbody2D>();
        knockback = GetComponent<Knockback>();
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage, Rigidbody2D enemy)
    {
        currentLives -= damage;
        Debug.Log("Player took damage! Lives left: " + currentLives);
       
        if (currentLives <= 0)
        {
            rb.bodyType = RigidbodyType2D.Static;
            anim.SetTrigger("death");
            Invoke("Die", 1f);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //letting player know they died
    }
}