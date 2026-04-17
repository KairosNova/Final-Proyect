using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class FlashLightVisuals : MonoBehaviour
{
    [SerializeField] private Light2D flashlightLight;
    [SerializeField] private float impactAddedLightIntensity;
    private float baseLightIntensity;
    [SerializeField] private float decayTime;

    private void Start()
    {
        baseLightIntensity = flashlightLight.intensity;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        StartCoroutine(FlashLightImpactVisual());
    }

    private IEnumerator FlashLightImpactVisual()
    {
        float timer = decayTime;
        while (timer > 0)
        {
            flashlightLight.intensity = baseLightIntensity + (impactAddedLightIntensity * (timer/decayTime));
            timer -= Time.deltaTime;
            yield return null;
        }
        flashlightLight.intensity = baseLightIntensity;
    }
}
