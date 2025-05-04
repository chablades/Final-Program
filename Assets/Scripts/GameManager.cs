using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /* Player */
    public static GameManager Instance {get; private set;}

    public GameObject pausePanel;
    public GameObject gameOverPanel;
    private bool _isPaused;
    private PlayerHealth _playerHealth;
    
    
    /* Awake / Start */
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
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

    
    

}


 



