using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxePatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
    }

    public override void LogicUpdate()
    {
        //一旦发现player就切换到chase
        //When touching the wall, trun around
        if (!currentEnemy.physicsCheck.onGround ||
                (currentEnemy.physicsCheck.touchLeftwall && currentEnemy.faceDir < 0) ||
                (currentEnemy.physicsCheck.touchRightwall && currentEnemy.faceDir > 0))
        {
            currentEnemy.wait = true;
            //When touching the wall, idle
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
