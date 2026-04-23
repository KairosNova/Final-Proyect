using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum TransitionType
{
    Fade,
    Fill
}

public class SceneTransitionUtility : MonoBehaviour
{
    public static SceneTransitionUtility Instance;
    public Image blackScreenImage;

    private String nextSceneName;
    private TransitionType activeTransitionType;
    private float activeTransitionTime;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.activeSceneChanged += EndTransition;
    }

    public void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        LoadNextScene();
    }

    public void LoadScene(string sceneName, TransitionType transitionType, float transitionTime)
    {
        activeTransitionTime = transitionTime / 2f;
        nextSceneName = sceneName;
        StartTransition(transitionType);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadSceneAsync(nextSceneName);
    }

    private void StartTransition(TransitionType transitionType)
    {
        activeTransitionType = transitionType;
        blackScreenImage.enabled = true;

        switch (transitionType)
        {
            case TransitionType.Fade:
                StartCoroutine(FadeTransition(false));
                break;

            case TransitionType.Fill:
                StartCoroutine(FillTransition(false));
                break;
        }
    }

    public void EndTransition(Scene scene1, Scene scene2)
    {
        switch (activeTransitionType)
        {
            case TransitionType.Fade:
                StartCoroutine(FadeTransition(true));
                break;
            case TransitionType.Fill:
                StartCoroutine(FillTransition(true));
                break;
        }
    }

    // mover a una clase Transition y hacer hijos para fade y fill
    private IEnumerator FadeTransition(bool isFinishing)
    {
        float timer = 0f;
        float progress;

        while (timer < activeTransitionTime)
        {
            Debug.Log(timer);
            progress = timer / activeTransitionTime;
            if (isFinishing) progress = 1f - progress;

            blackScreenImage.color = new Color(0f, 0f, 0f, progress);
            
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        progress = 1f;
        blackScreenImage.color = new Color(0f, 0f, 0f, progress);

        if (isFinishing)
        {
            blackScreenImage.enabled = false;
            Time.timeScale = 1f;
        }
        else
        {
            LoadNextScene();
        }
    }

    private IEnumerator FillTransition(bool isFinishing)
    {
        float timer = 0f;
        float progress;

        while (timer < activeTransitionTime)
        {
            Debug.Log(timer);
            progress = timer / activeTransitionTime;
            if (isFinishing) progress = 1f - progress;

            blackScreenImage.fillAmount = progress;

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        progress = 1f;
        blackScreenImage.fillAmount = progress;

        if (isFinishing)
        {
            blackScreenImage.enabled = false;
            Time.timeScale = 1f;
        }
        else
        {
            LoadNextScene();
        }
    }
}
