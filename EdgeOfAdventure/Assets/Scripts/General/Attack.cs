using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public float hurtForceX;
    public float hurtForceY;

    private void OnTriggerEnter2D(Collider2D other) {    
        other.GetComponent<Character>()?.TakeDamage(this);
    }
}
