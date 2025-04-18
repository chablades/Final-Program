using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 1;
    //reference rigidbody and animator
    private Rigidbody2D rb;
    private Animator anim;
    private bool grounded;
    private bool isAttacking = false;

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
    // Horizontal Speed
    rb.linearVelocity = new Vector2(horizontalInput * runSpeed, rb.linearVelocity.y);
    
    // Flip sprite
    if (horizontalInput > 0.01f)
        transform.localScale = Vector2.one;
    else if (horizontalInput < -0.01f)
        transform.localScale = new Vector2(-1, 1);

    // Jump
    if (Input.GetKey(KeyCode.Space) && grounded)
    {
        Jump();
    }

    // Attack on 'F' key press
    if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
    {
        Attack();
        Debug.Log("Attacking!"); // Prints to the console when attacking
    }

    // Animation parameters
    anim.SetBool("run", horizontalInput != 0);
    anim.SetBool("grounded", grounded);
}


    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, runSpeed);
        anim.SetTrigger("jump");
        grounded = false;
    }

     private void Attack()
    {
        isAttacking = true;
        //anim.SetTrigger("attack"); // Trigger attack animation

        // Detect enemies in range and deal damage
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in hitEnemies)
        {
            // You can add the method to deal damage here
            Debug.Log("Hitting enemy: " + enemy.name);
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        }

        // Reset attack flag after animation (assuming attack duration is 0.5 seconds)
        Invoke("ResetAttack", 0.5f);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // Show attack range in the scene view
    }
}
