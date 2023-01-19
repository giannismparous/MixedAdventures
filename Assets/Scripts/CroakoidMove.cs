using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CroakoidMove : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float distance;
    private bool isAwake;
    private bool isJumping;
    private bool isFalling;
    private float jumpTimer;
    private PlayerMove playerMove;
    public float dirX;
    public float distanceValue;
    public float jumpTimerValue;
    public float upwardForce;
    public float sidewardForce;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        isAwake = false;
        isFalling = false;
        isJumping = false;
        Physics2D.IgnoreCollision(player.transform.Find("Body").GetComponent<Collider2D>(), transform.parent.transform.Find("BounceCheck").GetComponent<Collider2D>());
    }

    void FixedUpdate()
    {

        distance = (float)Math.Sqrt(Math.Pow(playerMove.transform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(playerMove.transform.position.y - transform.parent.transform.position.y, 2));

        if (!isAwake && distance <= distanceValue) {
            isAwake = true;
            anim.SetBool("isAwake", true);
            jumpTimer = jumpTimerValue;
        }

        if (isAwake && !isJumping && !isFalling) {

            if (playerMove.transform.position.x <= transform.position.x) dirX = -1;
            else dirX = 1;
            if (jumpTimer <= 0 && rb.velocity.y <= 0.001f && rb.velocity.y >= -0.001f)
            {
                isJumping = true;
                jumpTimer = jumpTimerValue;
                rb.velocity = new Vector2(0,0.001f);
                rb.AddForce(Vector2.up * upwardForce);
                if (dirX>0)rb.AddForce(Vector2.right * sidewardForce);
                else if (dirX<0) rb.AddForce(Vector2.left * sidewardForce);
            }
            else jumpTimer -= Time.deltaTime;
        }
        
        if (isAwake && !isFalling && !isJumping && distance>distanceValue)
        {
            isAwake = false;
            anim.SetBool("isAwake", false);
        }

        if (isAwake)
        {
            if (rb.velocity.y > 0.001f)
            {
                anim.SetBool("isJumping", true);
                isFalling = false;
                isJumping = true;
            }
            else if (rb.velocity.y < -0.001f)
            {
                anim.SetBool("isJumping", false);
                isFalling = true;
                isJumping = false;
            }
            else if (isFalling && rb.velocity.y <= 0.001f && rb.velocity.y >= -0.001f){
                isJumping = false;
                isFalling = false;
            }
        }
        //rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);



    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        if (dirX > 0)
            facingRight = false;
        else if (dirX < 0)
            facingRight = true;

        if (((facingRight) && (parentLocalScale.x < 0)) || ((!facingRight) && (parentLocalScale.x > 0)))
            parentLocalScale.x *= -1;

        transform.parent.localScale = parentLocalScale;
    }

    public void SetDirX(float dirX) { 
        this.dirX=dirX;
    }

}