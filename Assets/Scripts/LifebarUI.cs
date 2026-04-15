using UnityEngine;
using UnityEngine.UI;

public class LifebarUI : MonoBehaviour
{
    [SerializeField] private Image barFillImage;
    [SerializeField] private PlayerData playerData;

    private void Start()
    {
        UpdateBar();
    }

    private void UpdateBar()
    {
        barFillImage.fillAmount = playerData.currentHealth / playerData.maxHealth;
    }
}
