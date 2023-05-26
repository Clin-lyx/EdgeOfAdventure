using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{    
    public LayerMask Ground;
    private Rigidbody2D rb;
    public Vector2 bottomOffset;
    public bool onGround;
    public float checkRadius;

    private void Update() {
        Check();
    }

    private void Check(){
        // checks if player object is overlapping with the platform layer mask
        onGround = Physics2D.OverlapCircle((Vector2) transform.position + bottomOffset, checkRadius, Ground);
    }

    private void OnDrawGizmosSelected() {
        // to visualize the size of overlapping area so as to set a better checkRadius and bottomOffset
        Gizmos.DrawSphere((Vector2) transform.position + bottomOffset, checkRadius);
    }
}
