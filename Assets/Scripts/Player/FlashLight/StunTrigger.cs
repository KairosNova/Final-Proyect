using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class StunTrigger : MonoBehaviour
{
    [SerializeField] private float stunTime;

    private IStunnable stunnedEntity; // temporal

    public void OnInteract(InputAction.CallbackContext context)
    {
        //Invoke()
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stunnedEntity = collision.GetComponentInParent<IStunnable>();

        stunnedEntity.OnStun();
    }
}
