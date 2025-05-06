using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class DashFoward : MonoBehaviour
{
    [SerializeField] public float dashtime = 0.2f;
    [SerializeField] public float dashdist = 1f;

    private Rigidbody2D rb;
    private Coroutine dashCoroutine;
    private Animator anim;
    private float elapsedTime = 0f;
    public bool dashing {get; private set;}

    private void Start()
    {
           rb = GetComponent<Rigidbody2D>();
           anim = GetComponent<Animator>();
    }

    public IEnumerator Dash(){
        dashing = true;

        elapsedTime = 0f;
        gameObject.layer = LayerMask.NameToLayer("TransparentFX");
        gameObject.name = "Invincible";
        while(elapsedTime < dashtime){
            //iterate the timer
            elapsedTime += Time.fixedDeltaTime;
            Vector2 direction = (transform.position).normalized;
            if (Input.GetAxisRaw("Horizontal") > 0.01f)
                direction = new Vector2(1, 0);
            else if (Input.GetAxisRaw("Horizontal") < -0.01f)
                direction = new Vector2(-1, 0);          
            // Apply the knockback force
            rb.AddForce(direction * dashdist, ForceMode2D.Impulse);
            Debug.Log(direction);  
            
            yield return new WaitForFixedUpdate();

        }
        gameObject.layer = LayerMask.NameToLayer("PlayerAttacked");
        gameObject.name = "Player";
        dashing = false;
        Debug.Log("Not Dashing");
        anim.SetBool("dashing", false);
    }


    public void CallDash(){
        rb.linearVelocity = Vector2.zero;
        Debug.Log("Dashing");
        anim.SetTrigger("dashing");
        anim.SetBool("run", false);
        anim.SetBool("grounded", false);
        dashCoroutine = StartCoroutine(Dash());
    }
    
}
