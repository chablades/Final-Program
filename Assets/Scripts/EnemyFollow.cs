using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private int damageAmount = 1;  // Amount of damage the enemy will deal

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance < detectionRange)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                movement = new Vector2(direction.x, 0f);

                // Flip to face player (this step can be optional depending on your design)
                if (player.position.x > transform.position.x)
                    transform.localScale = new Vector3(1, 1, 1); // Facing right
                else if (player.position.x < transform.position.x)
                    transform.localScale = new Vector3(-1, 1, 1); // Facing left
            }
            else
            {
                movement = Vector2.zero;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
    }

    // This function will be triggered when the enemy collides with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Assuming the player has a PlayerHealth script that handles the health system
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount); // Deal damage to the player
            }
        }
    }
}
