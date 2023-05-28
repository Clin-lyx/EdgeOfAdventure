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
    private PlayerController playerController;
    
    private void Awake() {

        // initialization
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();

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

        // if the object is crouching
        anim.SetBool("isCrouch", playerController.isCrouch);
    }
    
}
