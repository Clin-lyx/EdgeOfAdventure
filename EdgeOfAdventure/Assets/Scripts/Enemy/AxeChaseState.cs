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
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("speedWalk", true);
        currentEnemy.anim.SetBool("foundPlayer", true);
        currentEnemy.PatrolAfterPlayerDead();
        
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0) 
        {

            currentEnemy.SwitchState(NPCState.Patrol);
        }
        
        if (!currentEnemy.physicsCheck.onGround) {
            currentEnemy.anim.SetBool("speedWalk", false);
        } else {
            currentEnemy.anim.SetBool("speedWalk", true);
            
        }
        

        Axe axe = (Axe) currentEnemy;
        Transform playerTransform = axe.PlayerTransformWhenChase();
        
        float diff  = axe.transform.position.x - playerTransform.position.x;
        int facing = diff < 0 ? 1 : -1;
        

        if (Mathf.Abs(diff) <= 2f && axe.FoundPlayer()) {
            currentEnemy.SwitchState(NPCState.Encounter);
            
        } 

        currentEnemy.transform.localScale = new Vector3(facing, 1, 1);

        if (currentEnemy.physicsCheck.touchLeftwall || currentEnemy.physicsCheck.touchRightwall) {
            currentEnemy.anim.SetBool("speedWalk", false);
            
        }
        
    }

    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        
        currentEnemy.anim.SetBool("speedWalk", false);
        currentEnemy.anim.SetBool("foundPlayer", false);
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
    }
}
