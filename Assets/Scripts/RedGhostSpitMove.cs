using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RedGhostSpitMove : MonoBehaviour
{

    private float dirX;
    private float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 localScale;
    private float colliderTimer;
    private float destructionTimer;
    [SerializeField] private float moveSpeedValue;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private float KBForce;
    [SerializeField] private float destructionTimerValue;

    void Start()
    {
        localScale = transform.transform.localScale;
        rb = transform.GetComponent<Rigidbody2D>();
        moveSpeed = moveSpeedValue;
        colliderTimer = 0;
        destructionTimer = destructionTimerValue;
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.x <= transform.position.x) playerMove.RegisterHitKnockback(KBForce, true);
            else playerMove.RegisterHitKnockback(KBForce, false);
            playerMove.TakeDamage(10);
            Destroy(transform.gameObject);
        }
        else if (other.CompareTag("Ground"))Destroy(transform.gameObject);
        
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        destructionTimer -= Time.deltaTime;
        if (destructionTimer<=0) Destroy(transform.gameObject);
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

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    public void SetDirX(float dirX) { 
        this.dirX=dirX;
    }

}