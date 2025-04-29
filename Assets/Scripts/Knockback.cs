using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] public float knockbackTime = 0.2f;
    [SerializeField] public float hitDirectionForce = 10f;
    [SerializeField] public float consForce = 5f;
    [SerializeField] public float inputForce = 7.5f;

    private Rigidbody2D rb;
    private Coroutine knockbackCoroutine;
    public bool IsBeingKnockedBack {get; private set;}

    private void Start()
    {
           rb = GetComponent<Rigidbody2D>();
    }

    public IEnumerator KnockbackAction(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection){
        IsBeingKnockedBack = true;

        Vector2 hitForce;
        Vector2 constantForce;
        Vector2 knockbackForce;
        Vector2 combinedForce;

        hitForce = hitDirection * hitDirectionForce;
        constantForce = constantForceDirection * consForce;


        float elapsedTime = 0f;
        while(elapsedTime < knockbackTime){
            //iterate the timer
            elapsedTime += Time.fixedDeltaTime;

            //combine hitForce and constantForce
            knockbackForce = hitForce + constantForce;

            //combing knockBackForce with Input Force
            if(inputDirection != 0){
                combinedForce = knockbackForce + new Vector2(inputDirection * -inputForce, 0f);
            }
            else{
                combinedForce = knockbackForce;
            }

            //apply knockback
            rb.linearVelocity = combinedForce;
            
            yield return new WaitForFixedUpdate();

        }

        IsBeingKnockedBack = false;
    }

    public void CallKnockback(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection){
        knockbackCoroutine = StartCoroutine(KnockbackAction(hitDirection, constantForceDirection, inputDirection));
    }
    
}
