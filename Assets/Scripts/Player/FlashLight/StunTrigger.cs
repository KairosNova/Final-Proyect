using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class StunTrigger : MonoBehaviour
{
    [SerializeField] private Collider2D triggerCollider;
    [SerializeField] private float stunTime;

    private IStunnable stunnedEntity; // temporal

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            triggerCollider.enabled = true;
            Invoke(nameof(DisableTrigger), stunTime);
        }
    }

    private void DisableTrigger()
    {
        triggerCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        Debug.Log("DENTRO STUN!");

        stunnedEntity = collision.GetComponentInParent<IStunnable>();

        stunnedEntity.OnStun();
    }
}
