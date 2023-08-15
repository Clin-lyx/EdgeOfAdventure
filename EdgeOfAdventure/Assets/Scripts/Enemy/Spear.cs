using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Enemy
{
    private CapsuleCollider2D collid;
    

    protected override void Awake()
    {
        base.Awake();
        patrolState = new SpearPatrolState();
        chaseState = new SpearChaseState();
        
    }

}
