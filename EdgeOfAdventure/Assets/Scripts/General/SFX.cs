using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject SFXSprite;
    [SerializeField] private string sfx;

    private void Awake()
    {
        anim = SFXSprite.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.Play(sfx);
    }
}
