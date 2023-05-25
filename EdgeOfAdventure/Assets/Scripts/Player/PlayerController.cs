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
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();
        physicsCheck = GetComponent<PhysicsCheck>();
        
        runSpeed = speed;

        inputControl.Gameplay.Walk.performed += ctx => {
            if(physicsCheck.onGround) {
                speed = walkSpeed;
            }
        };
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
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        Jump();
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        int temp = (int) transform.localScale.x;
        int faceDir = inputDirection.x < 0 ? -1 
            : inputDirection.x > 0 ? 1 : temp;
        transform.localScale = new Vector3(faceDir, transform.localScale.y, transform.localScale.z);
    }
    
    private void Jump() {
        if (Gamepad.all.Count > 0) {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame) {
                if(physicsCheck.onGround && !toDojump) {
                delaytime = Time.time + 0.1f;
                toDojump = true;
                }
            }
        } else {
            if (Keyboard.current.spaceKey.wasPressedThisFrame) {
                if(physicsCheck.onGround && !toDojump) {
                delaytime = Time.time + 0.1f;
                toDojump = true;
                }
            }
        }

        if (toDojump && Time.time >= delaytime) {
            toDojump = false;
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    
}
