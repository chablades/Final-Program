using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    /* Player */
    public static GameManager Instance {get; private set;}

    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    
    [Header("Boss")]
    [SerializeField] private GameObject bossHealthBar;
    [SerializeField] private AudioClip victoryMusic;
    
    private bool _isPaused;
    private PlayerHealth _playerHealth;
    private bool _bossDefeated = false;
    private AudioSource _audioSource;
    
    
    /* Awake / Start */
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Get or add audio source for victory music
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();
    }
    

    /* Pause */
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    public void PlayerDied()
    {
        Time.timeScale = 0f;
        if (gameOverPanel) gameOverPanel.SetActive(true);
        RestartLevel();
    }

    public void RestartLevel()
    {
        if (pausePanel) pausePanel.SetActive(false); 
        _isPaused = false; 
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0f : 1f;
        if (pausePanel) pausePanel.SetActive(_isPaused);
    }

    public void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel) pausePanel.SetActive(false);
    }

    public void QuitToMenu()
{
    if (pausePanel) pausePanel.SetActive(false);
    Time.timeScale = 1f;
    SceneManager.LoadScene("MainMenu"); 
}
    
    public void BossDefeated()
    {
        _bossDefeated = true;
        
        // Hide boss health bar
        if (bossHealthBar != null)
            bossHealthBar.SetActive(false);
            
        // Play victory music
        if (victoryMusic != null && _audioSource != null)
        {
            // Fade out current music and play victory music
            StartCoroutine(FadeThenPlayVictoryMusic());
        }
        
        // Trigger victory UI after a delay
        Invoke("ShowVictoryScreen", 3f);
        
        // Unlock next level or trigger end game sequence
        // You might want to save progress here
        PlayerPrefs.SetInt("LevelCompleted_" + SceneManager.GetActiveScene().buildIndex, 1);
        PlayerPrefs.Save();
        
        Debug.Log("Boss defeated! Victory condition triggered.");
    }
    
    private System.Collections.IEnumerator FadeThenPlayVictoryMusic()
    {
        // Find the main audio source (assuming there's a separate music manager)
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in allAudioSources)
        {
            // Fade out other music sources
            if (source != _audioSource && source.isPlaying)
            {
                float startVolume = source.volume;
                float timer = 0;
                float fadeDuration = 2f;
                
                while (timer < fadeDuration)
                {
                    timer += Time.deltaTime;
                    source.volume = Mathf.Lerp(startVolume, 0, timer / fadeDuration);
                    yield return null;
                }
                
                source.Stop();
            }
        }
        
        // Play victory music
        _audioSource.clip = victoryMusic;
        _audioSource.volume = 0;
        _audioSource.Play();
        
        // Fade in victory music
        float vstartTime = 0;
        float vfadeDuration = 1.5f;
        
        while (vstartTime < vfadeDuration)
        {
            vstartTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(0, 1, vstartTime / vfadeDuration);
            yield return null;
        }
    }
    
    private void ShowVictoryScreen()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }
    
    // Optional: Load next level
    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // No more levels, go back to main menu
            QuitToMenu();
        }
    }
}


 



