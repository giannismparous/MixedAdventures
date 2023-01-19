using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CannibalMove : MonoBehaviour
{
    private float dirX;
    private float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float timer;
    public float pauseDuration = 2f;
    private Animator anim;
    private float colliderTimer;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dirX = -1f;
        moveSpeed = 6f;
        timer = 0;
        colliderTimer = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (colliderTimer<=0 && (collision.CompareTag("Ground") || collision.CompareTag("Enemy") || collision.CompareTag("Player")))
        {
            timer = pauseDuration;
            colliderTimer = 0.2f;
            dirX *= -1f;
        }
        
        if (collision.CompareTag("Ground")) {
            rb.velocity = new Vector2(dirX * moveSpeed, 0);
        }
    }

    void FixedUpdate()
    {

        timer -= Time.deltaTime;
        colliderTimer -= Time.deltaTime;
        if (timer <= 0)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
            if (Mathf.Abs(dirX) > 0 && rb.velocity.y == 0)
                anim.SetBool("isRunning", true);
            else
                anim.SetBool("isRunning", false);
        }
        else anim.SetBool("isRunning", false);
    }

    void LateUpdate()
    {
        if (timer <= 0)CheckWhereToFace();
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


    public void SetTimer(float timer) {
        this.timer = timer;
    }

}