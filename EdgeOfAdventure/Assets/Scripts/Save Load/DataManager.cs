using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header("Event listeners")]
    [SerializeField]private VoidEventSO saveDataEvent;
    [SerializeField]private VoidEventSO loadDataEvent;

    [Header("Broadcast")]
    [SerializeField]private FadeTextEventSO fadeEvent;
    [SerializeField]private GameObject player;

    [SerializeField]private GameObject textButton;
    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data saveData;
    private string jsonFolder;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        saveData = new Data();
        jsonFolder = Application.persistentDataPath + "/SAVE DATA/";

        ReadSavedData();
    }

    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += Save;
        loadDataEvent.OnEventRaised += Load;
    }

    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
        loadDataEvent.OnEventRaised -= Load;
    }

    public void RegisterSaveData(ISaveable saveable)
    {
        if (!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }

    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }

    public void Save()
    {
        foreach (var saveable in saveableList)
        {
            saveable.GetSaveData(saveData);
        }

        var resultPath = jsonFolder + "data.sav";
        var jsonData = JsonConvert.SerializeObject(saveData);

        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }

        File.WriteAllText(resultPath, jsonData);

        //foreach (var item in saveData.characterPosDict)
        //{
        //    Debug.Log(item.Key + "    " + item.Value);
        //}
    }
    public void Load()
    {   
        var resultPath = jsonFolder + "data.sav";

        if (!File.Exists(resultPath)) {
            StartCoroutine(FadeText());
            textButton.SetActive(false);
            return;
        }
        
        player.layer = LayerMask.NameToLayer("Player");

        foreach (var saveable in saveableList)
        {            
            saveable.LoadData(saveData);
        }
    }

    private IEnumerator FadeText()
    {
        fadeEvent.FadeIn(0.3f);
        yield return new WaitForSeconds(1f);
        textButton.SetActive(true);
        fadeEvent.FadeOut(0.5f);
    }

    private void ReadSavedData()
    {
        var resultPath = jsonFolder + "data.sav";

        if (File.Exists(resultPath))
        {
            var stringData = File.ReadAllText(resultPath);
            var jsonData = JsonConvert.DeserializeObject<Data>(stringData);

            saveData = jsonData;
        }
    }
}
