using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SnorlaxMove : MonoBehaviour
{

    private float moveSpeed;
    private Rigidbody2D rb;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float distance;
    private bool isSleeping;
    private bool isGoingToSleep;
    private bool isGettingUp;
    private PlayerMove playerMove;
    private Transform sleepingAnimationPositionTransform;
    [SerializeField] private GameObject sleepingAnimationGameObject;
    [SerializeField] private float dirX;
    [SerializeField] private float moveSpeedValue;
    [SerializeField] private GameObject sleepingAnimationPrefab;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        sleepingAnimationPositionTransform = transform.parent.Find("SleepingAnimationPosition").transform;
        isSleeping = true;
        moveSpeed = 0;
        sleepingAnimationGameObject = Instantiate(sleepingAnimationPrefab) as GameObject;
    }

    void FixedUpdate()
    {

        distance = (float)Math.Sqrt(Math.Pow(playerMove.transform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(playerMove.transform.position.y - transform.parent.transform.position.y, 2));

        if (!isSleeping && distance <= 5)
        {
            if (playerMove.transform.position.x < transform.parent.transform.position.x) dirX = -1;
            else dirX = 1;

        }
        else if (!isSleeping && !isGoingToSleep && distance > 10) {
            GoToSleep();
        }

        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;

        if (isGettingUp && current_animation.Equals("snorlax_get_up") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            isGettingUp = false;
            moveSpeed = moveSpeedValue;
            anim.SetTrigger("isMoving");
        }
        else if (isGoingToSleep && current_animation.Equals("snorlax_go_to_sleep") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) {
            isSleeping = true;
            isGoingToSleep = false;
            anim.SetTrigger("isSleeping");
            sleepingAnimationGameObject = Instantiate(sleepingAnimationPrefab) as GameObject;
        }

        if (sleepingAnimationGameObject != null) {
            sleepingAnimationGameObject.transform.position = new Vector2(sleepingAnimationPositionTransform.position.x, sleepingAnimationPositionTransform.position.y);
        }

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);


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

    public void GoToSleep() {
        isGoingToSleep = true;
        rb.velocity = new Vector2(0,rb.velocity.y);
        moveSpeed = 0;
        anim.SetTrigger("isGoingToSleep");
    }

    public void WakeUp() {
        isGettingUp = true;
        isSleeping = false;
        anim.SetTrigger("isGettingUp");
        if (sleepingAnimationGameObject != null) Destroy(sleepingAnimationGameObject);
    }

    public bool IsAwake() {
        return !isSleeping;
    }
}