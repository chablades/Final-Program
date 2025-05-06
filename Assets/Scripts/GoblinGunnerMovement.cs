using System;
using Unity.VisualScripting;
using UnityEngine;

public class GoblinGUnnerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float movingRange = 5f;
    [SerializeField] private float fireRate = 5f;
    [SerializeField] private int damageAmount = 1;  // Amount of damage the enemy will deal
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float attackRange = 1.5f;
    //reference rigidbody and animator
    private Animator anim;
    private bool isShooting = false;

    private bool isLeft = true;
    private float fireTimer;
    private RaycastHit2D[] hitPlayer;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;
    private EnemyHealth enemyHealth;
    private KnockbackEnemy knockbackEnemy;
    private Projectile projectile;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealth = GetComponent<EnemyHealth>();
        knockbackEnemy = GetComponent<KnockbackEnemy>();
        projectile = GetComponent<Projectile>();
    }

    private void Start()
    {
        fireTimer = Time.time;
    }
    private void FixedUpdate()
    {
        Shoot();
        if (knockbackEnemy.EnemyIsBeingKnockedBack == false && isShooting == false){
            Move();
        }
    }

    private void Move(){
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < movingRange)
        {
            anim.SetBool("Moving", true);
            Vector2 direction = (player.position - transform.position).normalized;
            movement = new Vector2(-direction.x, 0f);

            // Flip to face player (this step can be optional depending on your design)
            if (player.position.x > transform.position.x){
                transform.localScale = new Vector2(1, 1); // Facing right]
                isLeft = false;
            }
            else if (player.position.x < transform.position.x){
                transform.localScale = new Vector2(-1, 1); // Facing left\
                isLeft = true;
            }
            rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
        }
        else{
            anim.SetBool("Moving", false);
        }
    }
    private void Shoot(){
        float distance = Vector2.Distance(transform.position, player.position);
        if (Time.time > fireTimer && distance < detectionRange)
        {
            anim.SetTrigger("hasTarget");
            rb.linearVelocity = Vector2.zero;
            isShooting = true;

            Invoke("Attack", 1f);
            fireTimer = Time.time + fireRate;
            // Reset attack flag after animation (assuming attack duration is 0.5 seconds)
            Invoke("ResetAttack", 1f);
        }
    }

    private void Attack(){
        Instantiate(bullet, transform.position, Quaternion.identity);
    }
        
    private void ResetAttack()
    {
        anim.SetBool("hasTarget", false);
        isShooting = false;
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
}
