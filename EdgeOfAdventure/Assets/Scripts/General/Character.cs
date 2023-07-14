using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("Event Listener")]
    [SerializeField] VoidEventSO newGameEvent;

    [Header("Attributes")]
    public float maxHealth;
    public float currentHealth;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private GameObject enemy;

    [Header("Invulnerable")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;

    [Header("Events")]
    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Attack> OnTakeDamage;
    public UnityEvent IsDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
    }

    private void Start() {
        currentHealth = maxHealth;
    }

    private void NewGame() {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);
    }

    private void OnEnable() {
        newGameEvent.OnEventRaised += NewGame;
    }

    private void OnDisable() {
        newGameEvent.OnEventRaised -= NewGame;
    }

    private void Update() {
        if (invulnerable) {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0) {
                invulnerable = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Death Trigger"))
        {
            // make character die, and update the health
            currentHealth = 0;
            OnHealthChange?.Invoke(this);
            IsDead?.Invoke();
        }
    }

    public void TakeDamage(Attack attacker) {
        if (invulnerable) 
            return;

        if (this.gameObject.CompareTag("Player")) {
            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController.IsPerfectDodge()) {
                playerController.perfect = true;
                enemy = attacker.transform.parent.gameObject;
                return;
            }
        }

        if (currentHealth - attacker.damage > 0) {
            this.currentHealth -= attacker.damage;
            TriggerInvulnerable();
            OnTakeDamage?.Invoke(attacker);
        } else {
            currentHealth = 0;
            IsDead?.Invoke();
        }

        OnHealthChange?.Invoke(this);
    }

    // make character stop receiving damage
    private void TriggerInvulnerable() {
        if (!invulnerable) {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }

    //Frame event for attack displacement
    public void MoveEvent(float disX)
    {
        int faceDir = (int)transform.localScale.x;
        if (!(physicsCheck.TouchLeftWall() && faceDir < 0) ||
            !(physicsCheck.TouchRightWall() && faceDir > 0)) 
        {
            rb.transform.Translate(faceDir * disX, 0, 0);
        }
    
    }

    public void ForceOnAir(float force)
    {
        rb.velocity = new Vector2(0f, -6f);
        rb.AddForce(transform.up * force, ForceMode2D.Impulse);
    }

    public GameObject GetEnemy(){
        return enemy;
    }
}
