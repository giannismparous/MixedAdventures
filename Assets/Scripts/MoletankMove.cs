using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoletankMove : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private bool isMoving;
    [SerializeField] private float dirX;
    [SerializeField] private float moveSpeedValue;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        isMoving = false;
    }

    void FixedUpdate()
    {

        if (isMoving)rb.velocity = new Vector2(dirX * moveSpeedValue, rb.velocity.y);

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
        isMoving = true;
    }

    public bool isMovingTowardsPlayer() {
        return isMoving;
    }


}