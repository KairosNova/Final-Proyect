using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    private PlayerAim playerAim;
    [SerializeField]float range = 10f;
    [SerializeField] float fireDelay = 1.5f;
    private bool canShoot = true;
    //private Animator animator;
   
    void Awake()
    {
        playerAim = GetComponent<PlayerAim>();
        //animator = GetComponent<Animator>(); 
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
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, range);
        Color rayColor = hit.collider != null ? Color.yellow : Color.red;
        Debug.DrawRay(firePoint.position, direction * range, rayColor, 1f);
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            Destroy(hit.collider.gameObject);
        }
        
        StartCoroutine(ShootCooldown());
        
    }
    IEnumerator ShootCooldown()
    {
        canShoot = false;
        // animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(fireDelay);
        // animator.SetTrigger("Reload");
        canShoot = true;
    }
}
