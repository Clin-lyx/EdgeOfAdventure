using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;


    // Scene Load Request
    public void RaiseLoadRequestEvent(GameSceneSO sceneToload, Vector3 destPos, bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(sceneToload, destPos, fadeScreen);
    }
}