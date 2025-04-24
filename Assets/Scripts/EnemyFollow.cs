using UnityEngine;

public class EnemyFollow : MonoBehaviour{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private int damageAmount = 1;  //amount of damage the enemy will do
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update(){
        if (player != null){
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance < detectionRange){
                Vector2 direction = (player.position - transform.position).normalized;
                movement = new Vector2(direction.x, 0f);
                if (player.position.x > transform.position.x)
                    transform.localScale = new Vector3(1, 1, 1); //facing right
                else if (player.position.x < transform.position.x)
                    transform.localScale = new Vector3(-1, 1, 1); //facing left
            }
            else{
                movement = Vector2.zero;
            }
        }
    }

    private void FixedUpdate(){
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
    }
        private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Player")){
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null){
                playerHealth.TakeDamage(damageAmount); //dealing damage to the player
            }
        }
    }
}
