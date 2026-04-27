using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;

    [Header("Audio")]
    public AudioSource musicSource;

    void Start()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }
    // 🎮 START GAME
    public void StartGame()
    {
        Debug.Log("Starting Game...");
        SceneManager.LoadScene("UpperShipTest"); 
    }

    // ⚙️ OPEN SETTINGS
    public void OpenSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    // 🔙 BACK TO MENU
    public void BackToMenu()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}