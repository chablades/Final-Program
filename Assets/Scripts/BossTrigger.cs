using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private FinalBoss boss;
    [SerializeField] private bool destroyAfterTrigger = true;
    [SerializeField] private AudioClip bossIntroMusic;
    [SerializeField] private float fadeOutDuration = 1.5f;
    [SerializeField] private float fadeInDuration = 2f;
    
    private bool hasTriggered = false;
    private AudioSource audioSource;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && bossIntroMusic != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    
    private void Start()
    {
        // Auto-find boss if not assigned
        if (boss == null)
            boss = FindObjectOfType<FinalBoss>();
            
        if (boss == null)
            Debug.LogWarning("BossTrigger: No FinalBoss found in the scene");
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered || boss == null) return;
        
        if (collision.CompareTag("Player"))
        {
            hasTriggered = true;
            
            // Activate the boss
            boss.ActivateBoss();
            
            // Play intro music if assigned
            if (bossIntroMusic != null && audioSource != null)
            {
                StartCoroutine(TransitionToIntroMusic());
            }
            
            // Optional: lock doors, trigger cutscene, etc.
            
            // Destroy the trigger if needed
            if (destroyAfterTrigger)
            {
                // Keep the GameObject alive if we need to play music
                if (bossIntroMusic != null)
                {
                    // Disable the collider but keep the GameObject for audio
                    Collider2D col = GetComponent<Collider2D>();
                    if (col != null) col.enabled = false;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    
    private System.Collections.IEnumerator TransitionToIntroMusic()
    {
        // Find all audio sources to fade out
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        
        // Fade out current music
        foreach (AudioSource source in allAudioSources)
        {
            if (source != audioSource && source.isPlaying)
            {
                float startVolume = source.volume;
                float timer = 0;
                
                while (timer < fadeOutDuration)
                {
                    timer += Time.deltaTime;
                    source.volume = Mathf.Lerp(startVolume, 0, timer / fadeOutDuration);
                    yield return null;
                }
                
                source.Pause();
            }
        }
        
        // Start playing boss music
        audioSource.clip = bossIntroMusic;
        audioSource.volume = 0;
        audioSource.Play();
        
        // Fade in boss music
        float fadeTimer = 0;
        while (fadeTimer < fadeInDuration)
        {
            fadeTimer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, 1, fadeTimer / fadeInDuration);
            yield return null;
        }
    }
} 