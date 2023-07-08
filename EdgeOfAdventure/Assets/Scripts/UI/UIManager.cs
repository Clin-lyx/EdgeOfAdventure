using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;

    [Header("Event listeners")]
    public CharacterEventSO healthEvent;
    public FloatEventSO syncVolumeEvent;
    public SceneLoadEventSO loadEvent;

    [Header("Component")]
    public Button SettingsBtn;
    public GameObject pausePanel;
    public Slider volumeSlider;

    [Header("Broadcast")]
    [SerializeField]private VoidEventSO pauseEvent;

    private void Awake() {
        SettingsBtn.onClick.AddListener(TogglePausePanel);
    }

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        syncVolumeEvent.OnEventRaised += OnSyncVolumeEvent;
        loadEvent.LoadRequestEvent += OnloadEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        syncVolumeEvent.OnEventRaised -= OnSyncVolumeEvent;
        loadEvent.LoadRequestEvent -= OnloadEvent;
    }

    private void OnloadEvent(GameSceneSO sceneToload, Vector3 arg1, bool arg2)
    {
        var isMenu = (sceneToload.GetSceneType() == SceneType.Menu); 
        playerStatBar.gameObject.SetActive(!isMenu);    
    }

    private void OnSyncVolumeEvent(float amount)
    {
        volumeSlider.value = (amount + 80) / 100;
    }

    private void OnHealthEvent(Character character)
    {
        var persentage = character.currentHealth / character.maxHealth;
        playerStatBar.OnHealthChange(persentage);
    }

    private void TogglePausePanel() {
        if(pausePanel.activeInHierarchy) {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        } else {
            pauseEvent.RaiseEvent();
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
