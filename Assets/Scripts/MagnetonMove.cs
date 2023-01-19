using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagnetonMove : MonoBehaviour
{

    private float dirX;
    private float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private float distance;
    private Vector3 parentLocalScale;
    private Animator anim;
    private float attackTimer;
    private GameObject magnetonElectricBallGameObject;
    private PlayerMove playerMove;
    [SerializeField] private float attackTimerValue;
    [SerializeField] private GameObject magnetonElectricBallPrefab;
    [SerializeField] private float moveSpeedValue;


    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        dirX = -1f;
        moveSpeed = moveSpeedValue;
        anim = GetComponent<Animator>();
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
    }

    void FixedUpdate()
    {

        distance = (float)Math.Sqrt(Math.Pow(playerMove.transform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(playerMove.transform.position.y - transform.parent.transform.position.y, 2));


        if (distance <= 10)
        {
            if (playerMove.transform.position.x < rb.position.x) dirX = -1;
            else if (playerMove.transform.position.x > rb.position.x) dirX = 1;
            rb.transform.position = Vector2.MoveTowards(rb.transform.position, playerMove.transform.position, moveSpeed * Time.deltaTime);
            if (attackTimer <= 0 && magnetonElectricBallGameObject==null)
            {
                attackTimer = attackTimerValue;
                magnetonElectricBallGameObject = Instantiate(magnetonElectricBallPrefab) as GameObject;
            }
            else attackTimer -= Time.deltaTime;
        }
        else attackTimer = attackTimerValue;

        if (magnetonElectricBallGameObject != null) {
            magnetonElectricBallGameObject.transform.position = new Vector2(transform.position.x,transform.position.y);
        }
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