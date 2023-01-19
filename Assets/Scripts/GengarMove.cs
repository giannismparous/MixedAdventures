using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GengarMove : MonoBehaviour
{

    public float dirX;
    public float moveSpeedValue;
    private float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private bool facadeMode;
    private bool isAttacking;
    private bool isRevealed;
    private Vector3 parentLocalScale;
    private float colliderTimer;
    private float facadeTimer;
    private float spitBallTimer;
    private int spitBallCounter;
    private int realPosition;
    private Collider2D jumpedCheckCollider;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public PlayerMove playerMove;
    public float pauseDuration;
    public float facadeDuration;
    public float spitBallTimerValue;
    public int numberOfSpitBalls;
    private float pauseTimer;
    private float fallRadius;
    private Animator anim;
    public GameObject gengarFacadePrefab;
    public GameObject gengarSpitBallPrefab;
    private System.Random rand;
    private List<GameObject> gengarFacadePrefabs = new List<GameObject>();
    private List<Transform> points;

    void Start()
    { 
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpedCheckCollider= transform.parent.transform.Find("JumpedCheck").GetComponent<BoxCollider2D>();
        moveSpeed = 0;
        colliderTimer = 0;
        pauseTimer = 0;
        fallRadius = 0.1f;
        rand = new System.Random();
        facadeMode = false;
        isAttacking = false;
        isRevealed = false;
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        //Physics2D.IgnoreCollision(player.transform.Find("MarioBody").GetComponent<BoxCollider2D>(), transform.parent.Find("Feet").GetComponent<BoxCollider2D>(), true);
        points = new List<Transform>();
        points.Add(transform.parent.transform.Find("Point1").transform);
        points.Add(transform.parent.transform.Find("Point2").transform);
        points.Add(transform.parent.transform.Find("Point3").transform);
        points.Add(transform.parent.transform.Find("Point4").transform);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (colliderTimer <= 0 && (collision.CompareTag("Ground") || collision.CompareTag("Enemy")) && moveSpeed>0)
        {
            colliderTimer = 0.2f;
            dirX *= -1f;
        }
    }

    void FixedUpdate()
    {

        if (IsUsingFacade()) {
            if (playerMove.transform.position.x <= transform.parent.position.x) dirX = -1;
            else dirX = 1;
        }

        if (!IsUsingFacade() && !IsSpitting() && rb.velocity.y <= 0.001f && rb.velocity.y >= -0.001f && !Physics2D.OverlapCircle(groundCheck.position, fallRadius, groundLayer))
        {
            dirX *= -1;
            pauseTimer = pauseDuration;
            rb.velocity = new Vector2(0, rb.velocity.y);
            moveSpeed = 0;
            anim.SetTrigger("stoppedMoving");
        }

        if (pauseTimer <= 0 && !IsUsingFacade() && !IsSpitting())
        {
            if (moveSpeed == 0) anim.SetTrigger("isMoving");
            moveSpeed = moveSpeedValue;
            fallRadius = 0.1f;
        }
        else 
        {
            fallRadius = 100f;
        }

        if (IsUsingFacade()) {
            if (facadeTimer <= 0)
            {
                facadeMode = false;
                isAttacking = true;
                anim.SetTrigger("isSpitting");
                spitBallCounter = 0;
                spitBallTimer = 0;
                jumpedCheckCollider.enabled = true;
                isRevealed = false;
            }
            else facadeTimer -= Time.deltaTime;
        }

        if (isAttacking) {
            if (spitBallTimer <= 0)
            {
                Spit();
                spitBallCounter++;
                if (spitBallCounter >= numberOfSpitBalls)
                {
                    isAttacking = false;
                    anim.SetTrigger("stoppedSpitting");
                    pauseTimer = 0;
                }
                else
                {
                    spitBallTimer = spitBallTimerValue;
                }
            }
            else spitBallTimer -= Time.deltaTime;
        }

        colliderTimer -= Time.deltaTime;
        pauseTimer -= Time.deltaTime;

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    void Spit() {
        GameObject tempSpitBall = Instantiate(gengarSpitBallPrefab) as GameObject;
        Transform attackPosTransform = transform.parent.Find("AttackPosition").transform;
        tempSpitBall.transform.position = new Vector2(attackPosTransform.position.x, attackPosTransform.position.y);
        tempSpitBall.GetComponent<GengarSpitBallMove>().SetDirX(dirX);
        Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), tempSpitBall.GetComponent<Collider2D>(),true);
        foreach (GameObject gameObj in gengarFacadePrefabs)
        {
            Destroy(gameObj);
        }
        gengarFacadePrefabs.Clear();
    }

    void LateUpdate()
    {
        if (pauseTimer <= 0 && !IsSpitting()) CheckWhereToFace();
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


    public void ActivateFacade()
    {

        List<Vector2> availablePointsPositions=new List<Vector2>();

        for (int i = 0; i < 5; i++)
        {
            if (i==2)continue;
            if (i>2)points[i-1].position=new Vector2(playerMove.transform.position.x - 6 + i * 3, playerMove.transform.position.y+0.5f);
            else points[i].position=new Vector2(playerMove.transform.position.x - 6 + i * 3, playerMove.transform.position.y+0.5f);
        }

        foreach (Transform point in points)
        {
            if (point.GetComponent<PointAvailability>().IsAvailable())
            {
                availablePointsPositions.Add((Vector2)point.position);
                //Debug.Log(point.GetComponent<PointAvailability>().IsAvailable()+"Availability");
            }
        }
         
        if (availablePointsPositions.Count==0)return;

        facadeMode = true;
        facadeTimer = facadeDuration;
        moveSpeed = 0;
        anim.SetTrigger("isUsingFacade");
        if (pauseTimer >= 0)
        {
            pauseTimer = 0;
            dirX = -1;
        }
        jumpedCheckCollider.enabled = false;
        GameObject temp;
        realPosition = rand.Next() % availablePointsPositions.Count;
        transform.parent.position = availablePointsPositions[realPosition];
        for (int i = 0; i < availablePointsPositions.Count; i++)
        {
            if (i == realPosition)continue;
            else
            {
                temp = Instantiate(gengarFacadePrefab) as GameObject;
                temp.transform.position = availablePointsPositions[i];
                gengarFacadePrefabs.Add(temp);
                //Debug.Log("Facade");
            }

        }

    }

    public bool IsUsingFacade() {
        return facadeMode;
    }

    public bool IsSpitting() {
        return isAttacking;
    }

    public bool IsRevealed() {
        return isRevealed;
    }

    public void Reveal() {
        isRevealed = true;
        anim.SetTrigger("revealed");
    }

    public void SetDirX(float dirX)
    {
        this.dirX= dirX;
    }
}