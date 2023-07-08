using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Menu : MonoBehaviour
{
   [SerializeField]private GameObject newGamebutton;

   private void OnEnable() {
    EventSystem.current.SetSelectedGameObject(newGamebutton);
   }

   public void ExitGame() {
    Debug.Log("Quit!");
    Application.Quit();
   }
}
