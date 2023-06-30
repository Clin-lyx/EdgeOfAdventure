using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
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
    public float dashDistance;
    public float dashSpeed;
    public float dashCooldown;
    private float dashTimer;
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
    public bool isDash;
    
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
            if(physicsCheck.OnGround()) {
                speed = walkSpeed;
            }
        };

        // Back to running once left shift is released
        inputControl.Gameplay.Walk.canceled += ctx => {
            if (physicsCheck.OnGround()){
                speed = runSpeed;
            }
        };

        // Player Dash
        inputControl.Gameplay.Dash.started += Dash;

        // Attack
        inputControl.Gameplay.Attack.started += PlayerAttack;

        // object layer
        gameObject.layer = LayerMask.NameToLayer("Player");
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

        // Dash cool down
        dashTimer -= Time.deltaTime;
    }

    private void FixedUpdate() {
        if (!isHurt && !isAttack) Move();
    }

    private void Move() {
        isCrouch = physicsCheck.OnGround() && inputDirection.y < -0.5f;
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
        if (physicsCheck.OnGround() && !isCrouch && !isAttack) 
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            

        // Jump if the character is on the ground and not attack and crouching
        if (physicsCheck.OnGround() && isCrouch && !isAttack) 
            rb.AddForce(transform.up * jumpWhencrouch, ForceMode2D.Impulse);
    }
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        // Player only throws attack when standing on Ground
        if (!isCrouch && !isHurt) {
            rb.velocity = new Vector2(0, rb.velocity.y);
            playerAnimation.PlayAttack();
            isAttack = true;
        }

    }

    private void Dash(InputAction.CallbackContext context)
    {
        // Player can only dash when on ground not throwing attacks, and the dash is cooled down
        if (!isDash && physicsCheck.OnGround() && !isAttack && dashTimer <= 0f) {
            isDash = true;

            // target position after dash
            var targetPos = new Vector2(transform.position.x + dashDistance * transform.localScale.x, 
                transform.position.y);
            
            // avoid being attack from enemies;
            gameObject.layer = LayerMask.NameToLayer("Enemy");

            StartCoroutine(TriggerDash(targetPos));
            dashTimer = dashCooldown;
       }
    }

    private IEnumerator TriggerDash(Vector2 target) 
    {
        float faceDir = transform.localScale.x;
        do {
            
            yield return null;

            // if player is on air the dash is terminates.
            if (!physicsCheck.OnGround()) {
                break;
            }
            
            // if player turns the dash terminates
            if (faceDir != transform.localScale.x) break;

            // if player hits wall the dash terminates
            if (physicsCheck.TouchLeftWall() && transform.localScale.x < 0f 
                || physicsCheck.TouchRightWall() && transform.localScale.x > 0f) {
                break;
            }
            
            // moving towards target position
            rb.MovePosition(new Vector2(transform.position.x + transform.localScale.x * dashSpeed,
                transform.position.y));
            
            
            
        } while (MathF.Abs(target.x - transform.position.x) > 0.5f);

        isDash = false;
        // turns back layer once the player is done dashing
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void GetHurt(Attack attacker) {
        isHurt = true;
        
        // reset player velocity so that it wont fly away after receiving damage frequently 
        rb.velocity = Vector2.zero;
        
        // direction of attack
        Vector2 dir = new Vector2(rb.position.x - attacker.transform.position.x, 0).normalized;

        //Adding force on player
        rb.AddForce(dir * reactionForce, ForceMode2D.Impulse);
        rb.AddForce(transform.up * reactionForce, ForceMode2D.Impulse);
    }

    public void PlayerDead() {
        isDead = true;
        inputControl.Gameplay.Disable();
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }    

    private void checkState() {
        coll.sharedMaterial = physicsCheck.OnGround() ? normal : wall;
    }
}
