using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DashFoward : MonoBehaviour
{
    [SerializeField] public float dashtime = 0.2f;
    [SerializeField] public float dashdist = 1f;

    private Rigidbody2D rb;
    private Coroutine knockbackCoroutine;
    private Animator anim;
    public bool dashing {get; private set;}

    private void Start()
    {
           rb = GetComponent<Rigidbody2D>();
           anim = GetComponent<Animator>();
    }

    public IEnumerator Dash(){
        dashing = true;

        float elapsedTime = 0f;
        while(elapsedTime < dashtime){
            //iterate the timer
            elapsedTime += Time.fixedDeltaTime;
            Vector2 direction = (transform.position).normalized;
            if (Input.GetAxisRaw("Horizontal") > 0.01f)
                direction = new Vector2(1, 0);
            else if (Input.GetAxisRaw("Horizontal") < -0.01f)
                direction = new Vector2(-1, 0);

            Debug.Log(direction);            
            // Apply the knockback force
            rb.AddForce(direction * dashdist, ForceMode2D.Impulse);
            
            yield return new WaitForFixedUpdate();

        }
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
        knockbackCoroutine = StartCoroutine(Dash());
    }
    
}
