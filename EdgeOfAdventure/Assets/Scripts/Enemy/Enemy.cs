using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    [HideInInspector]public Animator anim;
    [HideInInspector]public PhysicsCheck physicsCheck;

    [Header("Arguments")]
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector]public float currentSpeed;
    public float hurtForce;
    public float faceDir;

    public Transform attacker;

    [Header("Timer")]
    public float waitTime;
    public float waitTimeCounter;
    public bool wait;

    [Header("State")]
    public bool isHurt;
    public bool isDead;

    //State machine
    private BaseState currentState;
    protected BaseState patrolState;
    protected BaseState chaseState;

    protected virtual void Awake() {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;

    }
    private void OnEnable()
    {
        //Patrolling when start the game
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    private void Update() {
        faceDir = rb.transform.localScale.x;

        currentState.LogicUpdate();
        TimeCounter();
    }

    private void FixedUpdate(){
        if (!isHurt && !isDead && !wait)
            Move();
        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }

    public virtual void Move() {
        rb.velocity = new Vector2(currentSpeed * faceDir * Time.deltaTime, rb.velocity.y);
    }

    //Timer
    public void TimeCounter()
    {
        //If touching the wall, wait
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
    }
    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;
        //Turn around
        if (attackTrans.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        if (attackTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        //Hurted and repelled
        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;

        //Start coroutine
        StartCoroutine(OnHurt(dir));
    }

    //Return the result of being attacked
    private IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        rb.AddForce(transform.up * hurtForce * 0.5f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.45f);
        isHurt = false;
    }

    public void OnDie()
    {
        gameObject.layer = 2;
        anim.SetBool("dead", true);
        isDead = true;
    }

    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }
}
