using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sign : MonoBehaviour
{
    private Animator anim;
    [SerializeField]private GameObject signSprite;
    [SerializeField]private Transform playerTransform;
    private bool canPress;

    private void Awake() {
        anim = signSprite.GetComponent<Animator>();
    }

    private void Update() {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress; 
        signSprite.transform.localScale = playerTransform.localScale;
    }

    private void OnDisable() {
        canPress = false;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Interactable")) {
            canPress = true;
            anim.Play("keyboard");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        canPress = false;
    }
}
