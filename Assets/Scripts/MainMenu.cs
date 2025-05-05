using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Stage1-Kingdom"); 
    }
    
    public void QuitGame()
    {
        Debug.Log("Quit Game"); 
        Application.Quit();
    }
}