using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class FadeCanvas : MonoBehaviour
{
    [SerializeField]private Image fadeImage;
    [SerializeField]private TextMeshProUGUI text;

    [Header("Event Listener")]
    [SerializeField]private FadeEventSO fadeEvent; 
    [SerializeField]private FadeTextEventSO fadeTextevent;
    
    private void OnEnable() {
        fadeEvent.OnEventRaised += OnFadeEvent;
        fadeTextevent.OnEventRaised += OnFadeTextEvent;
    }

    private void OnDisable() {
        fadeEvent.OnEventRaised -= OnFadeEvent;
        fadeTextevent.OnEventRaised -= OnFadeTextEvent;
    }

    private void OnFadeTextEvent(Color target, float fadeDuration, bool fadeIn)
    {
        if (fadeIn) {
            text.gameObject.SetActive(true);
            text.DOBlendableColor(target, fadeDuration);
        } else {
            text.DOBlendableColor(target, fadeDuration);
        }
    }

    private void OnFadeEvent(Color target, float fadeDuration, bool fadeIn) {
        fadeImage.DOBlendableColor(target, fadeDuration);
    }
}
