using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeoxysBoltAttackMove : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private bool moving;
    private Vector2 targetPosition;  
    private PlayerMove playerMove;
    [SerializeField] private float moveSpeedValue;
    [SerializeField] private float destroyTimer;
    [SerializeField] private int damage;
    [SerializeField] private float KBForce;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        Vector2 targetPosition = playerMove.transform.position;
        moving=false;
    }

    void FixedUpdate()
    {
        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;

        if (current_animation.Equals("deoxys_bolt_attack_start") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
            anim.SetTrigger("charged");
            moving=true;
        }

        if (moving)rb.transform.position = Vector2.MoveTowards(rb.transform.position, targetPosition, moveSpeedValue * Time.deltaTime);


        if (destroyTimer<=0)Destroy(gameObject);
        else destroyTimer-=Time.deltaTime;


    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.x <= transform.position.x) playerMove.RegisterHitKnockback(KBForce, true);
            else playerMove.RegisterHitKnockback(KBForce, false);
            playerMove.TakeDamage(10);
            Destroy(transform.gameObject);
        }
        else if (other.CompareTag("Ground"))Destroy(transform.gameObject);
    }

    public void SetTargetPosition(Vector2 target){
        targetPosition=target;
    }
}