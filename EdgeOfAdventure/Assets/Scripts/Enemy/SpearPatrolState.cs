using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }

    public override void LogicUpdate()
    {
        //If the player is found, start chasing
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
        //If touching the wall, trun around
        if (!currentEnemy.physicsCheck.onGround ||
                (currentEnemy.physicsCheck.touchLeftwall && currentEnemy.faceDir < 0) ||
                (currentEnemy.physicsCheck.touchRightwall && currentEnemy.faceDir > 0))
        {
            currentEnemy.wait = true;
            //If touching the wall, idle
            currentEnemy.anim.SetBool("walk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("walk", true);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("walk", false);
        Debug.Log("Exit");
    }
}
