using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MushroomMove : MonoBehaviour
{

    private float moveSpeed;
    private Rigidbody2D rb;
    private float colliderTimer;
    [SerializeField] private float dirX;
    [SerializeField] private float moveSpeedValue;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = moveSpeedValue;
        colliderTimer = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (colliderTimer<=0 && (collision.CompareTag("Ground") || collision.CompareTag("Enemy")))
        {
            colliderTimer = 0.2f;
            dirX *= -1f;
        }
    }

    void FixedUpdate()
    {
        
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        colliderTimer -= Time.deltaTime;
    }

    public void SetDirX(float dirX) { 
        this.dirX=dirX;
    }

}