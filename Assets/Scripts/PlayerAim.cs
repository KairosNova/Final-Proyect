using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    private Camera mainCamera;
    public Vector2 aimDirection;
    public bool isAiming = false;
    void Start()
    {
        mainCamera=Camera.main;
    }

    void Update()
    {
        if (Mouse.current != null)
        {
            isAiming = Mouse.current.rightButton.isPressed;
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, mainCamera.nearClipPlane));
            mouseWorldPosition.z = 0f;
            Vector3 direction = mouseWorldPosition - transform.position;
            aimDirection = new Vector2(direction.x, direction.y).normalized;
           
            Debug.DrawRay(transform.position, (Vector3)aimDirection * 2f, Color.green);
            if (isAiming)
            {
                FlipSprite(aimDirection.x);
            }
        }
    }
    public void FlipSprite(float directionX)
    {
        if (directionX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
            
        else if (directionX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
            
    }
}
