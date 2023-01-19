using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinyMove : MonoBehaviour
{

    private float dirX;
    private float moveSpeed;
    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float colliderTimer;
    private bool landed;
    private Collider2D feetCollider;
    [SerializeField] private LayerMask jumpableGround;
    

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dirX = -1f;
        moveSpeed = 3f;
        colliderTimer = 0;
        landed = false;
        feetCollider=transform.parent.transform.Find("Feet").GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (landed &&colliderTimer<=0 && (collision.CompareTag("Ground") || collision.CompareTag("Enemy")))
        {
            colliderTimer = 0.2f;
            dirX *= -1f;
        }
    }

    void FixedUpdate()
    {
        if (!landed && Physics2D.BoxCast(feetCollider.bounds.center,feetCollider.bounds.size,0f,Vector2.down,.25f,jumpableGround))
        {
            anim.SetTrigger("landed");
            landed = true;
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