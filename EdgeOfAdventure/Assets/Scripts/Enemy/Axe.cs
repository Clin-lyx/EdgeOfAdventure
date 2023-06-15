using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Enemy
{
    private Transform playerTransform;
    protected override void Awake()
    {
        base.Awake();
        patrolState = new AxePatrolState();
        chaseState = new AxeChaseState();
        encounterState = new AxeEncounterState();
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

    private void Chase() {

    }
    
}
