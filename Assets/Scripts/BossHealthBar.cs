using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private GameObject healthBarPanel;
    
    [Header("Colors")]
    [SerializeField] private Color normalColor = new Color(0.8f, 0.2f, 0.2f); // Red
    [SerializeField] private Color phase2Color = new Color(0.9f, 0.4f, 0.1f); // Orange
    
    [Header("Animation")]
    [SerializeField] private float animationSpeed = 5f;
    [SerializeField] private float pulseSpeed = 3f;
    [SerializeField] private float pulseAmount = 0.2f;
    
    [Header("References")]
    [SerializeField] private FinalBoss boss;
    
    private float targetFill = 1f;
    private bool isAnimating = false;
    private bool isPhaseTwo = false;
    private CanvasGroup canvasGroup;
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            
        // Initially hide the health bar until boss is activated
        canvasGroup.alpha = 0;
    }
    
    private void Start()
    {
        // Auto-find boss if not assigned
        if (boss == null)
            boss = FindObjectOfType<FinalBoss>();
            
        if (boss != null && healthBarPanel != null)
        {
            // Subscribe to boss events
            // Set initial values
            healthSlider.value = 1.0f;
            
            // Set boss name if needed
            if (bossNameText != null)
                bossNameText.text = "Kingdom Guardian";
        }
        else
        {
            Debug.LogWarning("BossHealthBar: Missing references to boss or UI elements");
        }
    }
    
    private void Update()
    {
        if (boss == null) return;
        
        // Get current health percentage from boss
        float currentHealthPercent = (float)boss.GetCurrentHealth() / boss.GetMaxHealth();
        
        // Animate health bar toward target
        if (targetFill != currentHealthPercent)
        {
            targetFill = currentHealthPercent;
            isAnimating = true;
        }
        
        if (isAnimating)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetFill, Time.deltaTime * animationSpeed);
            
            // Check if animation is basically done
            if (Mathf.Abs(healthSlider.value - targetFill) < 0.01f)
            {
                healthSlider.value = targetFill;
                isAnimating = false;
            }
        }
        
        // Phase two coloring
        if (!isPhaseTwo && targetFill <= 0.5f)
        {
            isPhaseTwo = true;
            
            // Change color
            if (fillImage != null)
                fillImage.color = phase2Color;
                
            // Pulse effect
            StartPulse();
        }
        
        // Pulse effect in phase two
        if (isPhaseTwo && fillImage != null)
        {
            float pulse = Mathf.PingPong(Time.time * pulseSpeed, pulseAmount);
            fillImage.transform.localScale = new Vector3(1f, 1f + pulse, 1f);
        }
    }
    
    private void StartPulse()
    {
        // Could add a brief flash or screen shake here
        Debug.Log("Boss entered Phase Two - Health bar pulsing!");
    }
    
    public void ShowBossHealth()
    {
        // Fade in the health bar when the boss is activated
        StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, 1f));
    }
    
    private System.Collections.IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cg.alpha = end;
    }
} 