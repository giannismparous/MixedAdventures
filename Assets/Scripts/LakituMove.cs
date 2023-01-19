using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LakituMove : MonoBehaviour
{

    private float moveSpeed;
    private Rigidbody2D rb;
    private Vector3 parentLocalScale;
    private float colliderTimer;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private System.Random rand;
    private bool isSpinnyAttacking;
    private bool isFallingAttacking;
    private float moveCounter;
    private bool moveRight;
    private Vector3 nextPos;
    private float parabola;
    private float distance;
    private float pauseTimer;
    private Vector2 initialPos;
    private bool active;
    [SerializeField] private float dirX;
    [SerializeField] private float moveSpeedValue1;
    [SerializeField] private float moveSpeedValue2;
    [SerializeField] private GameObject spinyPrefab;
    [SerializeField] private float AttackThrowXUpper;
    [SerializeField] private float AttackThrowXLower;
    [SerializeField] private float AttackThrowYUpper;
    [SerializeField] private float AttackThrowYLower;
    [SerializeField] private float marioRangeX;
    [SerializeField] private float marioRangeY;
    [SerializeField] private int movementsBeforeSpiny;
    [SerializeField] private float pauseDuration;
    //parabola
    private float arcHeight;
    private Vector2 leftPosition;
    private float progress;
    private Vector2 rightPosition;
    private float stepSize;
    private PlayerMove playerMove;

    void Start()
    {

        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        colliderTimer = 0;
        rand = new System.Random();
        isSpinnyAttacking = false;
        isFallingAttacking = false;
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        // leftPosition = new Vector2(playerMove.transform.parent.transform.position.x-marioRangeX,playerMove.transform.parent.transform.position.y+marioRangeY);
        // transform.parent.transform.position = leftPosition;
        // rightPosition = new Vector2(playerMove.transform.parent.transform.position.x+marioRangeX, playerMove.transform.parent.transform.position.y+marioRangeY);
        moveCounter = 0;
        moveRight = true;
        pauseTimer = 0;
        initialPos=transform.position;
    }

    void Update() {

        if (isSpinnyAttacking)
        {
            anim.SetBool("lakituSpinyAttacking", true);
            anim.SetBool("lakituFallingAttacking", false);
        }
        else if (isFallingAttacking)
        {
            anim.SetBool("lakituSpinyAttacking", false);
            anim.SetBool("lakituFallingAttacking", true);
        }
        else
        {
            anim.SetBool("lakituSpinyAttacking", false);
            anim.SetBool("lakituFallingAttacking", false);

        }

        
    }

    void FixedUpdate()
    {
        if (active){
            if (pauseTimer <= 0)
            {
                if (isFallingAttacking && !isSpinnyAttacking)
                {

                    moveSpeed = moveSpeedValue2;

                    // Increment our progress from 0 at the start, to 1 when we arrive.
                    progress = Mathf.Min(progress + Time.deltaTime * stepSize, 1.0f);

                    // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
                    parabola = 1.0f - 4.0f * (progress - 0.5f) * (progress - 0.5f);

                    // Travel in a straight line from our start position to the targetPosition.        
                    if (moveRight) nextPos = Vector2.Lerp(leftPosition, rightPosition, progress);
                    else nextPos = Vector2.Lerp(rightPosition, leftPosition, progress);

                    // Then add a vertical arc in excess of this.
                    nextPos.y += (-1) * parabola * arcHeight;

                    rb.transform.position = nextPos;

                    if (progress == 1)
                    {
                        if (playerMove.transform.parent.transform.position.x < rb.transform.position.x) moveRight = false;
                        else moveRight = true;
                        isFallingAttacking = false;
                    }
                }
                else
                {

                    leftPosition = new Vector2(playerMove.transform.parent.transform.position.x - marioRangeX, playerMove.transform.parent.transform.position.y + marioRangeY);
                    rightPosition = new Vector2(playerMove.transform.parent.transform.position.x + marioRangeX, playerMove.transform.parent.transform.position.y + marioRangeY);

                    moveSpeed = moveSpeedValue1;

                    if (moveRight) rb.transform.position = Vector2.MoveTowards(rb.transform.position, rightPosition, moveSpeed * Time.deltaTime);
                    else rb.transform.position = Vector2.MoveTowards(rb.transform.position, leftPosition, moveSpeed * Time.deltaTime);

                    if (transform.position.Equals(leftPosition) || transform.position.Equals(rightPosition))
                    {
                        moveCounter++;
                        if (moveCounter >= movementsBeforeSpiny && !isSpinnyAttacking)
                        {
                            moveCounter = 0;
                            isFallingAttacking = true;
                            distance = Vector3.Distance(leftPosition, rightPosition);
                            // This is one divided by the total flight duration, to help convert it to 0-1 progress.
                            stepSize = moveSpeed / distance;
                            progress = 0;
                            arcHeight = transform.position.y - playerMove.transform.position.y;
                        }
                        moveRight = !moveRight;
                        pauseTimer = (float)(pauseDuration + (rand.NextDouble()-0.5f));
                    }
                }
            }
            else pauseTimer -= Time.deltaTime;

            if (isSpinnyAttacking) {
                animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
                current_animation = animatorinfo[0].clip.name;
                if (current_animation.Equals("lakitu_spiny_attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1) { 
                    ThrowSpiny();
                    isSpinnyAttacking = false;
                }
            }

            colliderTimer -= Time.deltaTime;
        }
        else rb.transform.position=Vector2.MoveTowards(rb.transform.position,initialPos,moveSpeed);


    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {

        if (((!moveRight) && (parentLocalScale.x < 0)) || ((moveRight) && (parentLocalScale.x > 0)))
            parentLocalScale.x *= -1;

        transform.parent.localScale = parentLocalScale;
    }

    void ThrowSpiny() {
        GameObject tempSpiny = Instantiate(spinyPrefab) as GameObject;
        Transform attackPosTransform = transform.parent.Find("SpinyAttackPosition").transform;
        tempSpiny.transform.position = new Vector2(attackPosTransform.position.x, attackPosTransform.position.y);
        float tempX = (float)(rand.NextDouble() * (AttackThrowXUpper - AttackThrowXLower) + AttackThrowXLower);
        float tempY = (float)(rand.NextDouble() * (AttackThrowYUpper - AttackThrowYLower) + AttackThrowYLower);
        if (!moveRight) tempSpiny.GetComponent<Rigidbody2D>().velocity = new Vector2(-tempX, tempY);
        else tempSpiny.GetComponent<Rigidbody2D>().velocity = new Vector2(tempX, tempY);
        // if (playerMove.transform.position.x<=transform.position.x)tempSpiny.GetComponent<Rigidbody2D>().velocity = new Vector2(-tempX, tempY);
        // else tempSpiny.GetComponent<Rigidbody2D>().velocity = new Vector2(tempX, tempY);
    }

    public void SpinyAttack() {
        isSpinnyAttacking = true;
    }

    public void SetDirX(float dirX)
    {
        this.dirX= dirX;
    }

    public bool GetIsFallingAttacking() {
        return isFallingAttacking;
    }

    public void Active(){
        active=true;
    }

    public void Inactive(){
        active=false;
        moveSpeed=moveSpeedValue1;
        colliderTimer = 0;
        isSpinnyAttacking = false;
        isFallingAttacking = false;
        moveCounter = 0;
        pauseTimer = 0;
    }

    public bool IsActive(){
        return active;
    }
}