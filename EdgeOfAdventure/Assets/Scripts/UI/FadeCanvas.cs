using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeCanvas : MonoBehaviour
{
    [SerializeField]private Image fadeImage;

    [Header("Event Listener")]
    [SerializeField]private FadeEventSO fadeEvent; 
    
    private void OnEnable() {
        fadeEvent.OnEventRaised += OnFadeEvent;
    }

    private void OnDisable() {
        fadeEvent.OnEventRaised -= OnFadeEvent;
    }

    private void OnFadeEvent(Color target, float fadeDuration, bool fadeIn) {
        fadeImage.DOBlendableColor(target, fadeDuration);
    }
}
