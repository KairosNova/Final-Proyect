using UnityEngine;
using UnityEngine.UI;

public class LifebarUI : MonoBehaviour
{
    [SerializeField] private Image barFillImage;

    private void Start()
    {
        PlayerHealth.onHealthPercentageChanged += UpdateBar;
    }

    private void UpdateBar(float percentage)
    {
        Debug.Log("Barra de vida actualizada al " + percentage + "%");

        if (percentage > 1f) percentage = 1f;
        if (percentage < 0f) percentage = 0f;

        barFillImage.fillAmount = percentage;
    }
}
