using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("Attributes")]
    public float maxHealth;
    public float currentHealth;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;

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
        OnHealthChange?.Invoke(this);
    }

    private void Update() {
        if (invulnerable) {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0) {
                invulnerable = false;
            }
        }
    }

    public void TakeDamage(Attack attacker) {
        if (invulnerable) 
            return;

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
        if (!(physicsCheck.touchLeftwall && faceDir < 0) ||
            !(physicsCheck.touchRightwall && faceDir > 0)) 
        {
            rb.transform.Translate(faceDir * disX, 0, 0);
        }
    
    }

    public void ForceOnAir(float force)
    {
        rb.AddForce(transform.up * force, ForceMode2D.Impulse);
    }

}
