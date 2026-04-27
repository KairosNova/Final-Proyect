using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(FlashLightVisuals), typeof(StunTrigger))]
public class FlashLight : MonoBehaviour
{
    [SerializeField] private float rechargeTime;
    private float timer;

    private FlashLightVisuals visuals;
    private StunTrigger stun;

    [Header("UI")]
    [SerializeField] private Image barFillImage;


    private void UpdateBar(float percentage)
    {
        if (percentage > 1f) percentage = 1f;
        if (percentage < 0f) percentage = 0f;

        barFillImage.fillAmount = percentage;
    }

    private void Start()
    {
        visuals = GetComponent<FlashLightVisuals>();
        stun = GetComponent<StunTrigger>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        UpdateBar(timer/rechargeTime);
    }

    public void OnFlashlightUse(InputAction.CallbackContext context)
    {
        if (context.started && timer >= rechargeTime)
        {
            timer = 0f;
            visuals.Trigger();
            stun.Trigger();
        }
    }
}
