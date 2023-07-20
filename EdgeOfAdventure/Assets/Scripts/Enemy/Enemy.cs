using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    [HideInInspector]public Animator anim;
    [HideInInspector]public PhysicsCheck physicsCheck;
    private GameObject player;
    private Transform hurtSFX;
    private GameObject hurtAudio;

    [Header("Arguments")]
    public float normalSpeed;
    public float chaseSpeed;
    public float encounterSpeed = 0;
    private float currentSpeed;
    
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
        hurtSFX = GameObject.FindWithTag("SFX").transform.Find("Hurt");
        hurtAudio = GameObject.FindWithTag("Audio");
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
        if (!isHurt && !isDead && !wait && physicsCheck.OnGround())
            Move();
        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }
    public void ChangeSpeedIdle() {
        currentSpeed = normalSpeed;
    }

    public void ChangeSpeedChase() {
        currentSpeed = chaseSpeed;
    }

    public void ChangeSpeedEncounter() {
        currentSpeed = encounterSpeed;
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
            if (TouchingWalls() || !physicsCheck.OnGround()) {
                waitTimeCounter -= Time.deltaTime;
            } else {
                waitTimeCounter = -1f;
            }
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                if (!isHurt)
                    transform.localScale = new Vector3(-faceDir, 1, 1);
            }
        }

        if (!FoundPlayer() && lostTimeCounter > 0)
        {
            lostTimeCounter -= Time.deltaTime;
        }

    }

    public Transform PlayerTransformWhenChase () {
        return player.transform; 
    }

    public bool PlayerOnGround() {
        return player.GetComponent<PhysicsCheck>().OnGround();
    }

    public bool PlayerDead() {
        return player.GetComponent<PlayerController>().isDead;
    }

    public bool TouchingWalls() {
        return physicsCheck.TouchLeftWall() || physicsCheck.TouchRightWall();
    }

    public bool FoundPlayer()
    {
        RaycastHit2D feedback = Physics2D.BoxCast(transform.position + (Vector3)centerOffset, 
            checkSize, 0, new Vector3(faceDir, 0, 0), checkDistance, attackLayer);
        if (feedback != false) player = feedback.transform.gameObject;
        return feedback;
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

        anim.SetBool("isAttack", false);
        rb.AddForce(dir * attacker.hurtForceX, ForceMode2D.Impulse);
        rb.AddForce(transform.up * attacker.hurtForceY, ForceMode2D.Impulse);

        // Hurt SFX
        HurtSFX(attacker);

        // Hurt FX
        HurtFX(attacker);

        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }

    private void HurtSFX(Attack attacker)
    {
        hurtSFX.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);

        if (!hurtSFX.gameObject.activeSelf)
        {
            hurtSFX.gameObject.SetActive(true);
        }
        else
        {
            Transform clone = Instantiate(hurtSFX, hurtSFX.transform.parent);
            Destroy(clone.gameObject, 2f);
        }
    }

    private void HurtFX(Attack attacker)
    {
        Transform hurtFX1 = hurtAudio.transform.Find("Hurt1");
        Transform hurtFX2 = hurtAudio.transform.Find("Hurt2");
        Transform hurtFX3 = hurtAudio.transform.Find("Hurt3");
        if (attacker.damage == 10)
        {
            hurtFX2.gameObject.SetActive(true);
            hurtFX2.gameObject.SetActive(false);
        }
        else if (attacker.damage == 4)
        {
            hurtFX3.gameObject.SetActive(true);
            hurtFX3.gameObject.SetActive(false);
        }
        else
        {
            hurtFX1.gameObject.SetActive(true);
            hurtFX1.gameObject.SetActive(false);
        }
    }

    public void OnDie()
    {
        gameObject.layer = 2;
        anim.SetBool("dead", true);
        anim.SetBool("isAttack", false);

        // Hurt SFX
        hurtSFX.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        hurtSFX.gameObject.SetActive(false);
        hurtSFX.gameObject.SetActive(true);

        // Hurt FX
        Transform hurtFX = hurtAudio.transform.Find("Die");
        hurtFX.gameObject.SetActive(true);
        hurtFX.gameObject.SetActive(false);

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
