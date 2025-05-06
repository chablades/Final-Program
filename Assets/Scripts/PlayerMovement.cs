using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float dashCooldown = 5f;

    //reference rigidbody and animator
    private Rigidbody2D rb;
    private Animator anim;
    private bool grounded;
    private bool isAttacking = false;
    private int attackcounter = 0;
    private float dashtimer = 100;
    private float screenHeight, screenWidth;
    private RaycastHit2D hitEnemies;
    private Knockback knockback;
    private DashFoward dash;
    private new Camera camera;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
        dash = GetComponent<DashFoward>();
        camera = Camera.main;
        screenHeight = camera.orthographicSize;
        screenWidth = screenHeight * camera.aspect;
    }
    void Start()
    {
        
    }

    private void Update()
    {
    if (knockback.IsBeingKnockedBack ==false && isAttacking == false && dash.dashing == false)
        //Attack on 'F' key press
        if (Input.GetMouseButton(0) && isAttacking == false)
        {
            rb.linearVelocity = Vector2.zero;
            Invoke("Attack", 0.1f);
            //Debug.Log("Attacking!"); //prints to the console when attacking
        }
    }

    private void FixedUpdate()
    {
    if (knockback.IsBeingKnockedBack ==false && isAttacking == false && dash.dashing == false)
        Move();
        //Dash with cooldown of aprox 5s
        if (Input.GetMouseButton(1) && dashtimer > dashCooldown) //right click
        {
            Dash();
        }

        //jump
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            Jump();
        }

    // Animation parameters
    anim.SetBool("grounded", grounded);
    dashtimer += Time.fixedDeltaTime;
    }


    private void Move()
    {
        // Horizontal Speed
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(horizontalInput * runSpeed, rb.linearVelocity.y);
        // Flip sprite
        if (horizontalInput > 0.01f)
            transform.localScale = Vector2.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector2(-1, 1);

        anim.SetBool("run", horizontalInput != 0);
    }
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, runSpeed);
        anim.SetTrigger("jump");
        grounded = false;
        attackcounter = 0;
    }

    private void Dash()
    {
        Debug.Log("dash");
        dash.CallDash();
        dashtimer =0f;
    }

     private void Attack()
    {
        if(isAttacking == false){
            rb.linearVelocity = Vector2.zero;
            isAttacking = true;
            if (attackcounter == 0){
                anim.SetTrigger("attacking0"); // Trigger attack animation
                attackcounter +=1;
            }
            else if (attackcounter == 1){
                anim.SetTrigger("attacking1"); // Trigger attack animation
                attackcounter +=1;
            }
            else if (attackcounter == 2){
                anim.SetTrigger("attacking2"); // Trigger attack animation
                attackcounter = 0;
            }

            // Detect enemies in range and deal damage
            hitEnemies = Physics2D.CircleCast(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);
            EnemyHealth enemyHealth = hitEnemies.collider.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(attackDamage, rb);
            //resetting attack flag
            Invoke("ResetAttack", 0.8f);
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
        anim.SetBool("attacking0", false);
        anim.SetBool("attacking1", false);
        anim.SetBool("attacking2", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransform.position, attackRange); //showing attack range in scene view
    }
}
