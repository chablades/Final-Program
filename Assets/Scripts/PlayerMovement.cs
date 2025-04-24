using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 1;
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

    private void Update(){
    float horizontalInput = Input.GetAxis("Horizontal");
    rb.linearVelocity = new Vector2(horizontalInput * runSpeed, rb.linearVelocity.y);
    
    if (horizontalInput > 0.01f)
        transform.localScale = Vector2.one;
    else if (horizontalInput < -0.01f)
        transform.localScale = new Vector2(-1, 1);

    if (Input.GetKey(KeyCode.Space) && grounded)
    {
        Jump();
    }

    if (Input.GetKeyDown(KeyCode.F) && !isAttacking){ //press F to attack
        Attack();
        Debug.Log("Attacking!"); //printing output to show attacking
    }

    anim.SetBool("run", horizontalInput != 0);
    anim.SetBool("grounded", grounded);
}
    private void Jump(){
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, runSpeed);
        anim.SetTrigger("jump");
        grounded = false;
    }

     private void Attack(){
        isAttacking = true;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy")); //detecting enemies in range
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hitting enemy: " + enemy.name);
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        }
        Invoke("ResetAttack", 0.5f);
    }

    private void ResetAttack(){
        isAttacking = false;
    }
    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
    }
    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); //shows attack range
    }
}
