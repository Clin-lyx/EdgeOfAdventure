using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField]private int damage;
    [SerializeField]private float hurtForceX;
    [SerializeField]private float hurtForceY;

    private void OnTriggerEnter2D(Collider2D other) {
        other.GetComponent<Character>()?.TakeDamage(this);
    }

    public float ForceX() {
        return hurtForceX;
    }

    public float ForceY() {
        return hurtForceY;
    }

    public int Damage() {
        return damage;
    }
}
