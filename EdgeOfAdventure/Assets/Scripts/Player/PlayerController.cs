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
    private float delaytime;
    private bool toDojump = false;
    
    

    private void Awake() {
        // recieving info from keyboard / controller input and rigibody
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();
        physicsCheck = GetComponent<PhysicsCheck>();

        // setting running speed
        runSpeed = speed;

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

        // Jump function is in Update function because we want the Jump to has some level of delay 
        // after pressing space button on keyboard or south button on gamepad  
        Jump();
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
    
    private void Jump() {
        if (Gamepad.all.Count > 0) { // if the user is playing with gamepad
            if (Gamepad.current.buttonSouth.wasPressedThisFrame) {
                
                // making sure the player game object is on the ground
                // as well as making sure that is already a jump movement not yet processed
                if(physicsCheck.onGround && !toDojump) { 
                
                // jump will be performed after 0.1f
                delaytime = Time.time + 0.01f;
                toDojump = true;
                
                }
            }
        } else {
            
            // making sure the player game object is on the ground
            // as well as making sure that is already a jump movement not yet processed
            if (Keyboard.current.spaceKey.wasPressedThisFrame) {
                if(physicsCheck.onGround && !toDojump) {
                
                // jump will be performed after 0.1f
                delaytime = Time.time + 0.1f;
                toDojump = true;
                
                }
            }
        }

        if (toDojump && Time.time >= delaytime) {
            toDojump = false;
            
            // adding force to the object so that it can attain an upward velocity
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    
}
