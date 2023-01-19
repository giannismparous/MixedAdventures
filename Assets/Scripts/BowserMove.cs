using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BowserMove : MonoBehaviour
{

    private float moveSpeed;
    private int dirX=1;
    private bool facingRight = false;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private Vector3 parentLocalScale;
    private Transform fireBallsPositionTransform;
    private Vector2 playerTransformPositionForFireballs;
    private PlayerMove playerMove;
    private float distance;
    private float fireAttackTimer;
    private float fireBallTimer;
    private float jumpTimer;
    private int currentAttackType;
    private bool noAttacks;
    private int fireBallCounter;
    private GameObject temp;
    private bool moving;
    [SerializeField] private float moveSpeedValue1;
    [SerializeField] private float moveSpeedValue2;
    [SerializeField] private float distanceValue;
    [SerializeField] private float jumpForce;
    [SerializeField] private float earthquakeForce;
    [SerializeField] private float fireAttackTimerValue;
    [SerializeField] private float fireBallTimerValue;
    [SerializeField] private float jumpTimerValue;
    [SerializeField] private GameObject bowserFireBallPrefab;
    

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        spriteRenderer=GetComponent<SpriteRenderer>();
        anim=GetComponent<Animator>();
        rb=transform.parent.transform.GetComponent<Rigidbody2D>();
        fireBallsPositionTransform=transform.parent.transform.Find("FireBallsPosition").transform;
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        currentAttackType=0;
        moving=false;
        anim.enabled=false;
        spriteRenderer.enabled=false;
    }

    void FixedUpdate()
    {
        if (transform.position.x<=playerMove.transform.position.x){
            moving=true;
            spriteRenderer.enabled=true;
            anim.enabled=true;
        }

        if (moving){
            distance = Math.Abs(playerMove.transform.position.x-transform.position.x);
            if (!noAttacks){
                if (distance<=distanceValue){
                    moveSpeed=moveSpeedValue1;
                    if (currentAttackType==0){
                        if (fireAttackTimer<=0){
                            anim.SetTrigger("fire_attack");
                            noAttacks=true;
                            currentAttackType=1;
                            jumpTimer=jumpTimerValue;
                            moveSpeed=0;
                            fireBallCounter=0;
                        }
                        else fireAttackTimer-=Time.deltaTime;
                    }
                    else if (currentAttackType==1){
                        if (jumpTimer<=0){
                            anim.SetTrigger("jumping");
                            noAttacks=true;
                            currentAttackType=0;
                            fireAttackTimer=fireAttackTimerValue;
                            moveSpeed=0;
                            rb.velocity=new Vector2(0,jumpForce);
                        }
                        else jumpTimer-=Time.deltaTime;
                    }
                }
                else moveSpeed=moveSpeedValue2;
            }

            animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
            current_animation = animatorinfo[0].clip.name;
    
            if (current_animation.Equals("bowser_jump") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && rb.velocity.y<=0)anim.SetTrigger("falling");
            else if(current_animation.Equals("bowser_fire_ball_attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
                if (fireBallCounter<3){
                    if (fireBallTimer<=0){
                        if (fireBallCounter==0)playerTransformPositionForFireballs=new Vector2(playerMove.transform.position.x+100,playerMove.transform.position.y);
                        temp = Instantiate(bowserFireBallPrefab) as GameObject;
                        temp.transform.position = new Vector2(fireBallsPositionTransform.position.x, fireBallsPositionTransform.position.y);
                        temp.GetComponent<BowserFireBallMove>().SetTargetTransformPosition(new Vector2(playerTransformPositionForFireballs.x,playerTransformPositionForFireballs.y+ (fireBallCounter-1)*50));
                        temp.GetComponent<BowserFireBallMove>().SetTransformRotation((float)Math.Atan2( playerTransformPositionForFireballs.y+ (fireBallCounter-1)*50 - fireBallsPositionTransform.position.y, playerTransformPositionForFireballs.x - fireBallsPositionTransform.position.x ) * (float)( 180 / Math.PI ));
                        fireBallCounter++;
                        fireBallTimer=fireBallTimerValue;
                    }
                    else fireBallTimer-=Time.deltaTime;
                }
                else anim.SetTrigger("fire_attack_recovering");
            }
            else if(current_animation.Equals("bowser_fall") && rb.velocity.y<=0.0001f && rb.velocity.y>=-0.0001f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
                anim.SetTrigger("moving");
                noAttacks=false;
                if (playerMove.IsGrounded())playerMove.Earthquake(7);
            }
            else if(current_animation.Equals("bowser_fire_ball_recover") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
                anim.SetTrigger("moving");
                noAttacks=false;
            }

            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        }
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

}
