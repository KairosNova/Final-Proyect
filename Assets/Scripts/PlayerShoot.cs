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
    private bool canShoot = true;
    public bool isShooting = false;
    private Animator animator;

   
    void Awake()
    {
        playerAim = GetComponent<PlayerAim>();
        animator = GetComponentInChildren<Animator>();
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
        
    }
    IEnumerator ShootCooldown()
    {
        canShoot = false;
        isShooting = true;
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(movementLockTime);
        isShooting = false;
        yield return new WaitForSeconds(fireDelay - movementLockTime);
        canShoot = true;
    }
}
