using System.Collections;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    [Header("Boss Stats")]
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float aggroRange = 10f;
    [SerializeField] private int attackDamage = 2;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayer;

    [Header("Boss Phases")]
    [SerializeField] private float phaseThreshold = 0.5f; // 50% health
    [SerializeField] private float enragedSpeedMultiplier = 1.5f;
    [SerializeField] private float enragedAttackRateMultiplier = 0.7f;
    [SerializeField] private GameObject specialAttackPrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip phaseChangeSound;
    [SerializeField] private AudioClip specialAttackSound;

    // Component references
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private BossHealthBar healthBar;
    
    // State variables
    private int currentHealth;
    private bool isEnraged = false;
    private bool isAttacking = false;
    private bool isDead = false;
    private bool isFacingRight = true;
    private bool canAttack = true;
    private bool isActivated = false;
    
    // Special attack patterns
    private enum AttackPattern { MeleeSlash, GroundPound, ProjectileBarrage }
    private AttackPattern currentPattern;

    // Public methods for UI to access
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public bool IsEnraged() => isEnraged;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthBar = FindObjectOfType<BossHealthBar>();
        
        // Don't immediately start attacking - wait for activation
        if (isActivated)
        {
            StartCoroutine(DecideNextAction());
            if (healthBar != null)
                healthBar.ShowBossHealth();
        }
    }

    public void ActivateBoss()
    {
        if (!isActivated)
        {
            isActivated = true;
            StartCoroutine(BossActivationSequence());
        }
    }

    private IEnumerator BossActivationSequence()
    {
        // Optional: play intro cutscene or animation
        
        // Show health bar
        if (healthBar != null)
            healthBar.ShowBossHealth();
            
        animator.SetTrigger("Activate");
        
        // Wait for animation to complete
        yield return new WaitForSeconds(2f);
        
        // Start AI
        StartCoroutine(DecideNextAction());
    }

    private void Update()
    {
        if (isDead || !isActivated) return;
        
        FlipTowardsPlayer();
        
        // Show visual feedback when enraged
        if (isEnraged)
        {
            float pulseValue = Mathf.PingPong(Time.time * 2, 0.3f) + 0.7f;
            spriteRenderer.color = new Color(pulseValue + 0.3f, pulseValue, pulseValue);
        }
    }

    private void FixedUpdate()
    {
        if (isDead || isAttacking || !isActivated) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // Move towards player if outside attack range but within aggro range
        if (distanceToPlayer > attackRange && distanceToPlayer < aggroRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float currentSpeed = moveSpeed;
        
        if (isEnraged)
            currentSpeed *= enragedSpeedMultiplier;
            
        rb.linearVelocity = direction * currentSpeed;
        animator.SetBool("IsMoving", true);
    }

    private void FlipTowardsPlayer()
    {
        if (player == null) return;
        
        bool playerIsRight = player.position.x > transform.position.x;
        
        // Only flip if needed
        if (playerIsRight != isFacingRight)
        {
            isFacingRight = playerIsRight;
            transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);
        }
    }

    private IEnumerator DecideNextAction()
    {
        while (!isDead)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            
            // Close enough to attack
            if (distanceToPlayer <= attackRange && canAttack)
            {
                yield return StartCoroutine(PerformAttack());
            }
            // In phase 2 (enraged), occasionally perform special attack
            else if (isEnraged && Random.value < 0.3f && canAttack)
            {
                yield return StartCoroutine(PerformSpecialAttack());
            }
            
            // Wait a bit before next decision
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator PerformAttack()
    {
        canAttack = false;
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;
        
        // Play attack animation
        animator.SetTrigger("Attack");
        
        // Play attack sound
        if (attackSound != null)
            audioSource.PlayOneShot(attackSound);
        
        // Wait for animation to reach damage point
        yield return new WaitForSeconds(0.5f);
        
        // Check for player hit
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if (hitPlayer != null)
        {
            PlayerHealth playerHealth = hitPlayer.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage, rb);
            }
        }
        
        // Finish attack
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        
        // Apply cooldown
        float cooldownTime = attackCooldown;
        if (isEnraged)
            cooldownTime *= enragedAttackRateMultiplier;
            
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
    }

    private IEnumerator PerformSpecialAttack()
    {
        canAttack = false;
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;
        
        // Choose a random special attack
        currentPattern = (AttackPattern)Random.Range(0, 3);
        
        // Play special attack animation
        animator.SetTrigger("SpecialAttack");
        
        // Play special attack sound
        if (specialAttackSound != null)
            audioSource.PlayOneShot(specialAttackSound);
        
        // Wait for animation to reach damage point
        yield return new WaitForSeconds(0.7f);
        
        // Perform the special attack effect
        switch (currentPattern)
        {
            case AttackPattern.MeleeSlash:
                PerformMeleeSlash();
                break;
            case AttackPattern.GroundPound:
                PerformGroundPound();
                break;
            case AttackPattern.ProjectileBarrage:
                StartCoroutine(PerformProjectileBarrage());
                break;
        }
        
        // Finish attack
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
        
        // Apply cooldown
        yield return new WaitForSeconds(3.0f);
        canAttack = true;
    }

    private void PerformMeleeSlash()
    {
        // Wide area sweep attack
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRange * 1.5f, playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage + 1, rb);
            }
        }

        // Visual effect (could be implemented with particle system)
        Debug.Log("Boss performed Melee Slash attack!");
    }

    private void PerformGroundPound()
    {
        // Ground pound attack with knockback
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRange * 2f, playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage, rb);
                
                // Extra knockback effect
                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 knockbackDir = (player.transform.position - transform.position).normalized;
                    playerRb.AddForce(knockbackDir * 15f, ForceMode2D.Impulse);
                }
            }
        }

        // Visual effect - camera shake would be good here too
        Debug.Log("Boss performed Ground Pound attack!");
    }

    private IEnumerator PerformProjectileBarrage()
    {
        // Shoot multiple projectiles in a pattern
        if (specialAttackPrefab != null)
        {
            int projectileCount = 5;
            for (int i = 0; i < projectileCount; i++)
            {
                // Calculate spread angle
                float angle = -30f + (i * 15f);
                Vector2 direction = Quaternion.Euler(0, 0, angle) * (isFacingRight ? Vector2.right : Vector2.left);
                
                // Create projectile
                GameObject projectile = Instantiate(specialAttackPrefab, attackPoint.position, Quaternion.identity);
                BossProjectile projectileScript = projectile.GetComponent<BossProjectile>();
                if (projectileScript != null)
                {
                    projectileScript.Initialize(direction, attackDamage);
                }
                
                yield return new WaitForSeconds(0.2f);
            }
        }
        
        Debug.Log("Boss performed Projectile Barrage attack!");
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        
        // If boss isn't activated yet, activate it when hit
        if (!isActivated)
        {
            ActivateBoss();
        }
        
        currentHealth -= damage;
        
        // Play hit animation
        animator.SetTrigger("Hit");
        
        // Play hurt sound
        if (hurtSound != null)
            audioSource.PlayOneShot(hurtSound);
        
        // Flash the sprite
        StartCoroutine(DamageFlash());
        
        // Check for phase change
        if (!isEnraged && (float)currentHealth / maxHealth <= phaseThreshold)
        {
            EnterPhaseTwo();
        }
        
        // Check for death
        if (currentHealth <= 0)
        {
            Die();
        }
        
        Debug.Log("Boss took damage! Health: " + currentHealth + "/" + maxHealth);
    }

    private IEnumerator DamageFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = isEnraged ? new Color(1.3f, 1, 1) : Color.white;
    }

    private void EnterPhaseTwo()
    {
        isEnraged = true;
        
        // Play phase change sound
        if (phaseChangeSound != null)
            audioSource.PlayOneShot(phaseChangeSound);
        
        // Play transition animation
        animator.SetTrigger("Enrage");
        
        // Show a visual effect
        spriteRenderer.color = new Color(1.3f, 1, 1);
        
        Debug.Log("Boss has entered Phase Two!");
    }

    private void Die()
    {
        isDead = true;
        
        // Stop movement
        rb.linearVelocity = Vector2.zero;
        
        // Disable colliders
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
        
        // Play death animation
        animator.SetTrigger("Death");
        
        // Play death sound
        if (deathSound != null)
            audioSource.PlayOneShot(deathSound);
        
        // Notify game manager or trigger victory condition
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.BossDefeated();
        }
        
        // Optional: spawn rewards, trigger cinematics, etc.
        Debug.Log("Boss has been defeated!");
        
        // Destroy after animation
        Destroy(gameObject, 3f);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw attack range
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
        
        // Draw aggro range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
    
    // For trigger-based boss activation
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivated && collision.CompareTag("Player"))
        {
            ActivateBoss();
        }
    }
} 