using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Enemy
{
    private CapsuleCollider2D collid;
    

    public Vector2 offsetWhenrun;
    public Vector2 sizeWhenrun;
    public Vector2 offsetWhenidle;
    public Vector2 sizeWhenidle;

    protected override void Awake()
    {
        base.Awake();
        patrolState = new SpearPatrolState();
        chaseState = new SpearChaseState();

        collid = GetComponent<CapsuleCollider2D>();
        
        offsetWhenidle = new Vector2(0f, 1.312f);
        sizeWhenidle = new Vector2(1.75f, 2.625f);
        offsetWhenrun = new Vector2(0.23f, 1.312f);
        sizeWhenrun = new Vector2(2.21f, 2.625f);
    }

    public void ResetCollidToIdle() {
        collid.offset = offsetWhenidle;
        collid.size = sizeWhenidle;
    }

    public void ResetCollidToRun() {
        collid.offset = offsetWhenrun;
        collid.size = sizeWhenrun;
    }
}
