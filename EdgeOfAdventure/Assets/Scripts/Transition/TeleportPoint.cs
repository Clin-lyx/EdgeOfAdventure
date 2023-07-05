using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{   
    [SerializeField]private Vector3 destPos;
    public void TriggerAction()
    {
        Debug.Log("Teleport");
    }
}
