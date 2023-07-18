using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Menu : MonoBehaviour
{
   [SerializeField]private GameObject newGamebutton;
   [SerializeField]private GameObject continueButton;
   [SerializeField]private GameObject quitButton;

   private void OnEnable() {
      Time.timeScale = 1;
      EventSystem.current.SetSelectedGameObject(newGamebutton);
   }

   public void ExitGame() {
      Debug.Log("Quit!");
      Application.Quit();
   }


}
