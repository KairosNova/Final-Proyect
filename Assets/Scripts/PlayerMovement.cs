using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    PlayerInput characterInput;
    Rigidbody2D characterRigidbody2D;
    Animator characterAnimator;
    PlayerAim playerAim;
    PlayerShoot playerShoot;
    
    Vector2 moveInput;
    public float speed = 5.0f;

    [SerializeField] float sprintSpeed = 9f; // ← sprint
    private bool isSprinting;

    

    void Start()
    {
        characterRigidbody2D = GetComponent<Rigidbody2D>();
        characterInput = GetComponent<PlayerInput>();
        playerAim = GetComponent<PlayerAim>();
        characterAnimator = GetComponentInChildren<Animator>();
        playerShoot = GetComponent<PlayerShoot>();
       
        characterRigidbody2D.gravityScale = 0f;
        characterRigidbody2D.freezeRotation = true; 
    }

    void Update()
    {
        moveInput = characterInput.actions["Move"].ReadValue<Vector2>();
        if (playerShoot != null && playerShoot.isShooting)
        {
            moveInput = Vector2.zero;
        }
        isSprinting = Keyboard.current.leftShiftKey.isPressed && moveInput.magnitude > 0;
        if (playerAim == null) return;
        if (characterAnimator != null)
        {
            characterAnimator.SetFloat("moveX" ,moveInput.magnitude);
            characterAnimator.SetBool("isSprinting", isSprinting);
        }
        if (playerAim != null && !playerAim.isAiming && moveInput.x != 0)
        {
            playerAim.FlipSprite(moveInput.x);
        }
        
        
    }

    void FixedUpdate()
    {
        float currentSpeed = isSprinting ? sprintSpeed : speed;
        characterRigidbody2D.linearVelocity = moveInput * currentSpeed;
    }
}