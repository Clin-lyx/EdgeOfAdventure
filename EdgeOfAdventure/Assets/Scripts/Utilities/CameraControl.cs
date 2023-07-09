using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    [Header("Event listeners")]
    [SerializeField]private CinemachineImpulseSource impulseSource;
    [SerializeField]private VoidEventSO cameraShakeEvent;
    [SerializeField]private VoidEventSO aftSceneLoadedEvent;
    [SerializeField]private float playerDir;
    [SerializeField]private float shakeForce;

    private CinemachineConfiner2D confiner2D;

    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
        aftSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
        aftSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
    }

    private void OnAfterSceneLoadedEvent()
    {
        GetNewCameraBounds();
    }

    private void OnCameraShakeEvent()
    {
        playerDir = GameObject.Find("Player").transform.localScale.x;
        impulseSource.m_DefaultVelocity = new Vector3(playerDir * shakeForce, 0, 0) ;
        impulseSource.GenerateImpulse();
    }

    //private void Start()
    //{
    //    GetNewCameraBounds();
    //}

    private void GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
            return;

        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();

        confiner2D.InvalidateCache();
    }
}
