using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerInputControl inputControl;
    
    private void Awake() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        inputControl = new PlayerInputControl();
        
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
        if(physicsCheck.onGround) anim.SetTrigger("pressJump");
    }

    private void Update() {
        SetAnimation();
    }

    private void SetAnimation(){
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("onGround", physicsCheck.onGround);
    }
    
}
