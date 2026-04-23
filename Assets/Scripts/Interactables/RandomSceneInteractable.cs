using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string[] scenes;
    [SerializeField] private TransitionType transitionType;
    [SerializeField] private float transitionTime;
    public void OnInteract()
    {
        NextScene();
    }

    private void NextScene()
    {
        SceneTransitionUtility.Instance.LoadScene(scenes[Random.Range(0, scenes.Length)], transitionType, transitionTime);
    }
}
