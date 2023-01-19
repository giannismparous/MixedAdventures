using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagicianMovesController : MonoBehaviour
{

    private Rigidbody2D rb;
    private Vector3 parentLocalScale;
    private float attackRechargeTimer;
    private float colliderTimer;
    private int moveType;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private System.Random rand;
    private bool isStunAttacking;
    private bool isSpawningGoombasAttacking;
    private bool isUsingShield;
    private bool facingeRight;
    private float distance;
    private PlayerMove playerMove;
    [SerializeField] private float attackRechargeTimerValue;
    [SerializeField] private float distanceValue;
    [SerializeField] private float stunTimerValue;
    [SerializeField] private GameObject goombaPrefab;

    void Start()
    {

        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        colliderTimer = 0;
        rand = new System.Random();
        isSpawningGoombasAttacking = false;
        isStunAttacking = false;
        isUsingShield = false;
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        attackRechargeTimer = 0;
        moveType = rand.Next() % 2;

    }

    void Update() {

        if (isSpawningGoombasAttacking)
        {
            anim.SetBool("isSpawningGoombasAttacking", true);
        }
        else if (isStunAttacking)
        {
            anim.SetBool("isStunAttacking", true);
        }
        else if (isUsingShield)
        {
            anim.SetBool("isUsingShield", true);
        }
        else {
            anim.SetBool("isSpawningGoombasAttacking", false);
            anim.SetBool("isStunAttacking", false);
            anim.SetBool("isUsingShield", false);
        }


    }

    void FixedUpdate()
    {

        distance = (float)Math.Sqrt(Math.Pow(playerMove.transform.parent.transform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(playerMove.transform.parent.transform.position.y - transform.parent.transform.position.y, 2));
        if (!isUsingShield && !isSpawningGoombasAttacking && !isStunAttacking && attackRechargeTimer > 0) attackRechargeTimer -= Time.deltaTime;

        if (!isUsingShield && !isSpawningGoombasAttacking && !isStunAttacking && distance < distanceValue) {

            if (playerMove.transform.parent.transform.position.x < rb.transform.position.x) facingeRight = false;
            else facingeRight = true;

            if (attackRechargeTimer <= 0) {
                if (moveType == 0) StunAttack();
                else if (moveType == 1) SpawnGoombasAttack();
                else if (moveType == 2) isUsingShield = true;
                moveType = (moveType + 1) % 3;
                attackRechargeTimer = attackRechargeTimerValue;
            }

        }

        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;

        if (current_animation.Equals("magician_spawn_goombas_attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) isSpawningGoombasAttacking = false;
        else if (current_animation.Equals("magician_stun_attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) isStunAttacking = false;
        else if (current_animation.Equals("magician_shield") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            isUsingShield = false;
        }

        colliderTimer -= Time.deltaTime;

    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {

        if (((!facingeRight) && (parentLocalScale.x < 0)) || ((facingeRight) && (parentLocalScale.x > 0)))
            parentLocalScale.x *= -1;

        transform.parent.localScale = parentLocalScale;
    }

    void StunAttack()
    {
        isStunAttacking = true;
        playerMove.Stun(stunTimerValue);
    }

    void SpawnGoombasAttack()
    {
        isSpawningGoombasAttacking = true;
        Rigidbody2D temp1 = transform.parent.GetComponent<Rigidbody2D>();
        GameObject temp2;
        for (int i = 0; i < 5; i++)
        {
            if (i == 2) continue;
            temp2 = Instantiate(goombaPrefab) as GameObject;
            temp2.transform.position = new Vector2(temp1.position.x-4+i*2, temp1.position.y+3);
            if (i < 2) temp2.transform.Find("GoombaBody").GetComponent<GoombaMove>().SetDirX(-1);
            else if (i>2) temp2.transform.Find("GoombaBody").GetComponent<GoombaMove>().SetDirX(1);
        }
    }


    public bool ShieldIsActivated() {
        return isUsingShield;
    }
}