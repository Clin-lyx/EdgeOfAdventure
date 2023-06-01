using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Arguments")]
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public float faceDir;

    private void Awake() {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentSpeed = normalSpeed;

    }

    private void Update() {
        faceDir = rb.transform.localScale.x;
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        rb.velocity = new Vector2(currentSpeed * faceDir * Time.deltaTime, rb.velocity.y);
    }
}
