using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private LayerMask collisionLayers;
    [SerializeField] private AudioClip impactSound;
    
    private Vector2 direction;
    private int damage;
    private bool hasHit = false;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    private AudioSource audioSource;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
    
    private void Start()
    {
        Destroy(gameObject, lifetime);
        
        // Set rotation to match direction
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    
    private void FixedUpdate()
    {
        if (!hasHit)
        {
            rb.linearVelocity = direction * speed;
        }
    }
    
    public void Initialize(Vector2 direction, int damage)
    {
        this.direction = direction.normalized;
        this.damage = damage;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;
        
        if (((1 << collision.gameObject.layer) & collisionLayers) != 0)
        {
            hasHit = true;
            rb.linearVelocity = Vector2.zero;
            
            // Handle player hit
            if (collision.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    // Use a fake Rigidbody2D reference since we don't need it for knockback direction
                    playerHealth.TakeDamage(damage, rb);
                }
            }
            
            // Visual effect
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }
            
            // Play impact sound
            if (impactSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(impactSound);
                // Wait for sound to play before destroying
                Destroy(gameObject, 0.2f);
            }
            else
            {
                // No sound, destroy immediately
                DestroyProjectile();
            }
        }
    }
    
    private void DestroyProjectile()
    {
        // Hide visual components
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
            
        if (trailRenderer != null)
            trailRenderer.enabled = false;
            
        // Destroy game object
        Destroy(gameObject);
    }
} 