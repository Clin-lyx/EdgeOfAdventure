using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/FadeTextEventSO")]
public class FadeTextEventSO : ScriptableObject
{
    public UnityAction<Color, float, bool> OnEventRaised;

    public void FadeIn(float duration) {
        RaiseEvent(Color.red, duration, true);
    }

    public void FadeOut(float duration) {
        RaiseEvent(Color.clear, duration, false);
    }

    public void RaiseEvent(Color target, float duration, bool fadeIn) {
        OnEventRaised?.Invoke(target, duration, fadeIn);
    }
}
