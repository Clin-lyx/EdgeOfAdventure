using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    [HideInInspector]public Animator anim;
    [HideInInspector]public PhysicsCheck physicsCheck;

    public GameObject player;

    [Header("Arguments")]
    public float normalSpeed;
    public float chaseSpeed;
    public float encounterSpeed = 0;
    [HideInInspector]public float currentSpeed;
    
    //public float hurtForce;
    public Attack attack;
    public float faceDir;
    public Transform attacker;

    [Header("Player Detect")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;

    [Header("Timer")]
    public float waitTime;
    public float waitTimeCounter;
    public bool wait;
    public float lostTime;
    public float lostTimeCounter;

    [Header("State")]
    public bool isHurt;
    public bool isDead;

    //State machine
    private BaseState currentState;
    protected BaseState patrolState;
    protected BaseState chaseState;
    protected BaseState encounterState;

    protected virtual void Awake() {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        attack = GetComponent<Attack>();
        player = GameObject.FindWithTag("Player");
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
        //Axe axe = (Axe) this;
        //Debug.Log(axe.isAttack);
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

    public void PatrolAfterPlayerDead() {
        float health = player.GetComponent<Character>().currentHealth;
        
        if (health <= 0) {
            this.SwitchState(NPCState.Patrol);
            
        }
    }

    //Timer
    protected virtual void TimeCounter()
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

        if (!FoundPlayer() && lostTimeCounter > 0)
        {
            lostTimeCounter -= Time.deltaTime;
        }
        
    }

    public bool TouchingWalls() {
        return physicsCheck.touchLeftwall || physicsCheck.touchRightwall;
    }

    public bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, 
            checkSize, 0, new Vector3(faceDir, 0, 0), checkDistance, attackLayer);
    }

    //Switching state
    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            NPCState.Encounter => encounterState,
            _ => null
        };

        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    #region Events
    public void OnTakeDamage(Attack attacker)
    {
        //attacker = attackTrans;
        //Turn around
        if (attacker.transform.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        if (attacker.transform.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        //Hurted and repelled
        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attacker.transform.position.x, 0).normalized;

        //Start coroutine
        rb.velocity = new Vector2(0f, 0f);
        StartCoroutine(OnHurt(dir, attacker));
    }

    //Return the result of being attacked
    private IEnumerator OnHurt(Vector2 dir, Attack attacker)
    {
        //rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        //rb.AddForce(transform.up * hurtForce * 0.5f, ForceMode2D.Impulse);
        anim.SetBool("isAttack", false);
        rb.AddForce(dir * attacker.hurtForce, ForceMode2D.Impulse);
        rb.AddForce(transform.up * attacker.hurtForce * 0.5f, ForceMode2D.Impulse);

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

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * transform.localScale.x, 0), 0.2f);
    }
}
