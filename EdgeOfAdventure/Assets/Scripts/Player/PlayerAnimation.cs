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

        // if the player is not OnGround the animation in blend tree starts
        anim.SetBool("onGround", physicsCheck.OnGround());

        // if the player is crouching
        anim.SetBool("isCrouch", playerController.isCrouch);

        // if the player is dead
        anim.SetBool("isDead", playerController.isDead);

        // if the player tries to attack
        anim.SetBool("isAttack", playerController.isAttack);

        // if player triggers dash
        anim.SetBool("isDash", playerController.isDash);

        // if holding S key
        anim.SetBool("holdS", playerController.holdS);

        // if using skill
        anim.SetBool("isSkill", playerController.isSkill);

    }

    public void PlayerHurt() {
        anim.SetTrigger("isHurt");
    }

    public void PlayAttack() {
        anim.SetTrigger("attack");
    }

}
