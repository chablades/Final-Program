using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed;

    //reference rigidbody and animator
    private Rigidbody2D rb;
    private Animator anim;
    private bool grounded;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        //Hozontal Speed
        rb.linearVelocity = new Vector2(horizontalInput * runSpeed, rb.linearVelocity.y);
        
        //flip sprite
        if (horizontalInput > 0.01f)
            transform.localScale = Vector2.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector2(-1,1);

        //Jump
        if(Input.GetKey(KeyCode.Space) && grounded){
            Jump();
        }

        //Animation parameterss
        anim.SetBool("run", horizontalInput !=0);
        anim.SetBool("grounded", grounded);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, runSpeed);
        anim.SetTrigger("jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
            grounded = true;
    }
}
