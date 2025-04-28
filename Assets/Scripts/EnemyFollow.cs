using Unity.VisualScripting;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private int damageAmount = 1;  // Amount of damage the enemy will deal
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    //reference rigidbody and animator
    private Animator anim;
    private bool isAttacking = false;
    private RaycastHit2D[] hitPlayer;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player != null)
        {
            
            float distance = Vector2.Distance(transform.position, player.position);
           

            if (distance < detectionRange)
            {
                anim.SetBool("Moving", true);
                Vector2 direction = (player.position - transform.position).normalized;
                if(isAttacking == false)
                    movement = new Vector2(direction.x, 0f);

                // Flip to face player (this step can be optional depending on your design)
                if (player.position.x > transform.position.x)
                    transform.localScale = new Vector3(-1, 1, 1); // Facing right
                else if (player.position.x < transform.position.x)
                    transform.localScale = new Vector3(1, 1, 1); // Facing left
            }
            else
            {
                movement = Vector2.zero;
                anim.SetBool("Moving", false);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if (isAttacking) return; //if already attacking, dont double attack

        //check if the thing we are colliding with is in the attackable layer
        if (((1 << collider.gameObject.layer) & attackableLayer) != 0)
        {
            isAttacking = true;
            anim.SetBool("isAttacking", true);

            //delay the attack damage to help match the animation timing
            Invoke("DealDamage", 0.5f);

            // Reset attack flag after animation (assuming attack duration is 0.5 seconds)
            Invoke("ResetAttack", 1.0f);
        }
    }

    private void DealDamage()
    {
        hitPlayer = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);

        for (int i = 0; i < hitPlayer.Length; i++)
        {
            // You can add the method to deal damage here
            PlayerHealth playerHealth = hitPlayer[i].collider.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damageAmount);
            Debug.Log("Player Hit: " + playerHealth);
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }


    // This function will be triggered when the enemy collides with the player
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         // Assuming the player has a PlayerHealth script that handles the health system
    //         hitPlayer = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);
        
    //     for (int i = 0; i< hitPlayer.Length; i++)
    //     {
    //         // You can add the method to deal damage here
    //         PlayerHealth playerHealth = hitPlayer[i].collider.gameObject.GetComponent<PlayerHealth>();
    //         playerHealth.TakeDamage(damageAmount);
    //         Debug.Log("Player Hit: " + playerHealth);
    //     }
    //     }
    // }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransform.position, attackRange); // Show attack range in the scene view
    }
}
