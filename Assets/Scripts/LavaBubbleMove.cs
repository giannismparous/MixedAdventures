using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LavaBubbleMove : MonoBehaviour
{

    private float moveSpeed;
    private Vector3 startingPosition;
    private Vector3 wentUpPosition;
    private Rigidbody2D rb;
    private Animator anim;
    private bool wentUp;
    private float waitTimer;
    [SerializeField] private float moveSpeedValue;
    [SerializeField] private float goUpValue;
    [SerializeField] private float waitDuration;
    [SerializeField] private float initialDelay;


    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        moveSpeed = moveSpeedValue;
        startingPosition = new Vector2(rb.transform.position.x,rb.transform.position.y);
        wentUpPosition = new Vector2(startingPosition.x, startingPosition.y+goUpValue);
        wentUp = false;
        waitTimer = 0;
    }

    void Update() {

        if (initialDelay<=0){
            if (waitTimer <= 0)
            {
                if (wentUp) anim.SetBool("goingDown", true);
                else anim.SetBool("goingDown", false);
            }
        }
    }

    void FixedUpdate()
    {
        if (initialDelay<=0){
            if (waitTimer <= 0)
            {

                if (!wentUp) rb.transform.position = Vector2.MoveTowards(rb.transform.position, wentUpPosition, moveSpeed * Time.deltaTime);
                else rb.transform.position = Vector2.MoveTowards(rb.transform.position, startingPosition, moveSpeed * Time.deltaTime);

                if (rb.transform.position == startingPosition ||rb.transform.position == wentUpPosition)wentUp = !wentUp;

                if (rb.transform.position == startingPosition) waitTimer = waitDuration;
            }
            else waitTimer -= Time.deltaTime;
            }
        else initialDelay-=Time.deltaTime;
    }

}