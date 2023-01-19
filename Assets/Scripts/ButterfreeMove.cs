using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButterfreeMove : MonoBehaviour
{

    private float moveSpeed;
    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = false;
    private bool isAttacking;
    private Vector3 localScale;
    private float throwDustTimer;
    private GameObject dustGameObject;
    [SerializeField] private float moveSpeedValue;
    [SerializeField] private float dirX;
    [SerializeField] private float throwDustTimerValue;
    [SerializeField] private GameObject dustPrefab;

    void Start()
    {
        localScale = transform.parent.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        moveSpeed = moveSpeedValue;
        isAttacking = false;
    }


    void FixedUpdate()
    {
        if (isAttacking)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
            if (throwDustTimer <= 0) {
                throwDustTimer = throwDustTimerValue;
                dustGameObject = Instantiate(dustPrefab) as GameObject;
                dustGameObject.transform.position = new Vector2(transform.position.x, transform.position.y);
            }
            else throwDustTimer -= Time.deltaTime;
        }

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

    public bool AttackIsActivated() {
        return isAttacking;
    }

    public void Attack() {
        isAttacking = true;
        anim.SetTrigger("isAttacking");
        throwDustTimer = throwDustTimerValue;
    }


}