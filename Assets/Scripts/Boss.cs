using System;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float dashSpeed = 5f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float dashRange = 10f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float dashCooldown = 10f;
    [SerializeField] private int damageAmount = 1;  //Amount of damage the enemy deals
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private Transform attackTransform;
    //reference rigidbody and animator
    private Animator anim;
    private bool isAttacking = false;

    private float dashTimer = 0f;
    private bool isLeft;
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

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if(distance < attackRange ){
            SwordSlash();
        }
        else if(distance < dashRange && dashTimer > dashCooldown){
            Dash();
        }        
    }

    private void FixedUpdate()
    {
        if (isAttacking == false && dashTimer < dashCooldown){
            Move();
        }
        dashTimer += Time.fixedDeltaTime;    
    }

    private void SwordSlash(){
        anim.SetBool("slash", true);
        anim.SetBool("walking", false);
        hitPlayer = Physics2D.CircleCast(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);
        
        PlayerHealth playerHealth = hitPlayer.collider.gameObject.GetComponent<PlayerHealth>();
        if(isLeft==true)
            playerHealth.TakeDamage(damageAmount, rb);
        else 
            playerHealth.TakeDamage(damageAmount, rb);

        Invoke("ResetAttack", 0.5f);
    }

    private void Dash(){
        anim.SetBool("dash", true);
        anim.SetBool("walking", false);
        Vector2 direction = (player.position - transform.position).normalized;
        movement = new Vector2(direction.x, 0f);

        if (player.position.x > transform.position.x){
            transform.localScale = new Vector2(-1, 1); //Facing right
        }
        else if (player.position.x < transform.position.x){
            transform.localScale = new Vector2(1, 1); //Facing left
        }
        if (knockbackEnemy.EnemyIsBeingKnockedBack == false){
            rb.linearVelocity = new Vector2(movement.x * dashSpeed, rb.linearVelocity.y);
        }
        dashTimer = 0f;
    }

    private void Move(){
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < detectionRange)
        {
            anim.SetBool("walking", true);
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
                anim.SetBool("slash", true);

                Invoke("SwordSlash", 0.7f);
                
                //resetting enemy attack flag
                Invoke("ResetAttack", 0.5f);
            }
        }
    }
    
    private void ResetAttack()
    {
        isAttacking = false;
        anim.SetBool("slash", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransform.position, attackRange); //showing player attack range in scene view
    }
}
