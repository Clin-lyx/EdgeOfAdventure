using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandbag : MonoBehaviour
{
    private Rigidbody2D rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public PhysicsCheck physicsCheck;
    private GameObject player;
    private Transform hurtSFX;
    private GameObject hurtAudio;

    public Attack attack;
    public float faceDir;
    public Transform attacker;

    public LayerMask attackLayer;

    [Header("State")]
    public bool isHurt;
    public bool isDead;

    protected virtual void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        attack = GetComponent<Attack>();
        hurtSFX = GameObject.FindWithTag("SFX").transform.Find("Hurt");
        hurtAudio = GameObject.FindWithTag("Audio");

    }

    private void Update()
    {
        //Axe axe = (Axe) this;
        //Debug.Log(axe.isAttack);
        faceDir = rb.transform.localScale.x;
    }

    #region Events
    public void OnTakeDamage(Attack attacker)
    {
        //attacker = attackTrans;
        //Turn around
        if (attacker.transform.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        if (attacker.transform.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        //Hurted and repelled
        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attacker.transform.position.x, 0).normalized;

        //Start coroutine
        rb.velocity = new Vector2(0f, 0f);
        StartCoroutine(OnHurt(dir, attacker));
    }

    //Return the result of being attacked
    private IEnumerator OnHurt(Vector2 dir, Attack attacker)
    {
        rb.AddForce(dir * attacker.hurtForceX, ForceMode2D.Impulse);
        rb.AddForce(transform.up * attacker.hurtForceY, ForceMode2D.Impulse);

        // Hurt SFX
        HurtSFX(attacker);

        // Hurt FX
        HurtFX(attacker);

        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }
    private void HurtSFX(Attack attacker)
    {
        hurtSFX.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);

        if (!hurtSFX.gameObject.activeSelf)
        {
            hurtSFX.gameObject.SetActive(true);
        }
        else
        {
            Transform clone = Instantiate(hurtSFX, hurtSFX.transform.parent);
            Destroy(clone.gameObject, 2f);
        }
    }

    private void HurtFX(Attack attacker)
    {
        Transform hurtFX1 = hurtAudio.transform.Find("Hurt1");
        Transform hurtFX2 = hurtAudio.transform.Find("Hurt2");
        Transform hurtFX3 = hurtAudio.transform.Find("Hurt3");
        if (attacker.damage == 10)
        {
            hurtFX2.gameObject.SetActive(true);
            hurtFX2.gameObject.SetActive(false);
        }
        else if (attacker.damage == 4)
        {
            hurtFX3.gameObject.SetActive(true);
            hurtFX3.gameObject.SetActive(false);
        }
        else
        {
            hurtFX1.gameObject.SetActive(true);
            hurtFX1.gameObject.SetActive(false);
        }
    }

    public void OnDie()
    {
        gameObject.layer = 2;
        anim.SetBool("dead", true);
        anim.SetBool("isAttack", false);

        // Hurt SFX
        hurtSFX.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        hurtSFX.gameObject.SetActive(false);
        hurtSFX.gameObject.SetActive(true);

        // Hurt FX
        Transform hurtFX = hurtAudio.transform.Find("Die");
        hurtFX.gameObject.SetActive(true);
        hurtFX.gameObject.SetActive(false);

        isDead = true;
    }

    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }

    #endregion

}
