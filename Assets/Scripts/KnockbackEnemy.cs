using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class KnockbackEnemy : MonoBehaviour
{
    [SerializeField] public float enemyKnockbackTime = 0.2f;
    [SerializeField] public float force = 1f;

    private Rigidbody2D rb;
    private Coroutine enemyKnockbackCoroutine;
    private float elapsedTime = 0f;
    public bool EnemyIsBeingKnockedBack {get; private set;}

    private void Start()
    {
           rb = GetComponent<Rigidbody2D>();
    }

    public IEnumerator EnemyKnockbackAction(Rigidbody2D playerLoc){
        EnemyIsBeingKnockedBack = true;

        Vector2 direction = (transform.position - playerLoc.transform.position).normalized;
            if (direction.x > 0){
                direction = new Vector2(1, 0);
            }
            else{
                direction = new Vector2(-1,0);
            }
        
        Debug.Log("enemy knockback" + direction);

        elapsedTime = 0f;
        while(elapsedTime < enemyKnockbackTime){
            //iterate the timer
            elapsedTime += Time.fixedDeltaTime;            
            // Apply the knockback force
            rb.AddForce(direction * force, ForceMode2D.Impulse);
            
            yield return new WaitForFixedUpdate();
        }
        EnemyIsBeingKnockedBack = false;
        Debug.Log("enemy knockback false");
        
    }

    public void EnemyCallKnockback(Rigidbody2D playerLoc){
        if (EnemyIsBeingKnockedBack == false){
            enemyKnockbackCoroutine = StartCoroutine(EnemyKnockbackAction(playerLoc));
        }
    }
    
}
