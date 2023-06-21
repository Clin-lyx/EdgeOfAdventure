using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Enemy
{
    private Transform playerTransform;
    private float timer;
    private float attackTimer = 0.2f;
    

    protected override void Awake()
    {
        base.Awake();
        patrolState = new AxePatrolState();
        chaseState = new AxeChaseState();
        encounterState = new AxeEncounterState();
        timer = attackTimer;
    }


    public Transform PlayerTransformWhenChase () {
        return player.transform; 
    }

    public bool PlayerOnGround() {
        return player.GetComponent<PhysicsCheck>().onGround;
    }

    public void AttackRunDown() {
        if (!anim.GetBool("isAttack")){
            if (timer <= 0){
                anim.SetBool("isAttack", true);
                timer = attackTimer;
            } else {
                timer -= Time.deltaTime;
            }
        }
    }

    private void Chase() {

    }
    
    //Frame event to finish attack
    public void EndAttack()
    {
        anim.SetBool("isAttack", false);
        this.PatrolAfterPlayerDead();
    }

}
