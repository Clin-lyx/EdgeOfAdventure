using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;
    private string jsonFolder;

    [Header("Event listeners")]
    public CharacterEventSO healthEvent;
    public FloatEventSO syncVolumeEvent;
    public SceneLoadEventSO unloadedSceneEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;
    [SerializeField]private VoidEventSO NewGameEvent;

    [Header("Components")]
    [SerializeField]private Button SettingsBtn;
    [SerializeField]private GameObject pausePanel;
    [SerializeField]private Slider volumeSlider;
    [SerializeField]private GameObject commandList;
    [SerializeField]private Button commandListbtn;
    [SerializeField]private Button commandListclose;
    [SerializeField]private GameSceneSO MainMenu;
    [SerializeField]private GameObject gameOverPanel;
    [SerializeField]private GameObject restartBtn;
    [SerializeField]private GameObject mobileTouch;

    [Header("Broadcasts")]
    [SerializeField]private VoidEventSO pauseEvent;

    private void Awake() {
        SettingsBtn.onClick.AddListener(TogglePausePanel);
        commandListbtn.onClick.AddListener(OpenCommandList);
        commandListclose.onClick.AddListener(CloseCommandList);
        jsonFolder = Application.persistentDataPath + "/SAVE DATA/";

#if UNITY_STANDALONE
        mobileTouch.SetActive(false);
#endif
    }

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        syncVolumeEvent.OnEventRaised += OnSyncVolumeEvent;
        unloadedSceneEvent.LoadRequestEvent += OnUnLoadedSceneEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        NewGameEvent.OnEventRaised += OnNewGameEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        syncVolumeEvent.OnEventRaised -= OnSyncVolumeEvent;
        unloadedSceneEvent.LoadRequestEvent -= OnUnLoadedSceneEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
        NewGameEvent.OnEventRaised -= OnNewGameEvent;
    }

    private void OnNewGameEvent()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartBtn);
    }

    private void OnLoadDataEvent()
    {
        var resultPath = jsonFolder + "data.sav";
        if (File.Exists(resultPath)) gameOverPanel.SetActive(false);
    }

    private void OnUnLoadedSceneEvent(GameSceneSO sceneToload, Vector3 arg1, bool arg2)
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

    private void OpenCommandList() {
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

    public void ResumeGame() {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void ExitGame() {
        Debug.Log("quit");
        Application.Quit();
    }

}
