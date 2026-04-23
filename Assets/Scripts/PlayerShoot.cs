using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    private PlayerAim playerAim;
    [SerializeField]float range = 10f;
    [SerializeField] float fireDelay = 1.5f;
    [SerializeField] float movementLockTime = 0.4f; // El tiempo que dura el retroceso
    [SerializeField] int damage = 100;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float tracerDuration = 0.01f;
    private bool canShoot = true;
    public bool isShooting = false;
    [SerializeField] private Animator armsAnimator;

   
    void Awake()
    {
        playerAim = GetComponent<PlayerAim>();
        armsAnimator = GetComponentInChildren<Animator>();
        
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && playerAim.isAiming && canShoot)
        {
            Shoot();
        }
    }
    void Shoot()
    {
        Vector2 direction = playerAim.aimDirection;
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, range, enemyLayer);
        Debug.DrawRay(firePoint.position, direction * range, hit.collider != null ? Color.green : Color.red, 0.5f);
        if (hit.collider != null)
        {
            Health enemyHealth = hit.collider.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
        
        StartCoroutine(ShootCooldown());
        Vector2 endPoint = hit.collider != null ? hit.point : (Vector2)firePoint.position + (direction * range);
        StartCoroutine(ShowTracer(firePoint.position, endPoint));
        
    }
    IEnumerator ShowTracer(Vector3 start, Vector3 end)
    {
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
        if (armsAnimator != null) armsAnimator.SetTrigger("Shoot");
        yield return new WaitForSeconds(movementLockTime);
        isShooting = false;
        yield return new WaitForSeconds(fireDelay - movementLockTime);
        canShoot = true;
    }
}
