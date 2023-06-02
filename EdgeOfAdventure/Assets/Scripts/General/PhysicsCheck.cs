using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{    
    [Header("Arguments")]
    public bool manual;
    public LayerMask Ground;
    private Rigidbody2D rb;
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    private CapsuleCollider2D coll;
    
    [Header("States")]
    public bool onGround;
    public bool touchRightwall;
    public bool touchLeftwall;
    public float checkRadius;

    private void Awake() {
        coll = GetComponent<CapsuleCollider2D>();

        if (!manual) {
            rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2, coll.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
    }

    private void Update() {
        Check();
    }

    private void Check(){
        // checks if player object is overlapping with the platform layer mask
        onGround = Physics2D.OverlapCircle((Vector2) transform.position + bottomOffset, checkRadius, Ground);
        
        // check if player is overlapping with the left wall
        touchLeftwall = Physics2D.OverlapCircle((Vector2) transform.position + leftOffset, checkRadius, Ground);

        // check if player is overlapping with the right wall
        touchRightwall = Physics2D.OverlapCircle((Vector2) transform.position + rightOffset, checkRadius, Ground);
    }

    private void OnDrawGizmosSelected() {
        // to visualize the size of overlapping area so as to set a better checkRadius and bottomOffset
        Gizmos.DrawSphere((Vector2) transform.position + bottomOffset, checkRadius);
        Gizmos.DrawSphere((Vector2) transform.position + leftOffset, checkRadius);
        Gizmos.DrawSphere((Vector2) transform.position + rightOffset, checkRadius);
    }
}
