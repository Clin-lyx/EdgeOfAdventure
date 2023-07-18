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
    [SerializeField]private Button SettingsBtn;
    [SerializeField]private GameObject pausePanel;
    [SerializeField]private Slider volumeSlider;
    [SerializeField]private GameObject commandList;
    [SerializeField]private Button commandListbtn;
    [SerializeField]private Button commandListclose;
    [SerializeField]private GameSceneSO MainMenu;

    [Header("Broadcast")]
    [SerializeField]private VoidEventSO pauseEvent;

    private void Awake() {
        SettingsBtn.onClick.AddListener(TogglePausePanel);
        commandListbtn.onClick.AddListener(ToggleCommandList);
        commandListclose.onClick.AddListener(CloseCommandList);
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

    private void ToggleCommandList() {
        commandListclose.gameObject.SetActive(true);
        SettingsBtn.gameObject.SetActive(false);
        commandList.SetActive(true);
    }

    private void CloseCommandList() {
        commandList.SetActive(false);
        commandListclose.gameObject.SetActive(false);
        SettingsBtn.gameObject.SetActive(true);
    }

    public void ClickSettingsBtn() {
        SettingsBtn.onClick.Invoke();
    }

    public void ExitGame() {
        Debug.Log("quit");
        Application.Quit();
    }

}
