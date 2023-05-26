using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Unity Components")]
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerInputControl inputControl;
    
    private void Awake() {

        // initialization
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        inputControl = new PlayerInputControl();
        
        // once space or south button on gamepad is pressed the jumpPrep animation starts
        inputControl.Gameplay.Jump.started += OnJumpDown;
        
    }

    private void OnEnable() {
        inputControl.Enable();
    }

    private void OnDisable() {
        inputControl.Disable();
    }

    private void OnJumpDown(InputAction.CallbackContext context)
    {
        // only if the player object is on the ground
        if(physicsCheck.onGround) anim.SetTrigger("pressJump");
    }

    private void Update() {
        SetAnimation();
    }

    private void SetAnimation(){
        
        // setting velocityX to trigger Run/Walk/Idle/RunStop animation 
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));

        // setting velocityY to trigger different animation in the blend tree
        anim.SetFloat("velocityY", rb.velocity.y);

        // if the object is not OnGround the animation in blend tree starts
        anim.SetBool("onGround", physicsCheck.onGround);
    }
    
}
