using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    private Camera mainCamera;
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
            Vector3 aimDirection = mouseWorldPosition - transform.position;
            if (aimDirection.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }else if(aimDirection.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
