using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HammerKoopaMove : MonoBehaviour
{

    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float colliderTimer;
    private float timer;
    private float rechargingTimer;
    private float fallRadius;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private bool attack;
    private bool recharging;
    private System.Random rand;
    private bool attackNow;
    [SerializeField] private float dirX;
    [SerializeField] private float moveSpeedValue;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float pauseDuration;
    [SerializeField] private float rechargingDuration;
    [SerializeField] private GameObject hammerPrefab;
    [SerializeField] private float AttackThrowXUpper;
    [SerializeField] private float AttackThrowXLower;
    [SerializeField] private float AttackThrowYUpper;
    [SerializeField] private float AttackThrowYLower;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        colliderTimer = 0;
        timer = 0;
        fallRadius = 0.1f;
        attack = false;
        recharging = false;
        rechargingTimer = 0;
        rand = new System.Random();
        attackNow = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (colliderTimer <= 0 && (collision.CompareTag("Ground") || collision.CompareTag("Enemy")))
        {
            colliderTimer = 0.2f;
            dirX *= -1f;
        }
    }

    void FixedUpdate()
    {

        if (!attack)
        {
            timer -= Time.deltaTime;
            if (rb.velocity.y <= 0.001f && rb.velocity.y >= -0.001f && !Physics2D.OverlapCircle(groundCheck.position, fallRadius, groundLayer))
            {
                dirX *= -1;
                timer = pauseDuration;
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            if (timer <= 0)
            {
                rb.velocity = new Vector2(dirX * moveSpeedValue, rb.velocity.y);
                fallRadius = 0.1f;
                if (moveSpeedValue==0)anim.SetBool("isRunning", false);
                else anim.SetBool("isRunning", true );
            }
            else
            {
                fallRadius = 100f;
                anim.SetBool("isRunning", false);
            }
        }
        else {
            rechargingTimer -= Time.deltaTime;
            if (recharging && rechargingTimer <= 0)
            {
                recharging = false;
                attackNow = true;
            }
        }
        colliderTimer -= Time.deltaTime;
        
    }

    void LateUpdate()
    {
        if (timer<=0 && !recharging)CheckWhereToFace();
    }

    void CheckWhereToFace()
    {

        if (!attack)
        {
            if (dirX > 0)
                facingRight = true;
            else if (dirX < 0)
                facingRight = false;
        }
        else
        {
            if (dirX > 0)
                facingRight = false;
            else if (dirX < 0)
                facingRight = true;
        }

        if (((facingRight) && (parentLocalScale.x < 0)) || ((!facingRight) && (parentLocalScale.x > 0)))
            parentLocalScale.x *= -1;

        transform.parent.localScale = parentLocalScale;
    }

    void ThrowHammer() {
        GameObject tempHammer = Instantiate(hammerPrefab) as GameObject;
        Transform attackPosTransform = transform.parent.Find("AttackPosition").transform;
        tempHammer.transform.position = new Vector2(attackPosTransform.position.x, attackPosTransform.position.y);
        float tempX = (float)(rand.NextDouble() * (AttackThrowXUpper - AttackThrowXLower) + AttackThrowXLower);
        float tempY = (float)(rand.NextDouble() * (AttackThrowYUpper - AttackThrowYLower) + AttackThrowYLower);
        if (facingRight)tempHammer.GetComponent<Rigidbody2D>().velocity = new Vector2(-tempX, tempY);
        else tempHammer.GetComponent<Rigidbody2D>().velocity = new Vector2(tempX, tempY);
    }

    public void Attack() {
        rb.velocity = new Vector2(0, rb.velocity.y);
        attack = true;
        anim.SetBool("isRunning", false);
        timer = 0; //No idea

        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;

        if (!recharging)
        {
            if ((current_animation.Equals("hammer_koopa_recharging") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1) || attackNow || !current_animation.Equals("hammer_koopa_recharging") && !current_animation.Equals("hammer_koopa_attack") )
            {
                anim.SetBool("isAttacking", true);
                anim.SetBool("isRecharging", false);
                attackNow = false;
            }
            else if (current_animation.Equals("hammer_koopa_attack")&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1)
            {
                anim.SetBool("isAttacking", false);
                anim.SetBool("isRecharging", true);
                recharging = true;
                rechargingTimer = rechargingDuration;
                ThrowHammer();
            }
        }
    }

    public void StopAttack()
    {
        
        if (rechargingTimer <= 0)
        {
            attack = false;
            recharging = false;
            anim.SetBool("isAttacking", false);
            anim.SetBool("isRecharging", false);
        }
    }


    public void SetDirX(float dirX)
    {
        this.dirX= dirX;
    }
}