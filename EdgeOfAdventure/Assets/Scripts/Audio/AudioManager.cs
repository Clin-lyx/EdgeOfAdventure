using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Event listeners")]
    public PlayAudioEventSO FXEvent1;
    public PlayAudioEventSO FXEvent2;
    public PlayAudioEventSO HurtFXEvent;
    public PlayAudioEventSO BGMEvent;
    public FloatEventSO volumeEvent;
    public VoidEventSO pauseEvent;

    [Header("Broadcast")]
    public FloatEventSO syncVolumeEvent;

    [Header("Components")]
    public AudioSource FXSource1;
    public AudioSource FXSource2;
    public AudioSource HurtFXSource;
    public AudioSource BGMSource;
    public AudioMixer mixer;

    private void OnEnable()
    {
        FXEvent1.OnEventRaised += OnFXEvent1;
        FXEvent2.OnEventRaised += OnFXEvent2;
        HurtFXEvent.OnEventRaised += OnHurtFXEvent;
        BGMEvent.OnEventRaised += OnBGMEvent;
        volumeEvent.OnEventRaised += OnVolumeEvent;
        pauseEvent.OnEventRaised += onPauseEvent;
    }

    private void OnDisable()
    {
        FXEvent1.OnEventRaised -= OnFXEvent1;
        FXEvent2.OnEventRaised -= OnFXEvent2;
        HurtFXEvent.OnEventRaised -= OnHurtFXEvent;
        BGMEvent.OnEventRaised -= OnBGMEvent;
        volumeEvent.OnEventRaised -= OnVolumeEvent;
        pauseEvent.OnEventRaised -= onPauseEvent;
    }


    private void onPauseEvent()
    {   
        float amount;
        mixer.GetFloat("MasterVolume", out amount);

        syncVolumeEvent.RaiseEvent(amount);
    }

    private void OnVolumeEvent(float amount)
    {
        mixer.SetFloat("MasterVolume", amount * 100 - 80);
    }

    private void OnBGMEvent(AudioClip clip)
    {
        BGMSource.clip = clip;
        BGMSource.Play();
    }
    private void OnHurtFXEvent(AudioClip clip)
    {
        HurtFXSource.clip = clip;
        HurtFXSource.Play();
    }

    private void OnFXEvent1(AudioClip clip)
    {
        FXSource1.clip = clip;
        FXSource1.Play();
    }
    private void OnFXEvent2(AudioClip clip)
    {
        FXSource2.clip = clip;
        FXSource2.Play();
    }
}
