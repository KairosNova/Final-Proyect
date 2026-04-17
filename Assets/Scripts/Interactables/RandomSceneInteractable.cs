using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string[] scenes;

    public void OnInteract()
    {
        NextScene();
    }

    private void NextScene()
    {
        SceneManager.LoadScene(scenes[Random.Range(0, scenes.Length)]);
    }
}
