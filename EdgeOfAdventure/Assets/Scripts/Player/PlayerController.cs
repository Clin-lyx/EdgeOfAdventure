using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Unity Components")]
    private PlayerInputControl inputControl;
    private Vector2 inputDirection;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    
    [Header("Fundamental Arguments")]
    public float speed;
    public float runSpeed;
    public float walkSpeed => speed / 3f;
    public float jumpForce;
    
    private void Awake() {
        // recieving info from keyboard / controller input and rigibody
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();
        physicsCheck = GetComponent<PhysicsCheck>();

        // setting running speed
        runSpeed = speed;

        // player jump after press jump button
        inputControl.Gameplay.Jump.started += Jump;

        // Walking performed when left shift is being pressed
        inputControl.Gameplay.Walk.performed += ctx => {
            if(physicsCheck.onGround) {
                speed = walkSpeed;
            }
        };

        // Back to running once left shift is released
        inputControl.Gameplay.Walk.canceled += ctx => {
            if (physicsCheck.onGround){
                speed = runSpeed;
            }
        };
    }

    

    private void OnEnable() {
        inputControl.Enable();
    }

    private void OnDisable() {
        inputControl.Disable();
    }

    private void Update() {
        // consistently reading input from input devices.
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);

        // maintaining the value of facing Direction if no input is given. 
        int temp = (int) transform.localScale.x;
        int faceDir = inputDirection.x < 0 ? -1 
            : inputDirection.x > 0 ? 1 : temp;

        // establishing new facing direction.
        transform.localScale = new Vector3(faceDir, transform.localScale.y, transform.localScale.z);
    }
    
    private void Jump(InputAction.CallbackContext context)
    {
        // Jump only if the character is on the ground
        if (physicsCheck.onGround) rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
}
