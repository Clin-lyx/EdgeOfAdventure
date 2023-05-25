using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    public Vector2 bottomOffset;
    public bool onGround;
    public float checkRadius;
    public LayerMask Ground;
    private Rigidbody2D rb;
    private void Update() {
        Check();
    }

    private void Check(){
        onGround = Physics2D.OverlapCircle((Vector2) transform.position + bottomOffset, checkRadius, Ground);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawSphere((Vector2) transform.position + bottomOffset, checkRadius);
    }
}
