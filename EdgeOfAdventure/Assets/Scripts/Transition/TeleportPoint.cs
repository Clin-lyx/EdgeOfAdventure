using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{   
    [SerializeField]private SceneLoadEventSO loadEventSO;
    [SerializeField]private Vector3 destPos;
    [SerializeField]private GameSceneSO sceneTogo;
    public void TriggerAction()
    {
        Debug.Log("Teleport");
        loadEventSO.RaiseLoadRequestEvent(sceneTogo, destPos, true);
    }
}
