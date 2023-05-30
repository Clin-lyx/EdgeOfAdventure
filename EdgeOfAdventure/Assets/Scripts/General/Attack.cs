using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public float attackRange;
    public float attackRate;
    public float counter;

    public bool attacked;

    private void Update() {
        if (attacked) {
            counter -= Time.deltaTime;
            if (counter <= 0) {
                attacked = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (!attacked) {
            other.GetComponent<Character>()?.TakeDamage(this);
            attacked = true;
            counter = attackRate;
        }
    }
}
