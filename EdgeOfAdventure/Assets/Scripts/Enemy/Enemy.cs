using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    private PhysicsCheck physicsCheck;

    [Header("Arguments")]
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public float faceDir;
    private bool turned;

    private void Awake() {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;

    }

    private void Update() {
        faceDir = rb.transform.localScale.x;
        HitWallAndTurn();
    }

    private void FixedUpdate() {
        Move();
    }

    private void HitWallAndTurn () {
        if (physicsCheck.touchLeftwall || physicsCheck.touchRightwall) {
            if (!turned) {
                transform.localScale = new Vector3(-1 * faceDir, 1, 1);
                turned = true;
            }
        } else {
            turned = false;
        }
    }

    public virtual void Move() {
        rb.velocity = new Vector2(currentSpeed * faceDir * Time.deltaTime, rb.velocity.y);
    }
}
