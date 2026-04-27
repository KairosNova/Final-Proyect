using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("SCENES")]
    [SerializeField] private string startScene;
    [SerializeField] private float startTransitionTime;


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
        SceneTransitionUtility.Instance.LoadScene(startScene, TransitionType.Fill, startTransitionTime);
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