using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    { 
        currentEnemy = enemy;
        Axe axe = (Axe) enemy;
        Debug.Log("Chase");
        currentEnemy.waitTimeCounter = -1f;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("speedWalk", true);
        currentEnemy.anim.SetBool("foundPlayer", true);        
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0 || currentEnemy.PlayerDead()) 
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }

        if (currentEnemy.TouchingWalls()) {
            currentEnemy.anim.SetBool("speedWalk", false);
            currentEnemy.anim.SetBool("walk", false);
        } else {
            if (!currentEnemy.physicsCheck.onGround) {
                currentEnemy.anim.SetBool("speedWalk", false);
            } else {
                currentEnemy.anim.SetBool("speedWalk", true);
            }
        }
        
        Axe axe = (Axe) currentEnemy;
        Transform playerTransform = axe.PlayerTransformWhenChase();
        
        // if player is within 2f, switch to encounter state and attack player
        float diff  = axe.transform.position.x - playerTransform.position.x;
        if (Mathf.Abs(diff) <= 2f && axe.FoundPlayer()) {
            currentEnemy.SwitchState(NPCState.Encounter);
        } 
        
    }

    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("speedWalk", false);
        currentEnemy.anim.SetBool("foundPlayer", currentEnemy.FoundPlayer());
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
    }
}
