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
    public Vector2 inputDirection;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D cap;
    private PlayerAnimation playerAnimation;

    
    [Header("Fundamental Arguments")]
    public float speed;
    public float runSpeed;
    public float walkSpeed => speed / 3f;
    public float jumpForce;
    public float jumpWhencrouch;
    public float reactionForce; 
    private Vector2 originalOffset;
    private Vector2 orginalSize;
    private CapsuleCollider2D coll;
    
    [Header("physics material")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    [Header("Player States")]
    public bool isHurt;
    public bool isDead;
    public bool isCrouch;
    public bool isAttack;
    
    private void Awake() {
        // Initialization
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();
        physicsCheck = GetComponent<PhysicsCheck>();
        cap = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        coll = GetComponent<CapsuleCollider2D>();
        
        // setting running speed
        runSpeed = speed;

        // player jump after press jump button
        inputControl.Gameplay.Jump.started += Jump;

        // getting the capsule collider arguments when standing up
        orginalSize = cap.size;
        originalOffset = cap.offset;

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

        // Attack
        inputControl.Gameplay.Attack.started += PlayerAttack;
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

        // checking which physics material to use
        checkState();
    }

    private void FixedUpdate() {
        if (!isHurt && !isAttack) Move();
    }

    private void Move() {
        isCrouch = physicsCheck.onGround && inputDirection.y < -0.5f;
        if (isCrouch) {
            // adjusting collider size when crouching and immediately goes stop
            cap.size = new Vector2(1.4f, 1.7f);
            cap.offset = new Vector2(0.1f, 0.9f);
            rb.velocity = new Vector2(0f, rb.velocity.y);
            
        } else {
            // adjusting collider size when back at standing
            cap.size = orginalSize;
            cap.offset = originalOffset;
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        }
        // maintaining the value of facing Direction if no input is given. 
        int temp = (int) transform.localScale.x;
        int faceDir = inputDirection.x < 0 ? -1 
            : inputDirection.x > 0 ? 1 : temp;

        // establishing new facing direction.
        transform.localScale = new Vector3(faceDir, transform.localScale.y, transform.localScale.z);
    }
    
    private void Jump(InputAction.CallbackContext context)
    {
        // Jump if the character is on the ground and not attack and not crouch
        if (physicsCheck.onGround && !isCrouch && !isAttack) 
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        // Jump if the character is on the ground and not attack and crouching
        if (physicsCheck.onGround && isCrouch && !isAttack) 
            rb.AddForce(transform.up * jumpWhencrouch, ForceMode2D.Impulse);
    }
    private void PlayerAttack(InputAction.CallbackContext context)
    {

        if (!isCrouch && physicsCheck.onGround) {
            rb.velocity = new Vector2(0, rb.velocity.y);
            playerAnimation.PlayAttack();
            isAttack = true;
        }

    }

    public void GetHurt(Transform attack) {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(rb.position.x - attack.position.x, 0).normalized;

        rb.AddForce(dir * reactionForce, ForceMode2D.Impulse);
    }

    public void PlayerDead() {
        isDead = true;
        inputControl.Gameplay.Disable();
    }    

    private void checkState() {
        coll.sharedMaterial = physicsCheck.onGround ? normal : wall;
    }
}
