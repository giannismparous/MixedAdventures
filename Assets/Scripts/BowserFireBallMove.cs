using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BowserFireBallMove : MonoBehaviour
{

    private float dirX;
    private float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 localScale;
    private Vector2 targetTransformPosition;
    [SerializeField] private float moveSpeedValue;
    [SerializeField] private float destroyTimer;


    void Start()
    {
        localScale = transform.localScale;
        rb = transform.GetComponent<Rigidbody2D>();
        dirX = 1f;
        moveSpeed = moveSpeedValue;
    }

    void FixedUpdate()
    {

        rb.transform.position = Vector2.MoveTowards(rb.transform.position, targetTransformPosition , moveSpeed * Time.deltaTime);

        if (destroyTimer<=0)Destroy(gameObject);
        else destroyTimer-=Time.deltaTime;
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

    public void SetTargetTransformPosition(Vector2 targetTransformPosition){
        this.targetTransformPosition=targetTransformPosition;
    }

    public void SetTransformRotation(float degrees){
        transform.rotation=Quaternion.Euler(0,0,degrees);
    }

}