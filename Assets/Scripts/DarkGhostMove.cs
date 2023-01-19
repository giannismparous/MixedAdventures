using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DarkGhostMove : MonoBehaviour
{

    private float dirX;
    private float moveSpeed;
    private float distance;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private bool appeared;
    private bool fullyAppeared;
    private bool isChasing;
    private float disappearTimer;
    private PlayerMove playerMove;
    [SerializeField] private float moveSpeedValue;
    [SerializeField] private float disappearTimerValue;
    [SerializeField] private float distanceValue;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        dirX = -1f;
        moveSpeed = 0;
        anim = GetComponent<Animator>();
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        appeared = false;
        fullyAppeared = false;
        isChasing = false;
    }

    void FixedUpdate()
    {

        distance = (float)Math.Sqrt(Math.Pow(playerMove.transform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(playerMove.transform.position.y - transform.parent.transform.position.y, 2));
        
        if (!appeared && !fullyAppeared && distance <= distanceValue)
        {
            appeared = true;
            moveSpeed = moveSpeedValue;
            anim.SetTrigger("appeared");
        }


        if (fullyAppeared && !isChasing && distance <= distanceValue)
        {
            isChasing = true;
            anim.SetBool("isChasing", true);
            moveSpeed = moveSpeedValue;
        }
        else if (fullyAppeared && isChasing && distance > distanceValue) {
            isChasing = false;
            anim.SetBool("isChasing", false);
            moveSpeed = 0;
        }

        if (fullyAppeared && !isChasing && distance > distanceValue) {
            if (disappearTimer <= 0)
            {
                anim.SetTrigger("disappeared");
                fullyAppeared = false;
            }
            else disappearTimer -= Time.deltaTime;
        }

        if (distance <= distanceValue)
        {
            if (playerMove.transform.position.x < rb.position.x) dirX = -1;
            else if (playerMove.transform.position.x > rb.position.x) dirX = 1;
            disappearTimer = disappearTimerValue;
        }

        if (isChasing) rb.transform.position = Vector2.MoveTowards(rb.transform.position, playerMove.transform.position, moveSpeed * Time.deltaTime);
        else rb.velocity = Vector2.zero;


        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;

        if (current_animation.Equals("dark_ghost_disappear") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            fullyAppeared = false;
            appeared = false;

        }
        else if (current_animation.Equals("dark_ghost_appear") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            fullyAppeared = true;
            anim.SetBool("isChasing", true);
            isChasing = true;
            anim.ResetTrigger("appeared");
        }
        
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        if (dirX > 0)
            facingRight = true;
        else if (dirX < 0)
            facingRight = false;

        if (((facingRight) && (parentLocalScale.x < 0)) || ((!facingRight) && (parentLocalScale.x > 0)))
            parentLocalScale.x *= -1;

        transform.parent.localScale = parentLocalScale;
    }

}