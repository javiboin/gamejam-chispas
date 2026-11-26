using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string _gameSceneName = "MainScene"; //podemos usar el index de escenas (aunque igual son pocas xd)

    [Header("UI Panels")]
    [SerializeField] private GameObject _creditsPanel;

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(_gameSceneName);
    }
    public void OpenCredits()
    {
        if (_creditsPanel != null)
        {
            _creditsPanel.SetActive(true);
        }
    }
    public void CloseCredits()
    {
        if (_creditsPanel != null)
        {
            _creditsPanel.SetActive(false);
        }
    }
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }   
}
