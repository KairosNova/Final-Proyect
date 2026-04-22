using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
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

    private void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        gameObject.SetActive(true);
    }

    private void Unpause()
    {
        Time.timeScale = 1f;
        isPaused = false;
        gameObject.SetActive(false);
    }
}
