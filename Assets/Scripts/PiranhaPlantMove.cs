using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PiranhaPlantMove : MonoBehaviour
{

    private float moveSpeed;
    private Vector3 startingPosition;
    private Vector3 wentUpPosition;
    private Rigidbody2D rb;
    private bool wentUp;
    [SerializeField] private float moveSpeedValue;
    [SerializeField] private float goUpValue;


    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        moveSpeed = moveSpeedValue;
        startingPosition = new Vector2(rb.transform.position.x,rb.transform.position.y);
        wentUpPosition = new Vector2(startingPosition.x, startingPosition.y+goUpValue);
        wentUp = false;
    }

    void FixedUpdate()
    {
        
        if (!wentUp) rb.transform.position = Vector2.MoveTowards(rb.transform.position, wentUpPosition, moveSpeed * Time.deltaTime);
        else rb.transform.position = Vector2.MoveTowards(rb.transform.position, startingPosition, moveSpeed * Time.deltaTime);

        if (rb.transform.position == startingPosition || rb.transform.position == wentUpPosition)wentUp = !wentUp;

    }

}