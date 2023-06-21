using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeEncounterState : BaseState
{
    
    public override void OnEnter(Enemy enemy)
    { 
        currentEnemy = enemy;
        Axe axe = (Axe) enemy;
        Debug.Log("Encounter");
        currentEnemy.currentSpeed = currentEnemy.encounterSpeed;
        currentEnemy.anim.SetBool("walk", false);
        currentEnemy.anim.SetBool("speedWalk", false);
        currentEnemy.anim.SetBool("foundPlayer", true);
        
        

    }

    public override void LogicUpdate()
    {
        
        Animator anim = currentEnemy.GetComponent<Animator>();
        Axe axe = (Axe) currentEnemy;
        axe.PatrolAfterPlayerDead();

        Transform playerTransform = axe.PlayerTransformWhenChase();
        float diff  = axe.transform.position.x - playerTransform.position.x;
        int facing = diff < 0 ? 1 : -1;
        
        axe.AttackRunDown();


        if (!anim.GetBool("isAttack"))
        {
            currentEnemy.transform.localScale = new Vector3(facing, 1, 1);
        }
        

        if (Mathf.Abs(diff) > 2f && !anim.GetBool("isAttack"))
        {
            currentEnemy.waitTimeCounter = 0;
            currentEnemy.anim.SetBool("isAttack", false);
            currentEnemy.SwitchState(NPCState.Chase);
        } 

        if (Mathf.Abs(diff) < 2f && axe.PlayerOnGround() && !axe.FoundPlayer() && !anim.GetBool("isAttack")) {
            currentEnemy.waitTimeCounter = 0;
            currentEnemy.anim.SetBool("isAttack", false);
            currentEnemy.SwitchState(NPCState.Chase);
        }

        if (currentEnemy.lostTimeCounter <= 0) 
        {
            currentEnemy.waitTimeCounter = 0;
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        
    }

    public override void PhysicsUpdate()
    {
        
    }
    public override void OnExit()
    {   
        
        currentEnemy.anim.SetBool("speedWalk", true);
        currentEnemy.anim.SetBool("foundPlayer", true);
        currentEnemy.anim.SetBool("isAttack", false);

    }
}
