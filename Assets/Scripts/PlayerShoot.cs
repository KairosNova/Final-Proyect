using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Animator armsAnimator;
    [SerializeField] private LineRenderer lineRenderer;
    private PlayerAim playerAim;

    [Header("Configuración")]
    [SerializeField] private float range = 10f;
    [SerializeField] private float fireDelay = 1.5f;
    [SerializeField] private float movementLockTime = 0.4f;
    [SerializeField] private int damage = 100;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float tracerDuration = 0.05f;

    private bool canShoot = true;
    public bool isShooting = false;

    void Awake()
    {
        playerAim = GetComponent<PlayerAim>();
    }

    void Update()
    {
        // Verificamos que el mouse y playerAim no sean nulos antes de preguntar
        if (Mouse.current != null && playerAim != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame && playerAim.isAiming && canShoot)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        // 1. Animación (Solo una vez y aquí mismo)
        if (armsAnimator != null)
        {
            armsAnimator.SetTrigger("Shoot");
        }

        // 2. Lógica de Raycast
        if (firePoint == null) return;

        Vector2 direction = playerAim.aimDirection;
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, range, enemyLayer);

        if (hit.collider != null)
        {
            Health enemyHealth = hit.collider.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }

        // 3. Feedback visual y Cooldown
        Vector2 endPoint = hit.collider != null ? hit.point : (Vector2)firePoint.position + (direction * range);
        StartCoroutine(ShowTracer(firePoint.position, endPoint));
        StartCoroutine(ShootCooldown());
    }

    IEnumerator ShowTracer(Vector3 start, Vector3 end)
    {
        if (lineRenderer == null) yield break;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        yield return new WaitForSeconds(tracerDuration);
        lineRenderer.enabled = false;
    }

    IEnumerator ShootCooldown()
    {
        canShoot = false;
        isShooting = true;
        yield return new WaitForSeconds(movementLockTime);
        isShooting = false;
        yield return new WaitForSeconds(fireDelay - movementLockTime);
        canShoot = true;
    }
}