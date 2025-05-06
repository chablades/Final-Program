using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private int damageAmount = 1;  //Amount of damage the enemy deals
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    //reference rigidbody and animator
    private Animator anim;
    private bool isAttacking = false;

    private bool isLeft = true;
    private RaycastHit2D hitPlayer;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;
    private EnemyHealth enemyHealth;
    private KnockbackEnemy knockbackEnemy;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealth = GetComponent<EnemyHealth>();
        knockbackEnemy = GetComponent<KnockbackEnemy>();
    }

    private void FixedUpdate()
    {
        if (knockbackEnemy.EnemyIsBeingKnockedBack == false && isAttacking == false){
            Move();
        }
    }

    private void Move(){
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < detectionRange)
        {
            anim.SetBool("Moving", true);
            Vector2 direction = (player.position - transform.position).normalized;
            movement = new Vector2(direction.x, 0f);

            if (player.position.x > transform.position.x){
                transform.localScale = new Vector2(-1, 1); //Facing right
                isLeft = false;
            }
            else if (player.position.x < transform.position.x){
                transform.localScale = new Vector2(1, 1); //Facing left
                isLeft = true;
            }
            if (knockbackEnemy.EnemyIsBeingKnockedBack == false){
                rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D player){
        if (player.gameObject.name.Equals("Player")){
            if (isAttacking == false){
                isAttacking = true;
                anim.SetBool("hasTarget", true);

                Invoke("Attack", 0.5f);
                
                //resetting enemy attack flag
                Invoke("ResetAttack", 1.0f);
            }
        }
    }

    private void Attack(){
        hitPlayer = Physics2D.CircleCast(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);
        
        PlayerHealth playerHealth = hitPlayer.collider.gameObject.GetComponent<PlayerHealth>();
        if(isLeft==true)
            playerHealth.TakeDamage(damageAmount, rb);
        else 
            playerHealth.TakeDamage(damageAmount, rb);
    }
    
    private void ResetAttack()
    {
        isAttacking = false;
        anim.SetBool("hasTarget", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransform.position, attackRange); //showing player attack range in scene view
    }
}
