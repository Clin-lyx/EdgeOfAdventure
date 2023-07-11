using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementCut : MonoBehaviour
{
    private GameObject enemy;
    private Transform judgementCutSFX;

    private void Awake()
    {
        enemy = GameObject.FindWithTag("Enemy");
        judgementCutSFX = GameObject.FindWithTag("SFX").transform.Find("Judgement Cut");
    }

    private void OnEnable()
    {
        if (enemy != null)
        {
            Vector3 enemyPos = enemy.transform.position;
            judgementCutSFX.position = new Vector3(enemyPos.x, enemyPos.y + 2f, 0);
            judgementCutSFX.gameObject.SetActive(true);
        }
    }
}
