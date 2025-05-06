using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private GameObject[] hearts;

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
        updateHealthUI();
    }

    public void TakeDamage(int damage, Rigidbody2D enemy)
    {
        currentLives -= damage;
        Debug.Log("Player took damage! Lives left: " + currentLives);
        updateHealthUI();
        // Play player damage sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlayerDamage();
        }
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

    private void updateHealthUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            {
                Animator heartAnim = hearts[i].GetComponent<Animator>();

                if (i < currentLives)
                {
                    hearts[i].SetActive(true);
                    if (heartAnim != null) heartAnim.Rebind();
                }
                else if (hearts[i].activeSelf)
                {
                    heartAnim.SetTrigger("Hit");
                    StartCoroutine(DisableAfterDelay(hearts[i], 1f));
                }
            }
        }
    }

    private IEnumerator DisableAfterDelay(GameObject heart, float delay)
    {
        yield return new WaitForSeconds(delay);
        heart.SetActive(false);
    }
}