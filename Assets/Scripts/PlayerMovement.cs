using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private Transform attackTransform;
    //reference rigidbody and animator
    private Rigidbody2D rb;
    private Animator anim;
    private bool grounded;
    private bool knockedback;
    private bool isAttacking = false;
    private int attackcounter = 0;
    private RaycastHit2D[] hitEnemies;
    private Knockback knockback;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
    }
    void Start()
    {
        
    }

    private void Update()
{
    if (isAttacking == false && !knockback.IsBeingKnockedBack)
        Move();
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
    anim.SetBool("grounded", grounded);
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

     private void Attack()
    {
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
        hitEnemies = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);
        
        for (int i = 0; i< hitEnemies.Length; i++)
        {
            // You can add the method to deal damage here
            EnemyHealth enemyHealth = hitEnemies[i].collider.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(attackDamage, transform.right);
            Debug.Log("Hitting enemy: " + enemyHealth);
        }

        // Reset attack flag after animation (assuming attack duration is 0.5 seconds)
        Invoke("ResetAttack", 1.1f);
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
        Gizmos.DrawWireSphere(attackTransform.position, attackRange); // Show attack range in the scene view
    }
}
