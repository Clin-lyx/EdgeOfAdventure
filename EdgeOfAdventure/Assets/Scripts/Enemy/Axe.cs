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

    protected override void TimeCounter() {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(-faceDir, 1, 1);
            }
        }

        if (PlayerTransformWhenChase() == null && lostTimeCounter > 0)
        {
            lostTimeCounter -= Time.deltaTime;
        }
    }

    public Transform PlayerTransformWhenChase () {
        Transform temp = Physics2D.BoxCast(transform.position + (Vector3)centerOffset, 
            checkSize, 0, new Vector3(faceDir, 0, 0), checkDistance, attackLayer).transform;
        temp ??= Physics2D.BoxCast(transform.position + (Vector3)centerOffset, 
            checkSize, 0, new Vector3(faceDir * -1, 0, 0), checkDistance, attackLayer).transform;
        return temp;
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
