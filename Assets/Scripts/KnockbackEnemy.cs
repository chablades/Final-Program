using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class KnockbackEnemy : MonoBehaviour
{
    [SerializeField] public float knockbackTime = 0.2f;
    [SerializeField] public float force = 1f;

    private Rigidbody2D rb;
    private Coroutine knockbackCoroutine;
    public bool IsBeingKnockedBack {get; private set;}

    private void Start()
    {
           rb = GetComponent<Rigidbody2D>();
    }

    public IEnumerator KnockbackAction(Rigidbody2D playerLoc){
        IsBeingKnockedBack = true;

        float elapsedTime = 0f;
        while(elapsedTime < knockbackTime){
            //iterate the timer
            elapsedTime += Time.fixedDeltaTime;
            Vector2 direction = (transform.position - playerLoc.transform.position).normalized;

            if (direction.x > 0){
                direction = new Vector2(force, 0);
            }
            else{
                direction = new Vector2(-force,0);
            }
            Debug.Log(direction);            
            // Apply the knockback force
            rb.AddForce(direction, ForceMode2D.Impulse);
            
            yield return new WaitForFixedUpdate();
        }
        IsBeingKnockedBack = false;
        
    }

    public void CallKnockback(Rigidbody2D playerLoc){
        rb.linearVelocity = Vector2.zero;
        knockbackCoroutine = StartCoroutine(KnockbackAction(playerLoc));
    }
    
}
