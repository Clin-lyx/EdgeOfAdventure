using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxePatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        //Debug.Log("Patrol");
        currentEnemy.ChangeSpeedIdle();
        currentEnemy.anim.SetBool("speedWalk", false);
        if (currentEnemy.TouchingWalls()) {
            currentEnemy.anim.SetBool("walk", false);  
        } else {
            currentEnemy.anim.SetBool("walk", true);
        }
    }

    public override void LogicUpdate()
    {
        //If the player is found, start chasing
        
        if (currentEnemy.FoundPlayer() && currentEnemy.physicsCheck.OnGround() && currentEnemy.PlayerOnGround()) {
            currentEnemy.SwitchState(NPCState.Chase);
        } 
        //When touching the wall, trun around
        if (!currentEnemy.physicsCheck.OnGround() ||
                (currentEnemy.physicsCheck.TouchLeftWall() && currentEnemy.GetFaceDir() < 0) ||
                (currentEnemy.physicsCheck.TouchRightWall() && currentEnemy.GetFaceDir() > 0))
        {
            currentEnemy.SetWait(true);
            //When touching the wall, idle
            currentEnemy.anim.SetBool("walk", false);
            currentEnemy.anim.SetBool("speedWalk", false);
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
    }
}
