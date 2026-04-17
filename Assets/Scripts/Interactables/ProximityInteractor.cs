using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class ProximityInteractor : MonoBehaviour
{
    [SerializeField] private GameObject visualIndicator;

    private Transform closestTarget;
    private HashSet<Transform> targets;
    private bool isLookingForTarget = true;

    private GameObject grabbedObject;

    private void Awake()
    {
        targets = new HashSet<Transform>();
    }

    private void Start()
    {
        UnattachInteractorVisuals();
    }

    private void UnattachInteractorVisuals()
    {
        visualIndicator.transform.SetParent(null);
        visualIndicator.SetActive(false);
    }

    private void AttachInteractorVisuals(Transform target)
    {
        visualIndicator.SetActive(true);
        visualIndicator.transform.SetParent(target);
        visualIndicator.transform.localPosition = Vector3.zero;
    }

    private void Update() // se podria cambiar a OnTick en un futuro para mejor performance
    {
        if (isLookingForTarget && targets.Count != 0)
        {
            CheckClosestTarget();
            AttachInteractorVisuals(closestTarget);
        }
    }

    private void CheckClosestTarget()
    {
        float minDistance = float.MaxValue;
        foreach (Transform target in targets)
        {
            float currentTargetDistance = (transform.position - target.position).magnitude;
            if (currentTargetDistance < minDistance)
            {
                minDistance = currentTargetDistance;
                closestTarget = target;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerInteractable"))
        {
            targets.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerInteractable"))
        {
            targets.Remove(collision.transform);

            if (targets.Count == 0)
            {
                UnattachInteractorVisuals();
            }
        }
    }

    private void LockTarget()
    {
        isLookingForTarget = false;
    }

    public void UnlockTarget()
    {
        isLookingForTarget = true;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        closestTarget.GetComponent<IInteractable>().OnInteract();
    }
}
