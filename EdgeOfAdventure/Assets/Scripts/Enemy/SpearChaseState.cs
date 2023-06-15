using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        Debug.Log("Chase");
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("run", true);

    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0) currentEnemy.SwitchState(NPCState.Patrol);

        //����⵽�泯ǽ�ļ������ǽ����ת��
        if (!currentEnemy.physicsCheck.onGround ||
                (currentEnemy.physicsCheck.touchLeftwall && currentEnemy.faceDir < 0) ||
                (currentEnemy.physicsCheck.touchRightwall && currentEnemy.faceDir > 0))
        {
            //����ǽʱ��û��waitֱ��ת��
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
