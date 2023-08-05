using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeChaseState : BaseState
{ 

    public override void OnEnter(Enemy enemy)
    { 
        currentEnemy = enemy;
        Axe axe = (Axe) enemy;
        //Debug.Log("Chase");
        currentEnemy.SetWaitTimeCounter(-1f);
        currentEnemy.ChangeSpeedChase();
        currentEnemy.anim.SetBool("speedWalk", true);
        currentEnemy.anim.SetBool("foundPlayer", true);        
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.LostTimeCounter() <= 0 || currentEnemy.PlayerDead()) 
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }

        if (currentEnemy.TouchingWalls()) {
            currentEnemy.anim.SetBool("speedWalk", false);
            currentEnemy.anim.SetBool("walk", false);
        } else {
            if (!currentEnemy.physicsCheck.OnGround()) {
                currentEnemy.anim.SetBool("speedWalk", false);
            } else {
                currentEnemy.anim.SetBool("speedWalk", true);
            }
        }
        
        
        Axe axe = (Axe) currentEnemy;

        Transform playerTransform = currentEnemy.PlayerTransformWhenChase();
        
        // if player is within range, switch to encounter state and attack player
        float diff  = currentEnemy.transform.position.x - playerTransform.position.x;
        if (Mathf.Abs(diff) <= axe.Range() && currentEnemy.FoundPlayer()) {
            currentEnemy.SwitchState(NPCState.Encounter);
        } 
        
        float facing = diff > 0 ? -1f : diff < 0 ? 1f : currentEnemy.transform.localScale.x;
        currentEnemy.transform.localScale = new Vector3(facing, 1f, 1f);
        
    }

    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("speedWalk", false);
        currentEnemy.anim.SetBool("foundPlayer", currentEnemy.FoundPlayer());
        currentEnemy.ResetLostTimer();
    }
}
