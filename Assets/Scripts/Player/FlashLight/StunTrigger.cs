using System.Collections;
using UnityEngine;

public class StunTrigger : MonoBehaviour
{
    [SerializeField] private Collider2D triggerCollider;
    [SerializeField] private float stunTime;

    public void Trigger()
    {
        triggerCollider.enabled = true;
        Invoke(nameof(DisableTrigger), stunTime);
    }

    private void DisableTrigger()
    {
        triggerCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        collision.GetComponentInParent<IStunnable>().OnStun();
    }
}
