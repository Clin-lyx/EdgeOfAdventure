using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JudgementCut : MonoBehaviour
{
    [SerializeField]private GameObject enemy;
    [SerializeField]private Character character;
    private Transform judgementCutSFX;
    private PlayerController playerController;

    private void Awake()
    {
        judgementCutSFX = GameObject.FindWithTag("SFX").transform.Find("Judgement Cut");
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        enemy = character.GetEnemy();
        if (enemy != null) {

            Vector3 cutPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, 0);
            judgementCutSFX.position = cutPos;          
            judgementCutSFX.gameObject.SetActive(playerController.perfect);
        }
    }

    private void OnDisable() {
        enemy = null;
        character.GetComponent<PlayerController>().perfect = false;
    }
}
