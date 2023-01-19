using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharizardMove : MonoBehaviour
{

    private float moveSpeed;
    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float colliderTimer;
    private bool isMoving;
    private bool isAttacking;
    private Transform flamethrowerTransform;
    public float dirX;
    public float moveSpeedValue;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public GameObject flamethrowerPrefab;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        flamethrowerTransform = transform.parent.transform.Find("FlamethrowerPosition").transform;
        moveSpeed = 0;
        colliderTimer = 0;
        isMoving = false;
        isAttacking = false;
        groundCheck=transform.parent.transform.Find("GroundCheck").transform;
    }

    void FixedUpdate()
    {

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (isMoving && rb.velocity.y <= 0.001f && rb.velocity.y>=-0.001f && !Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer))
        {
            dirX *= -1;
            isMoving = false;
            UseFlamethrower();
        }

        colliderTimer -= Time.deltaTime;

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

    public void FoundPlayer() {
        moveSpeed = moveSpeedValue;
        isMoving = true;
        anim.SetTrigger("isMoving");
    }

    public void UseFlamethrower() {
        moveSpeed = 0;
        isAttacking = true;
        anim.SetTrigger("isAttacking");
        GameObject temp = Instantiate(flamethrowerPrefab) as GameObject;
        temp.transform.position = new Vector2(flamethrowerTransform.position.x, flamethrowerTransform.position.y);
        Flamethrower flamethower = temp.transform.GetComponent<Flamethrower>();
        flamethower.SetDirX(dirX);
        flamethower.SetCharizardMove(this);
    }

    public void StopFlamethrower() {
        isAttacking = false;
        anim.SetTrigger("stoppedAttacking");
    }

    public bool isMovingTowardsPlayer() {
        return isMoving;
    }

    public bool isUsingFlamethrower()
    {
        return isAttacking;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if ((collision.CompareTag("Ground")))
        {
            if (isMoving && rb.velocity.y <= 0.001f && rb.velocity.y>=-0.001f)
            {
                
                dirX *= -1;
                isMoving = false;
                UseFlamethrower();
            }
        }
    }
}