using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _gameOverPanel;

    [Header("Scene Settings")]
    [SerializeField] private string _mainMenuSceneName = "MainMenu"; 

    private void Start()
    {

        if (_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(false);
        }
        TriggerGameOver(); //LLAMO TEMPORALMENTE A GAMEOVER PARA PROBAR QUE FUNCIONE
    }

    public void TriggerGameOver()
    {
        if (_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(_mainMenuSceneName);
    }
}
