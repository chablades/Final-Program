using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] public float knockbackTime = 0.2f;
    [SerializeField] public float force = 1f;

    private Rigidbody2D rb;
    private Animator anim;
    private Coroutine knockbackCoroutine;
    private float elapsedTime = 0f;
    public bool IsBeingKnockedBack {get; private set;}

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public IEnumerator KnockbackAction(Rigidbody2D enemyLoc){
        IsBeingKnockedBack = true;

        Vector2 direction = (transform.position - enemyLoc.transform.position).normalized;

            if (direction.x > 0){
                direction = new Vector2(1, 0);
            }
            else{
                direction = new Vector2(-1,0);
            }
        
        Debug.Log("knockback "+direction);  

        elapsedTime = 0f;
        while(elapsedTime < knockbackTime){
            //iterate the timer
            elapsedTime += Time.fixedDeltaTime;          
            // Apply the knockback force
            rb.AddForce(direction * force, ForceMode2D.Impulse);
            
            yield return new WaitForFixedUpdate();

        }
        IsBeingKnockedBack = false;
        Debug.Log("knockback falsed");
    }

    public void CallKnockback(Rigidbody2D enemyLoc){
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("run", false);
        anim.SetBool("grounded", false);
        if (IsBeingKnockedBack == false){
            knockbackCoroutine = StartCoroutine(KnockbackAction(enemyLoc));
        }
    }
    
}
