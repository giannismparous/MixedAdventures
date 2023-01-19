using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlueGhostMove : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float colliderTimer;
    private float invisibleTimer;
    private float invisibleDurationTimer;
    private bool isInvisible;
    private bool isScared;
    private PlayerMove playerMove;
    [SerializeField] private float dirX;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float invisibleTimerValue;
    [SerializeField] private float invisibleDuration;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        colliderTimer = 0;
        invisibleTimer = invisibleTimerValue;
        isInvisible = false;
        isScared=false;
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
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

        if (!isInvisible && invisibleTimer <= 0)
        {
            anim.SetBool("isInvisible", true);
            isInvisible = true;
            invisibleDurationTimer = invisibleDuration;
        }
        else if (!isInvisible && invisibleTimer > 0) invisibleTimer -= Time.deltaTime;
        else if (isInvisible && invisibleDurationTimer <= 0)
        {
            anim.SetBool("isInvisible", false);
            isInvisible = false;
            invisibleTimer = invisibleTimerValue;
        }
        else if (isInvisible && invisibleDurationTimer > 0) invisibleDurationTimer -= Time.deltaTime;

        if (rb.velocity.y <= 0.0001f && rb.velocity.y >= -0.0001f && !Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer)) dirX *= -1;

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        colliderTimer -= Time.fixedDeltaTime;

        
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