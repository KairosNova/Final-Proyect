using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;


public class TransitionScreen : MonoBehaviour
{
    [Header("UI references")]
    [SerializeField] private TextMeshPro introText;
    [SerializeField] private Image backgroundImage;


    [Header("Settings")]
    [SerializeField] private string gameSceneName = "Level1Test";


    public TextMeshProUGUI textComponent;
    [TextArea] public string fullText;

    public float delayBetweenWords = 0.04f;
    public float startDelay = 2f;
    public float endDelay = 3f;

    void Start()
    {
        StartCoroutine(StartTextAfterDelay());
        backgroundImage.gameObject.SetActive(false);
         
    }

    IEnumerator StartTextAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        textComponent.text = "";
        backgroundImage.gameObject.SetActive(true);
        
        

        string[] words = fullText.Split(' ');



        foreach (string word in words)
        {
            textComponent.text += word + " ";
            yield return new WaitForSeconds(delayBetweenWords);
        }
        yield return new WaitForSeconds(endDelay);
        SceneManager.LoadScene("UpperShipTest");
    }
}
