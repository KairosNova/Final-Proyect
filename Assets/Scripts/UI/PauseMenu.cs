using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private float mainMenuTransitionTime;
    private bool isPaused = false;

    private void Start()
    {
        Unpause();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isPaused) Unpause();
            else Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        gameObject.SetActive(true);
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        isPaused = false;
        gameObject.SetActive(false);

        // Cuando se pausa el juego una segunda vez, el boton deja de mostrar el overlay de color (highlight) esta linea es para corregirlo:
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void GoToScene(string sceneName)
    {
        SceneTransitionUtility.Instance.LoadScene(sceneName, TransitionType.Fill, mainMenuTransitionTime);
    }
}
