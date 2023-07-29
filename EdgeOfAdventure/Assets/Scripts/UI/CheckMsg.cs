using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckMsg : MonoBehaviour
{
    private TextMeshPro text;

    [Header("Event Listener")]
    [SerializeField]private FadeTextEventSO fadeTextevent;

    private void Awake() {
        text = this.GetComponent<TextMeshPro>();
    }

    private void OnEnable() {
        fadeTextevent.OnEventRaised += OnFadeTextEvent;
    }

    private void OnDisable() {
        fadeTextevent.OnEventRaised -= OnFadeTextEvent;
    }

    private void OnFadeTextEvent(Color target, float fadeDuration, bool fadeIn)
    {
        if (fadeIn) {
            text.DOBlendableColor(target, fadeDuration);
        } else {
            text.DOBlendableColor(target, fadeDuration);
        }
    }
}
