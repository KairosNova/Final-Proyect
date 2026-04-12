using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    private PlayerAim playerAim;
    float range = 10f;
   
    void Awake()
    {
        playerAim = GetComponent<PlayerAim>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
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
    }
}
