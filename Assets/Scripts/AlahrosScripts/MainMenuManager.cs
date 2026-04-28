using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("INITIAL SETUP")]
    [SerializeField] private PlayerData playerData;

    [Header("SCENES")]
    [SerializeField] private string startScene;

    [Header("PANELS")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("AUDIO")]
    [SerializeField] private AudioSource musicSource;

    void Start()
    {
        if (musicSource != null && !musicSource.isPlaying) // migrar a singleton de musica
        {
            musicSource.Play();
        }
    }

    // 🎮 START GAME
    public void StartGame()
    {
        playerData.currentHealth = playerData.maxHealth;
        SceneManager.LoadScene(startScene);
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