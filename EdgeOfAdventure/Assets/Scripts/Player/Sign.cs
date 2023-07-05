using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{
    private Animator anim;
    private PlayerInputControl playerInput;
    [SerializeField]private GameObject signSprite;
    [SerializeField]private Transform playerTransform;
    private IInteractable targetItem;
    private bool canPress;

    private void Awake() {
        anim = signSprite.GetComponent<Animator>();
        playerInput = new PlayerInputControl();
        playerInput.Enable();
    }

    private void OnEnable() {
        playerInput.Gameplay.Confirm.started += OnConfirm;
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
            targetItem = other.GetComponent<IInteractable>();
            anim.Play("keyboard");
        }
    }

    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (canPress) 
        {
            targetItem.TriggerAction();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        canPress = false;
    }
}
