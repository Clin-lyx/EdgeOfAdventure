using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        Debug.Log("spear chase");
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("run", true);
        currentEnemy.PatrolAfterPlayerDead();

    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0 || (currentEnemy.isHurt && !currentEnemy.physicsCheck.onGround)) 
            currentEnemy.SwitchState(NPCState.Patrol);

        if ((currentEnemy.physicsCheck.touchLeftwall && currentEnemy.faceDir < 0) || (currentEnemy.physicsCheck.touchRightwall && currentEnemy.faceDir > 0))
        {
            currentEnemy.transform.localScale = new Vector3(-currentEnemy.faceDir, 1, 1);
        } else if (!currentEnemy.physicsCheck.onGround && !currentEnemy.isHurt)
        {
            currentEnemy.transform.localScale = new Vector3(-currentEnemy.faceDir, 1, 1);
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
