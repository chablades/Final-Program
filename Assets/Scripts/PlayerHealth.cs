using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour{
    [SerializeField] private int maxLives = 3;
    private int currentLives;

    private void Start(){
        currentLives = maxLives;
        UIManager.Instance.UpdateLives(currentLives);
    }
    public void TakeDamage(int damage){
        currentLives -= damage;
        UIManager.Instance.UpdateLives(currentLives);
        Debug.Log("Player took damage! Lives left: " + currentLives);
        if (currentLives <= 0){
            Die();
        }
    }
    private void Die(){
        Debug.Log("Player Died!");
        GameManager.Instance.PlayerDied();
    }
}
