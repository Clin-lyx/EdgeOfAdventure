using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeEncounterState : BaseState
{
    private Transform prevTrans;
    private static Transform final;

    public override void OnEnter(Enemy enemy)
    { 
        currentEnemy = enemy;
        Axe axe = (Axe) enemy;
        prevTrans = axe.PlayerTransformWhenChase();
        Debug.Log("Encounter");
        currentEnemy.currentSpeed = currentEnemy.encounterSpeed;
        currentEnemy.anim.SetBool("walk", false);
        currentEnemy.anim.SetBool("speedWalk", false);
        currentEnemy.anim.SetBool("foundPlayer", true);

        axe.isAttack = true;
        currentEnemy.anim.SetBool("isAttack", true);
    }

    public override void LogicUpdate()
    {
        
        Axe axe = (Axe) currentEnemy;
        Transform playerTransform = axe.PlayerTransformWhenChase();
        playerTransform ??= prevTrans;
        playerTransform ??= final;
        float diff  = axe.transform.position.x - playerTransform.position.x;
        int facing = diff < 0 ? 1 : -1;
        prevTrans = playerTransform;

        if (Mathf.Abs(diff) > 2f && !axe.isAttack)
        {
            currentEnemy.anim.SetBool("isAttack", false);
            currentEnemy.SwitchState(NPCState.Chase);  
        }

        if (!axe.isAttack)
        {
            currentEnemy.transform.localScale = new Vector3(facing, 1, 1);
        }
        


        
    }

    public override void PhysicsUpdate()
    {
        
    }
    public override void OnExit()
    {
        AxeEncounterState.final = prevTrans;
        currentEnemy.anim.SetBool("speedWalk", true);
        currentEnemy.anim.SetBool("foundPlayer", true);
        currentEnemy.anim.SetBool("isAttack", false);
    }
}
