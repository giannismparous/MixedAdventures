using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SonicMove : MonoBehaviour

{
    private Rigidbody2D rb;
    private Animator anim;
    private float moveSpeed;
    private float dirX;
    private bool facingRight = true;
    private Vector3 localScale;
    private bool doubleJumped;

     private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            localScale = transform.localScale;
			moveSpeed = 5f;
		}

    private void Update()
        {

            dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;

            if (Input.GetButtonDown("Jump") && rb.velocity.y == 0)
                rb.AddForce(Vector2.up * 400f);

            if (Mathf.Abs(dirX) > 0 && rb.velocity.y == 0)
                anim.SetBool("isRunning", true);
            else
                anim.SetBool("isRunning", false);

        if (rb.velocity.y == 0)
        {
            
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
            doubleJumped = false;
        }

            if (rb.velocity.y > 0 && !doubleJumped)
            {
                
                if (Input.GetButtonDown("Jump")){
                    anim.SetBool("isDoubleJumping", true);
                    rb.AddForce(Vector2.up * 100f);
                    anim.SetBool("isJumping", false);
                    doubleJumped = true;
                }
                else anim.SetBool("isJumping", true);

            }

        if (rb.velocity.y > 0 && doubleJumped)
        {
            anim.SetBool("isDoubleJumping", true);
        }

        if (rb.velocity.y < 0)
            
            {
                if (Input.GetButtonDown("Jump") && !doubleJumped)
                {
                    anim.SetBool("isDoubleJumping", true);
                    rb.velocity = Vector2.zero;
                    rb.AddForce(Vector2.up * 200f);
                    anim.SetBool("isFalling", false);
                    doubleJumped = true;
                }
                else {
                    anim.SetBool("isJumping", false);
                    anim.SetBool("isDoubleJumping", false);
                    anim.SetBool("isFalling", true);
                }
            
            }

        }    

    private void FixedUpdate()
        {
            rb.velocity = new Vector2(dirX, rb.velocity.y);
        }

    private void LateUpdate()
        {
        if (dirX > 0)
            facingRight = true;
        else if(dirX < 0)
            facingRight = false;


        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
                localScale.x *= -1;

            transform.localScale = localScale;
        }        

    }