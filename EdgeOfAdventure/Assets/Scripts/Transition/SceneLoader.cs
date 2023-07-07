using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{


    [Header("Event listeners")]
    [SerializeField]private SceneLoadEventSO loadEventSO; 
    [SerializeField]private GameSceneSO firstLoadscene;

    [Header("Broadcast")]
    public VoidEventSO aftSceneLoadedEvent;

    [SerializeField]private GameSceneSO currentLoadedscene;
    private GameSceneSO sceneToload;
    private Vector3 destPos;
    private bool fadeScreen;
    private bool isLoading;
    [SerializeField]private float fadeDuration;
    [SerializeField]public Transform playerTrans;
    [SerializeField]public Vector3 firstPosition;

    private void Awake() {
        //Addressables.LoadSceneAsync(firstLoadscene.GetRef(), LoadSceneMode.Additive);
        //currentLoadedscene = firstLoadscene;
        //currentLoadedscene.GetRef().LoadSceneAsync(LoadSceneMode.Additive);
    }

    private void Start()
    {
        NewGame();
    }

    private void OnEnable() {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent; 
    }

    private void OnDisable() {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent; 
    }

    private void NewGame()
    {
        sceneToload = firstLoadscene;
        OnLoadRequestEvent(sceneToload, firstPosition, true);
    }

    private void OnLoadRequestEvent(GameSceneSO sceneToload, Vector3 destPos, bool fadeScreen)
    {
        if (isLoading)
            return;

        isLoading = true;
        this.sceneToload = sceneToload;
        this.destPos = destPos;
        this.fadeScreen = fadeScreen;
        if (currentLoadedscene != null) {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
        
        //Debug.Log(sceneToload.GetRef().SubObjectName);
    }

    private IEnumerator UnLoadPreviousScene() 
    {
        if (fadeScreen) 
        {
            
        }

        yield return new WaitForSeconds(fadeDuration);
        
        yield return currentLoadedscene.GetRef().UnLoadScene();

        playerTrans.gameObject.SetActive(false);
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = sceneToload.GetRef().LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        currentLoadedscene = sceneToload;

        playerTrans.position = destPos;

        playerTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {

        }

        isLoading = false;
        aftSceneLoadedEvent.RaiseEvent();
    }
}
