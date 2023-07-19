using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Game Scene/GameSceneSO")]
public class GameSceneSO : ScriptableObject
{
    [SerializeField]private AssetReference sceneReference;
    [SerializeField]private SceneType sceneType;

    public AssetReference GetRef() {
        return sceneReference;
    }

    public SceneType GetSceneType() {
        return sceneType;
    }
}
