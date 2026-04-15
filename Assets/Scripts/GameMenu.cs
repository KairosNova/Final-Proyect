using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public Button Play;
    public Button Pause;
    public Button Restart;
    public Button Exit; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Play.onClick.AddListener(PlayGame);
        Pause.onClick.AddListener(PauseGame);
        Restart.onClick.AddListener(RestartGame);
        Exit.onClick.AddListener(ExitGame);
    }

    void PlayGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Play");
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        Debug.Log("Pause");
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit");
    }


   
}
