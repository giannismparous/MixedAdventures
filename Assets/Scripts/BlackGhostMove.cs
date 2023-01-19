using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlackGhostMove : MonoBehaviour
{

    private float dirX;
    private float moveSpeed;
    private float distance;
    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private PlayerMove playerMove;
    private float spawnGhostTimer;
    public float distanceValue;
    public float moveSpeedValue;
    public float spawnGhostTimerValue;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dirX = -1f;
        moveSpeed = 0;
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
    }

    void Update() {
        if (distance <= distanceValue) anim.SetBool("isMoving", true);
        else anim.SetBool("isMoving",false);
    }

    void FixedUpdate()
    {

        distance = (float)Math.Sqrt(Math.Pow(playerMove.transform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(playerMove.transform.position.y - transform.parent.transform.position.y, 2));

        if (distance <= distanceValue)
        {
            if (playerMove.transform.position.x <= transform.position.x) dirX = -1;
            else dirX = 1;

            moveSpeed = moveSpeedValue;

            if (spawnGhostTimer <= 0)
            {
                spawnGhostTimer = spawnGhostTimerValue;
            }
            else spawnGhostTimer -= Time.deltaTime;
        }
        else
        {
            moveSpeed = 0;
            spawnGhostTimer = spawnGhostTimerValue;
        }

        if (rb.velocity.y <= 0.0001f && rb.velocity.y >= -0.0001f && !Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer))moveSpeed=0;

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

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