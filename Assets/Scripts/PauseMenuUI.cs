using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        // safety: if a slot is missing, try to find by name
        if (!resumeButton)  resumeButton  = transform.Find("Resume").GetComponent<Button>();
        if (!restartButton) restartButton = transform.Find("Restart").GetComponent<Button>();
        if (!quitButton)    quitButton    = transform.Find("Quit").GetComponent<Button>();

        // hook the listeners
        resumeButton.onClick.AddListener(() => GameManager.Instance.ResumeGame());
        restartButton.onClick.AddListener(() => GameManager.Instance.RestartLevel());
        quitButton.onClick.AddListener(() => GameManager.Instance.QuitToMenu());
    }
}