using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        patrolState = new SpearPatrolState();
        chaseState = new SpearChaseState();
    }

    protected override void TimeCounter()
    {
        base.TimeCounter();
    }
}
