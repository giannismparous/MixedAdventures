using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuyoonMove : MonoBehaviour
{

    private float moveSpeed;
    private float distance;
    private Rigidbody2D rb;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private bool facingRight = false;
    private Vector3 localScale;
    private Transform point1;
    private Transform point2;
    private Transform target;
    private bool extended;
    private bool extending;
    private float colliderTimer;
    private float extendTimer;
    private bool lowering;
    private Collider2D extendedJumpedCheckCollider;
    private Collider2D extendedBodyCollider;
    private Collider2D jumpedCheckCollider;
    private Collider2D bodyCollider;
    public float dirX;
    public float moveSpeedValue;
    public float extendTimerValue;

    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        point1 = transform.parent.transform.Find("Point1").transform;
        point2 = transform.parent.transform.Find("Point2").transform;
        target = point1;
        moveSpeed = moveSpeedValue;
        if (dirX < 0) target = point1;
        else target = point2;
        extended = true;
        extending = false;
        lowering = false;
        extendedBodyCollider = transform.Find("BuyoonBodyColliderExtended").GetComponent<Collider2D>();
        extendedJumpedCheckCollider = transform.Find("JumpedCheckExtended").GetComponent<Collider2D>();
        bodyCollider = transform.Find("BuyoonBodyCollider").GetComponent<Collider2D>();
        jumpedCheckCollider = transform.Find("JumpedCheck").GetComponent<Collider2D>();
        bodyCollider.enabled = false;
        jumpedCheckCollider.enabled = false;
        colliderTimer = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (colliderTimer <= 0 && (collision.CompareTag("Obstacle") || collision.CompareTag("Enemy")))
        {
            colliderTimer = 0.2f;
            dirX *= -1f;
            if (dirX<0)target=point1;
            else if (dirX>0)target=point2;
        }
    }

    void FixedUpdate()
    {
        if (!lowering && !extending)
        {
            if (transform.position.x <= point1.position.x && target.position.x == point1.position.x && dirX < 0)
            {
                target = point2;
                dirX = 1;
            }
            else if (transform.position.x >= point2.position.x && target.position.x == point2.position.x && dirX > 0)
            {
                target = point1;
                dirX = -1;
            }
        }

        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;

        if (lowering && current_animation.Equals("buyoon_lower") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) {
            lowering = false;
            moveSpeed = moveSpeedValue;
            anim.SetTrigger("move_lowered");
        }
        else if (extending && current_animation.Equals("buyoon_extend") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            extending = false;
            moveSpeed = moveSpeedValue;
            extended = true;
            anim.SetTrigger("move_extended");
        }

        if (!lowering && !extending && !extended) {
            if (extendTimer <= 0)
            {
                Extend();
                ResetExtendTimer();
            }
            else extendTimer -= Time.deltaTime;
        }

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

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

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    public void Lower() {
        moveSpeed = 0;
        rb.velocity = new Vector2(0, rb.velocity.y);
        if (!lowering || !extending)anim.SetTrigger("lower");
        lowering = true;
        extended = false;
        extendedBodyCollider.enabled = false;
        extendedJumpedCheckCollider.enabled = false;
        bodyCollider.enabled = true;
        jumpedCheckCollider.enabled = true;
    }

    void Extend(){ 
        moveSpeed = 0;
        rb.velocity = new Vector2(0, rb.velocity.y);
        anim.SetTrigger("extend");
        extending = true;
        extendedBodyCollider.enabled = true;
        extendedJumpedCheckCollider.enabled = true;
        bodyCollider.enabled = false;
        jumpedCheckCollider.enabled = false;
    }

    public bool IsExtended() {
        return extended;
    }

    public void ResetExtendTimer()
    {
        extendTimer = extendTimerValue;
    }

}