using System.Threading;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Projectile : MonoBehaviour
{
    //serialized fields
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private int damageAmount = 1;  // Amount of damage the enemy will deal

    //reference rigidbody and animator
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;
    
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //grab direction and speed
        Vector2 direction = (player.position - transform.position).normalized;
        movement = new Vector2(direction.x * moveSpeed, 0f);

        // Flip to face player 
        if (player.position.x > transform.position.x){
            transform.localScale = new Vector2(-1, 1); // Facing right
        }
        else if (player.position.x < transform.position.x){
            transform.localScale = new Vector2(1, 1); // Facing left
        }

        //move projectile
        rb.linearVelocity = movement;

        //destroy after 3 seconds
        Destroy(gameObject,3f);
    }
    
    private void OnTriggerEnter2D(Collider2D player){
        //check if player
        if (player.gameObject.name.Equals("Player")){
            //destroy projectile
            rb.linearVelocity = Vector2.zero;

            //damage player
            Debug.Log("projectile hit player");
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damageAmount, rb);
            Destroy(gameObject);
        }
    }
}
