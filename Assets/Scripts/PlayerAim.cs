using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    private Camera mainCamera;
    public Vector2 aimDirection;
    public bool isAiming = false;
    [Header("cuerpo")]
    [SerializeField] private Transform torso;
    [SerializeField] private Transform arms;
    [SerializeField] private GameObject baseSprite; 
    [SerializeField] private float maxArmsAngle = 15f;
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
                ToggleAimParts(true);
                RotateArms();
            }else
            {
                ToggleAimParts(false);
                if (arms != null) arms.localRotation = Quaternion.identity;
            }
        }
    }
    private void RotateArms()
    {
        if (arms == null) return;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (transform.localScale.x < 0)
        {
            angle = 180f - angle;
            if (angle > 180f) angle -= 360f;
        }
        
        angle = Mathf.Clamp(angle, -maxArmsAngle, maxArmsAngle);
        arms.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void ToggleAimParts(bool aiming)
    {
        if (baseSprite != null) baseSprite.SetActive(!aiming); 
        if (torso != null) torso.gameObject.SetActive(aiming);
        if (arms != null) arms.gameObject.SetActive(aiming);
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
