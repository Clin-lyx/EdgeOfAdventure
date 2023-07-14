using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        //Debug.Log("spear chase");
        currentEnemy.ChangeSpeedChase();
        currentEnemy.anim.SetBool("run", true);
        currentEnemy.waitTimeCounter = -1f;

        // resizing physics check and collider
        Spear spear = (Spear) currentEnemy;
        spear.collid.offset = spear.offsetWhenrun;
        spear.collid.size = spear.sizeWhenrun;
        spear.physicsCheck.rightOffset =  new Vector2((spear.sizeWhenrun.x + spear.offsetWhenrun.x) / 2 + 0.1f, 
            spear.sizeWhenrun.y / 2);
        spear.physicsCheck.leftOffset = new Vector2(-spear.physicsCheck.rightOffset.x,
            spear.physicsCheck.rightOffset.y);
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0 || (currentEnemy.isHurt && !currentEnemy.physicsCheck.OnGround())) 
            currentEnemy.SwitchState(NPCState.Patrol);

        // switiching facing when hits wall or hits an edge
        if ((currentEnemy.physicsCheck.TouchLeftWall() && currentEnemy.faceDir < 0) 
            || (currentEnemy.physicsCheck.TouchRightWall() && currentEnemy.faceDir > 0))
        {
            currentEnemy.transform.localScale = new Vector3(-currentEnemy.faceDir, 1, 1);
        } else if (!currentEnemy.physicsCheck.OnGround() && !currentEnemy.isHurt)
        {
            currentEnemy.transform.localScale = new Vector3(-currentEnemy.faceDir, 1, 1);
        }

        // if player is dead stop chasing
        if (currentEnemy.PlayerDead()) {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
    }

    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("run", false);
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
    }
}
