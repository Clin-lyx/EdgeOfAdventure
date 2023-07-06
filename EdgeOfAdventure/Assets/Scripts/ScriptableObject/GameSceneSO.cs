using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Game Scene/GameSceneSO")]
public class GameSceneSO : ScriptableObject
{
    [SerializeField]private AssetReference sceneReference;
    [SerializeField]private SceneType type;

    public AssetReference GetRef() {
        return sceneReference;
    }
}
