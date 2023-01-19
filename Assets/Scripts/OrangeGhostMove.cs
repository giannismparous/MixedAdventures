using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OrangeGhostMove : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float moveSpeed;
    private float colliderTimer;
    private float scareTimer;
    private float scareDuration;    
    private bool isScaring;
    private float distance;
    private bool isScared;
    private PlayerMove playerMove;
    [SerializeField] private float dirX;
    [SerializeField] private float moveSpeedValue1;
    [SerializeField] private float moveSpeedValue2;
    [SerializeField] private float distanceValue;
    [SerializeField] private float scareTimerValue;
    [SerializeField] private float scareDurationValue;
    [SerializeField] private float stunDuration;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        colliderTimer = 0;
        isScaring = false;
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        moveSpeed = moveSpeedValue1;
        isScared=false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (colliderTimer<=0 && (collision.CompareTag("Ground") || collision.CompareTag("Enemy")))
        {
            colliderTimer = 0.2f;
            dirX *= -1f;
        }
    }

    void FixedUpdate()
    {

        if (playerMove.ScaringGhosts() && !isScared){
            anim.SetTrigger("isScared");
            isScared=true;
        }
        else if (!playerMove.ScaringGhosts() && isScared){
            anim.SetTrigger("isNotScared");
            isScared=false;
        }

        if (isScared)distance=10000;
        else distance = (float)Math.Sqrt(Math.Pow(playerMove.transform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(playerMove.transform.position.y - transform.parent.transform.position.y, 2));


        if (distance <= distanceValue)
        {

            if (rb.velocity.y <= 0.0001f && rb.velocity.y >= -0.0001f && !Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer)) moveSpeed=0;
            else moveSpeed=moveSpeedValue2;

            if (playerMove.transform.position.x <= transform.position.x) dirX = -1;
            else dirX = 1;

            anim.SetBool("isChasing", true);

            if (!isScaring && scareTimer <= 0)
            {
                anim.SetBool("isScaring", true);
                isScaring = true;
                scareDuration = scareDurationValue;
            }
            else if (!isScaring && scareTimer > 0) scareTimer -= Time.deltaTime;
            else if (isScaring && scareDuration <= 0)
            {
                anim.SetBool("isScaring", false);
                isScaring = false;
                scareTimer = scareTimerValue;
            }
            else if (isScaring && scareDuration > 0) scareDuration -= Time.deltaTime;

            if (isScaring && dirX * playerMove.GetDirX() < 0)playerMove.Stun(stunDuration);
        }
        else
        {
            if (rb.velocity.y <= 0.0001f && rb.velocity.y >= -0.0001f && !Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer)) dirX *= -1;
            moveSpeed = moveSpeedValue1;
            anim.SetBool("isChasing", false);
            anim.SetBool("isScaring", false);
            isScaring = false;
            scareTimer = scareTimerValue;
        }


        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        colliderTimer -= Time.deltaTime;

        
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