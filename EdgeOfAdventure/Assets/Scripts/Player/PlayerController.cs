using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Event Listeners")]
    [SerializeField]private SceneLoadEventSO sceneLoadEvent;
    [SerializeField]private VoidEventSO afterSceneloadedEvent;
    [SerializeField]private VoidEventSO loadDataEvent;
    [SerializeField]private VoidEventSO backToMenuEvent;
    [SerializeField]private VoidEventSO NewGameEvent;

    [Header("Components")]
    private PlayerInputControl inputControl;
    private Vector2 inputDirection;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D cap;
    private PlayerAnimation playerAnimation;


    [Header("Fundamental Arguments")]
    [SerializeField]private float speed;
    [SerializeField]private float runSpeed;
    [SerializeField]private float walkSpeed => speed / 3f;
    [SerializeField]private float jumpForce;
    [SerializeField]private float jumpWhencrouch;
    [SerializeField]private float dashDistance;
    [SerializeField]private float dashSpeed;
    [SerializeField]private float dashCooldown;
    private float dashTimer;
    [SerializeField]private float perfectDodge;
    private float dodgeTimer;
    private Vector2 originalOffset;
    private Vector2 orginalSize;
    private CapsuleCollider2D coll;

    [Header("physics material")]
    [SerializeField]private PhysicsMaterial2D normal;
    [SerializeField]private PhysicsMaterial2D wall;

    [Header("Player States")]
    public bool isHurt;
    public bool isDead;
    public bool isCrouch;
    public bool isAttack;
    public bool isDash;
    public bool holdS;
    public bool holdW;
    public bool isSkill;
    public bool perfect;

    private void Awake()
    {
        // Initialization
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();
        physicsCheck = GetComponent<PhysicsCheck>();
        cap = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        coll = GetComponent<CapsuleCollider2D>();
        inputControl.Enable();

        // setting running speed
        runSpeed = speed;

        // player jump after press jump button
        inputControl.Gameplay.Jump.started += Jump;

        // getting the capsule collider arguments when standing up
        orginalSize = cap.size;
        originalOffset = cap.offset;

        // Walking performed when left shift is being pressed
        inputControl.Gameplay.Walk.performed += ctx => {
            if (physicsCheck.OnGround())
            {
                speed = walkSpeed;
            }
        };

        // Back to running once left shift is released
        inputControl.Gameplay.Walk.canceled += ctx => {
            if (physicsCheck.OnGround())
            {
                speed = runSpeed;
            }
        };

        // Player Dash
        inputControl.Gameplay.Dash.started += Dash;

        // Attack
        inputControl.Gameplay.Attack.started += PlayerAttack;

        // Skill
        inputControl.Gameplay.Skill.started += PlayerSkill;

        // object layer
        gameObject.layer = LayerMask.NameToLayer("Player");
    }



    private void OnEnable()
    {
        sceneLoadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneloadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        NewGameEvent.OnEventRaised += OnLoadDataEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneloadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
        NewGameEvent.OnEventRaised -= OnLoadDataEvent;
    }


    private void Update()
    {
        // consistently reading input from input devices.
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();

        // checking which physics material to use
        checkState();

        // Dash cool down
        if (dashTimer > 0)
            dashTimer -= Time.deltaTime;

        // Perfect Dodge
        if (dodgeTimer > 0)
            dodgeTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isAttack) Move();

        holdS = inputDirection.y < -0.1f;
        holdW = inputDirection.y > 0.1f;
    }

    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();
    }

    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    private void OnAfterSceneLoadedEvent()
    {
        inputControl.Gameplay.Enable();
    }


    private void Move()
    {
        isCrouch = physicsCheck.OnGround() && Input.GetKey(KeyCode.S) && !isAttack && !isSkill && !isHurt && !isDash;
        if (isCrouch)
        {
            // adjusting collider size when crouching and immediately goes stop
            cap.size = new Vector2(1.4f, 1.7f);
            cap.offset = new Vector2(0.1f, 0.9f);
            rb.velocity = new Vector2(0f, rb.velocity.y);

        }
        else
        {
            // adjusting collider size when back at standing
            cap.size = orginalSize;
            cap.offset = originalOffset;
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        }
        // maintaining the value of facing Direction if no input is given. 
        int temp = (int)transform.localScale.x;
        int faceDir = inputDirection.x < 0 ? -1
            : inputDirection.x > 0 ? 1 : temp;

        // establishing new facing direction.
        transform.localScale = new Vector3(faceDir, transform.localScale.y, transform.localScale.z);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        // Jump if the character is on the ground and not attack and not crouch
        if (physicsCheck.OnGround() && !holdS && !isAttack)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);


        // Jump if the character is on the ground and not attack and crouching
        if (physicsCheck.OnGround() && holdS && !isAttack)
            rb.AddForce(transform.up * jumpWhencrouch, ForceMode2D.Impulse);
    }
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        // Player only throws attack when standing on Ground
        if (!isHurt && !isDash)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            isAttack = true;
            playerAnimation.PlayAttack();

            isSkill = isAttack && (holdW || holdS);
        }
    }

    private void PlayerSkill(InputAction.CallbackContext context)
    {
        // Player only throws attack when standing on Ground
        if (!isHurt && physicsCheck.OnGround())
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            isAttack = true;
            isSkill = true;
            playerAnimation.PlaySkill();
            dodgeTimer = perfectDodge;
        }

    }

    private void Dash(InputAction.CallbackContext context)
    {
        // Player can only dash when on ground not throwing attacks, and the dash is cooled down
        if (!isDash && physicsCheck.OnGround() && !isAttack && dashTimer <= 0f && !physicsCheck.TouchLeftWall()
            && !physicsCheck.TouchRightWall())
        {
            isDash = true;
            isCrouch = false;

            // target position after dash
            var targetPos = new Vector2(transform.position.x + dashDistance * transform.localScale.x,
                transform.position.y);

            // avoid being attack from enemies;
            gameObject.layer = LayerMask.NameToLayer("EnemyWeapon");

            StartCoroutine(TriggerDash(targetPos));
            dashTimer = dashCooldown;
        }
    }

    private IEnumerator TriggerDash(Vector2 target)
    {
        float faceDir = transform.localScale.x;
        do
        {

            yield return null;

            // if player is on air the dash is terminates.
            if (!physicsCheck.OnGround())
            {
                break;
            }

            // if player turns the dash terminates
            if (faceDir != transform.localScale.x) break;

            // if player hits wall the dash terminates
            if (physicsCheck.TouchLeftWall() && transform.localScale.x < 0f
                || physicsCheck.TouchRightWall() && transform.localScale.x > 0f)
            {
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

    public void GetHurt(Attack attacker)
    {
        isHurt = true;

        // reset player velocity so that it wont fly away after receiving damage frequently 
        rb.velocity = Vector2.zero;

        // direction of attack
        Vector2 dir = new Vector2(rb.position.x - attacker.transform.position.x, 0).normalized;

        //Adding force on player
        rb.AddForce(dir * attacker.ForceX(), ForceMode2D.Impulse);
        rb.AddForce(transform.up * attacker.ForceY(), ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    private void checkState()
    {
        coll.sharedMaterial = physicsCheck.OnGround() ? normal : wall;
    }

    public void SetInputControlToDisable()
    {
        inputControl.Disable();
    }

    public bool IsPerfectDodge()
    {
        return dodgeTimer > 0;
    }

    public Vector2 InputDirection() {
        return inputDirection;
    }
}
