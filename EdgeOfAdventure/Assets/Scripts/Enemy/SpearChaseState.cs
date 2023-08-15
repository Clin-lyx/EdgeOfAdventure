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
        currentEnemy.SetWaitTimeCounter(-1f);

    }

    public override void LogicUpdate()
    {
        if (currentEnemy.LostTimeCounter() <= 0 || (currentEnemy.isHurt && !currentEnemy.physicsCheck.OnGround())) 
            currentEnemy.SwitchState(NPCState.Patrol);

        // switiching facing when hits wall or hits an edge
        if ((currentEnemy.physicsCheck.TouchLeftWall() && currentEnemy.GetFaceDir() < 0) 
            || (currentEnemy.physicsCheck.TouchRightWall() && currentEnemy.GetFaceDir() > 0))
        {
            currentEnemy.transform.localScale = new Vector3(-currentEnemy.GetFaceDir(), 1, 1);
        } else if (!currentEnemy.physicsCheck.OnGround() && !currentEnemy.isHurt)
        {
            currentEnemy.transform.localScale = new Vector3(-currentEnemy.GetFaceDir(), 1, 1);
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
        currentEnemy.ResetLostTimer();
    }
}
