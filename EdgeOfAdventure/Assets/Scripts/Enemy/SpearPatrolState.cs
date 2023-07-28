using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        //Debug.Log("spear patrol");
        currentEnemy = enemy;
        currentEnemy.ChangeSpeedIdle();

        // resizing collider and physics check
        Spear spear = (Spear) currentEnemy;
        spear.ResetCollidToIdle();
        spear.physicsCheck.rightOffset =  new Vector2((spear.sizeWhenidle.x + spear.offsetWhenidle.x) / 2, 
            spear.sizeWhenidle.y / 2);
        spear.physicsCheck.leftOffset = new Vector2(-spear.physicsCheck.rightOffset.x,
            spear.physicsCheck.rightOffset.y);
    }

    public override void LogicUpdate()
    {
        //If the player is found, start chasing
        if (currentEnemy.FoundPlayer() && currentEnemy.PlayerOnGround())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
        //If touching the wall, trun around
        if (!currentEnemy.physicsCheck.OnGround() ||
                (currentEnemy.physicsCheck.TouchLeftWall() && currentEnemy.GetFaceDir() < 0) ||
                (currentEnemy.physicsCheck.TouchRightWall() && currentEnemy.GetFaceDir() > 0))
        {
            currentEnemy.SetWait(true);
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
    }
}
