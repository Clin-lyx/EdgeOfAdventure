using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        patrolState = new AxePatrolState();
    }
}
