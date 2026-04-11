using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    private Camera mainCamera;
    public Vector2 aimDirection;
    void Start()
    {
        mainCamera=Camera.main;
    }

    void Update()
    {
        if (Mouse.current != null)
        {
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, mainCamera.nearClipPlane));
            mouseWorldPosition.z = 0f;
            Vector3 direction = mouseWorldPosition - transform.position;
            aimDirection = new Vector2(direction.x, direction.y).normalized;
            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }else if(direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
