using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Events")]
    [SerializeField]private SceneLoadEventSO loadEventSO; 
    [SerializeField]private GameSceneSO firstLoadscene;

    [SerializeField]private GameSceneSO currentLoadedscene;
    private GameSceneSO sceneToload;
    private Vector3 destPos;
    private bool fadeScreen;
    [SerializeField]private float fadeDuration;

    private void Awake() {
        //Addressables.LoadSceneAsync(firstLoadscene.GetRef(), LoadSceneMode.Additive);
        currentLoadedscene = firstLoadscene;
        currentLoadedscene.GetRef().LoadSceneAsync(LoadSceneMode.Additive);
    }
    
    private void OnEnable() {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent; 
    }

    private void OnDisable() {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent; 
    }

    private void OnLoadRequestEvent(GameSceneSO sceneToload, Vector3 destPos, bool fadeScreen)
    {
        this.sceneToload = sceneToload;
        this.destPos = destPos;
        this.fadeScreen = fadeScreen;
        if (currentLoadedscene != null) {
            StartCoroutine(UnLoadPreviousScene());
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
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        sceneToload.GetRef().LoadSceneAsync(LoadSceneMode.Additive, true);
    }

}
