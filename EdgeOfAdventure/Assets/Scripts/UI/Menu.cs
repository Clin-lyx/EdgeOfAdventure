using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Menu : MonoBehaviour
{
   [SerializeField]private GameObject newGamebutton;
   [SerializeField]private GameObject continueButton;
   [SerializeField]private GameObject quitButton;
   private GameObject player;

   private void OnEnable() {
      EventSystem.current.SetSelectedGameObject(newGamebutton);
      player = GameObject.FindWithTag("Player");
      Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
      rb.gravityScale = 0;
   }

   private void OnDisable() {
      player = GameObject.FindWithTag("Player");
      Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
      rb.gravityScale = 4.5f;
   }

   public void ExitGame() {
      Debug.Log("Quit!");
      Application.Quit();
   }


}
