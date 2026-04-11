using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    PlayerInput characterInput;
    Rigidbody2D characterRigidbody2D;
   
    
    Vector2 moveInput;
    public float speed = 5.0f; // Aumenté un poco la velocidad base

    Vector3 originalScale;

    void Start()
    {
        characterRigidbody2D = GetComponent<Rigidbody2D>();
        characterInput = GetComponent<PlayerInput>();
        
        
        originalScale = transform.localScale;

       
        characterRigidbody2D.gravityScale = 0f;
        characterRigidbody2D.freezeRotation = true; 
    }

    void Update()
    {
        moveInput = characterInput.actions["Move"].ReadValue<Vector2>();


        
        
    }

    void FixedUpdate()
    {
        
        characterRigidbody2D.linearVelocity = moveInput * speed;
    }
}