using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    [Header("Broadcast")]
    [SerializeField]private VoidEventSO saveGameEvent;

    [Header("Arguments")]
    [SerializeField]private SpriteRenderer spriteRenderer;
    [SerializeField]private GameObject lightObj;
    [SerializeField]private Sprite darkSprite;
    [SerializeField]private Sprite lightSprite;
    private bool isDone;

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? lightSprite : darkSprite;
        lightObj.SetActive(isDone);
    }

    public void TriggerAction()
    {
        if (!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = lightSprite;
            lightObj.SetActive(true);
            saveGameEvent.RaiseEvent();

            this.gameObject.tag = "Untagged";
        }
    }
}
