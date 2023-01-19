using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class DeoxysMove : MonoBehaviour
{
    private float moveSpeed;
    private Rigidbody2D rb;
    private Vector3 localScale;
    private float shootTimer;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private bool isMoving;
    private bool isBoltAttacking;
    private bool isExplosionAttacking;
    private bool isDefensiveAttacking;
    private bool isFastAttacking;
    private int movesCounter;
    private int boltAttackCounter;
    private int explosionAttackCounter;
    private bool moveRight;
    private bool moveUp;
    private Vector3 nextPos;
    private float boltAttackTimer;
    private float explosionAttackTimer;
    private float pauseTimer;
    private float previousPauseTimer;
    private Vector2 changeFormPosition;
    private Transform boltAttackPositionTransform;
    private Vector2 leftPosition;
    private Vector2 rightPosition;
    private PlayerMove playerMove;
    private bool changedForm;
    private List<Transform> fastAttackPoints;
    private int fastAttackMovesCounter;
    private float hitTimer;
    private float changeFormTimer;
    private bool changingForm;
    private bool fastAttackStopping;
    private bool stoppedEarthquakeAttack;
    private bool inverseFastAttack;
    private bool phaseTwo;
    private bool phaseThree;
    private bool activated;
    private bool loadingNextLevel;
    private bool newPhase;
    private bool dead;
    [SerializeField]private int hp;
    [SerializeField]private float moveSpeedValue1;
    [SerializeField]private float moveSpeedValue2;
    [SerializeField]private float moveSpeedValue3;
    [SerializeField]private float fastAttackMoveSpeedValue;
    [SerializeField] private int boltAttackCounterValue;
    [SerializeField] private int explosionAttackCounterValue;
    [SerializeField] private float pauseDuration;
    [SerializeField] private float boltAttackTimerValue;
    [SerializeField] private float explosionAttackTimerValue;
    [SerializeField] private GameObject deoxysBoltAttackPrefab;
    [SerializeField] private GameObject deoxysExplosionAttackPrefab;
    [SerializeField] private float changeFormTimerValue;
    [SerializeField] private float KBForce;
    [SerializeField] private int damage;
    [SerializeField] private GameObject scytherPrefab;
    [SerializeField] private float nextLevelTimer;
    [SerializeField] private AudioClip getHitSound;
    // private bool b;

    void Start()
    {

        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isMoving=true;
        isBoltAttacking = false;
        isExplosionAttacking = false;
        isFastAttacking = false;
        isDefensiveAttacking = false;
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        movesCounter = 1;
        moveRight = true;
        pauseTimer = 0;
        leftPosition=transform.parent.Find("Waypoint1").transform.position;
        rightPosition=transform.parent.Find("Waypoint2").transform.position;
        changeFormPosition=transform.parent.Find("Waypoint3").transform.position;
        boltAttackPositionTransform=transform.Find("BoltAttackPosition").transform;
        moveSpeed = moveSpeedValue1;
        pauseTimer=pauseDuration;
        fastAttackPoints=new List<Transform>();
        for (int i=4;i<8;i++)fastAttackPoints.Add(transform.parent.Find("Waypoint"+i.ToString()).transform);
        inverseFastAttack=false;
    }

    void Update(){
        // if (pauseTimer>=0 && !isFastAttacking && !isDefensiveAttacking)anim.SetBool("isMoving",false);
        // else anim.SetBool("isMoving",true);
        anim.SetBool("isMoving",true);
    }

    void FixedUpdate()
    {

        // if (!b && Input.GetKey(KeyCode.P)){
        //     Hit(50);
        //     b=true;
        // }

        if (loadingNextLevel){
            if (nextLevelTimer<=0){
                SceneManager.LoadScene(1);
                Destroy(transform.parent.transform.gameObject);
            }
            else nextLevelTimer-=Time.deltaTime;
        }
        else {
            if (activated){

                if (pauseTimer <= 0){

                    if (isMoving){
                        if (moveRight) rb.transform.position = Vector2.MoveTowards(rb.transform.position, rightPosition, moveSpeed * Time.deltaTime);
                        else rb.transform.position = Vector2.MoveTowards(rb.transform.position, leftPosition, moveSpeed * Time.deltaTime);

                        if ((Vector2.Distance(transform.position,leftPosition)<=0.1f && !moveRight) || (Vector2.Distance(transform.position,rightPosition)<=0.1f && moveRight)){
                            
                            if (newPhase){
                                if ((phaseTwo && movesCounter==2) || (phaseThree && movesCounter==3))movesCounter=0;
                                newPhase=false;
                            }
                            else {
                                if (hp>150)movesCounter=movesCounter%2;
                                else if (hp>100)movesCounter=movesCounter%3;
                                else movesCounter=movesCounter%4;
                            }

                            if (movesCounter==0){
                                isBoltAttacking=true;
                                anim.SetTrigger("boltAttack");
                                boltAttackTimer=boltAttackTimerValue;
                            }
                            else if (movesCounter==1){
                                isExplosionAttacking=true;
                                anim.SetTrigger("explosionAttack");
                                explosionAttackTimer=explosionAttackTimerValue;
                            }
                            isMoving=false;
                            moveRight = !moveRight;
                            pauseTimer =pauseDuration;

                            movesCounter++;
                        }
                    }

                    if (isBoltAttacking){

                        if (boltAttackTimer<=0){
                            BoltAttack();
                            boltAttackTimer=boltAttackTimerValue;
                            boltAttackCounter++;
                            if (boltAttackCounter>=boltAttackCounterValue){
                                isBoltAttacking=false;
                                isMoving=true;
                                pauseTimer =pauseDuration;
                                boltAttackCounter=0;
                                anim.SetTrigger("boltAttackStopping");
                            }
                        }
                        else boltAttackTimer-=Time.deltaTime;

                    }

                    if (isExplosionAttacking)
                    {
                        if (explosionAttackTimer<=0){
                            ExplosionAttack();
                            explosionAttackTimer=explosionAttackTimerValue;
                            explosionAttackCounter++;
                            if (explosionAttackCounter>=explosionAttackCounterValue){
                                isExplosionAttacking=false;
                                if (hp<=150){
                                    isDefensiveAttacking=true;
                                    movesCounter++;
                                    moveUp=false;
                                    changedForm=false;
                                    changingForm=false;
                                    stoppedEarthquakeAttack=false;
                                }
                                else isMoving=true;
                                pauseTimer =pauseDuration;
                                explosionAttackCounter=0;
                                anim.SetTrigger("explosionAttackStopping");
                            }
                        }
                        else explosionAttackTimer-=Time.deltaTime;
                    }

                    if (isDefensiveAttacking)
                    {

                        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
                        current_animation = animatorinfo[0].clip.name;

                        if (changedForm){
                            if (moveUp){    
                                rb.transform.position = Vector2.MoveTowards(rb.transform.position, changeFormPosition, moveSpeed * Time.deltaTime);
                                if (stoppedEarthquakeAttack){
                                    isDefensiveAttacking=false;
                                    if (hp<=100){
                                        isFastAttacking=true;
                                        moveSpeed=fastAttackMoveSpeedValue;
                                        if (!inverseFastAttack)fastAttackMovesCounter=0;
                                        else fastAttackMovesCounter=3;
                                        changedForm=false;
                                        changingForm=false;
                                        fastAttackStopping=false;
                                    }   
                                    else isMoving=true;
                                    anim.SetTrigger("defensiveChangedForm");
                                    pauseTimer=pauseDuration;
                                    Debug.Log("DEFENSIVECHANGEFORM");
                                }
                                else if (Vector2.Distance(changeFormPosition,rb.transform.position)<=0.1f){
                                    anim.SetTrigger("earthquakeAttackStop");
                                    pauseTimer=pauseDuration;
                                    stoppedEarthquakeAttack=true;
                                    Debug.Log("EARTHQUAKEATTACKSTOP");
                                }
                            }
                            else {
                                rb.velocity=new Vector2(0,-moveSpeed);
                                if (previousPauseTimer>0 && pauseTimer<=0){
                                    anim.ResetTrigger("changeFormDefensive");
                                    Debug.Log("CHANGEFORMDEFENSIVERESET");
                                }
                            }
                        }
                        else if (!changingForm){
                            rb.transform.position = Vector2.MoveTowards(rb.transform.position, changeFormPosition, moveSpeed * Time.deltaTime);
                            if (Vector2.Distance(changeFormPosition,rb.transform.position)<=0.1f){
                                anim.SetTrigger("changeForm");
                                pauseTimer=pauseDuration;
                                changingForm=true;
                                Debug.Log("CHANGEFORM");
                            }
                        }

                        if (changingForm && current_animation.Equals("deoxys_change_form") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
                            anim.SetTrigger("changeFormDefensive");
                            pauseTimer=pauseDuration;
                            Debug.Log("CHANGEFORMDEFENSIVE");
                        }

                        if (current_animation.Equals("deoxys_change_form_defensive") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
                            changedForm=true;
                            pauseTimer=pauseDuration;
                            anim.SetTrigger("earthquakeAttack");
                            Debug.Log("EARTHQUAKEATTACK");
                        }
                    }

                    if (isFastAttacking){

                        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
                        current_animation = animatorinfo[0].clip.name;

                        if (changedForm){
                            if (fastAttackMovesCounter<4 && !inverseFastAttack){
                                rb.transform.position = Vector2.MoveTowards(rb.transform.position, fastAttackPoints[fastAttackMovesCounter].position, moveSpeed * Time.deltaTime);
                                    if (Vector2.Distance(fastAttackPoints[fastAttackMovesCounter].position,rb.transform.position)<=0.1f){
                                        fastAttackMovesCounter++;
                                        localScale.x *= -1;
                                    }
                            }
                            else if (fastAttackMovesCounter>=0 && inverseFastAttack){
                                rb.transform.position = Vector2.MoveTowards(rb.transform.position, fastAttackPoints[fastAttackMovesCounter].position, moveSpeed * Time.deltaTime);
                                    if (Vector2.Distance(fastAttackPoints[fastAttackMovesCounter].position,rb.transform.position)<=0.1f){
                                        fastAttackMovesCounter--;
                                        localScale.x *= -1;
                                    }
                            }
                            else {
                                rb.transform.position = Vector2.MoveTowards(rb.transform.position, changeFormPosition, moveSpeed * Time.deltaTime);
                                if (fastAttackStopping){
                                    if (current_animation.Equals("deoxys_fast_attack_stop") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
                                        anim.SetTrigger("fastAttackStop");
                                        pauseTimer=pauseDuration;
                                    }
                                    else if (current_animation.Equals("deoxys_fast_change_form") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
                                        anim.SetTrigger("fastChangedForm");
                                        pauseTimer=pauseDuration;
                                        isFastAttacking=false;
                                        isMoving=true;
                                        moveSpeed=moveSpeedValue3;
                                        changedForm=false;
                                        movesCounter++;
                                        inverseFastAttack=!inverseFastAttack;
                                    }
                                }
                                else if (Vector2.Distance(changeFormPosition,rb.transform.position)<=0.1f){
                                    anim.SetTrigger("fastAttackStopping");
                                    localScale.x *= -1;
                                    pauseTimer=pauseDuration;
                                    fastAttackStopping=true;
                                }
                            }
                        }
                        else if (!changingForm){
                            anim.SetTrigger("changeForm");
                            pauseTimer=pauseDuration;
                            changingForm=true;
                            Debug.Log(2);
                        }

                        if (changingForm && current_animation.Equals("deoxys_change_form") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
                            anim.SetTrigger("changeFormFast");
                            pauseTimer=pauseDuration;
                        }

                        if (current_animation.Equals("deoxys_change_form_fast") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
                            changedForm=true;
                            pauseTimer=pauseDuration;
                            anim.SetTrigger("fastAttack");
                        }

                    }

                    animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
                    current_animation = animatorinfo[0].clip.name;

                    if (current_animation.Equals("deoxys_bolt_attack_stop") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
                        anim.SetTrigger("boltAttackStop");
                        pauseTimer=pauseDuration;
                    }
                    if (current_animation.Equals("deoxys_explosion_attack_stop") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)anim.SetTrigger("explosionAttackStop");
                    
                    
                }

                pauseTimer -= Time.deltaTime;
                hitTimer-=Time.deltaTime;
            }
        }
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        
        if (!isFastAttacking){
            if (((!moveRight) && (localScale.x < 0)) || ((moveRight) && (localScale.x > 0)))
                localScale.x *= -1;
        }
        

        transform.localScale = localScale;
    }

    void BoltAttack()
    {
        GameObject temp;
        temp = Instantiate(deoxysBoltAttackPrefab) as GameObject;
        temp.transform.position=boltAttackPositionTransform.position;
        temp.GetComponent<DeoxysBoltAttackMove>().SetTargetPosition(transform.parent.transform.Find("BoltAttackTarget"+(boltAttackCounter+1)).transform.position);
    }

    void ExplosionAttack()
    {
        GameObject temp;
        temp = Instantiate(deoxysExplosionAttackPrefab) as GameObject;
    }

    public void Hit(int damage){
        hp=hp-damage;
        if (hp<=0 && !dead){
            if (GetComponent<AudioSource>()!=null)GetComponent<AudioSource>().Play();
            transform.GetComponent<SpriteRenderer>().color=new Color(0,0,0,0);
            loadingNextLevel=true;
            GameObject.Find("BossMusic").GetComponent<AudioSource>().Stop();
            CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins=playerMove.GetCoins().ToString();
            dead=true;
            CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.SaveToJSON();
        }
        else if (hp<=150 && !phaseTwo){
            moveSpeed=moveSpeedValue3;
            explosionAttackCounterValue++;
            phaseTwo=true;
            SpawnScythers();
            newPhase=true;
        }
        else if (hp<=100 && !phaseThree){
            moveSpeed=moveSpeedValue2;
            explosionAttackCounterValue++;
            phaseThree=true;
            SpawnScythers();
            newPhase=true;
        }
        if (!dead)AudioSource.PlayClipAtPoint(getHitSound, GameObject.Find("Camera").transform.position,0.75f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") && isDefensiveAttacking)
        {
            if (playerMove.IsGrounded())playerMove.Earthquake(7);
            pauseTimer=pauseDuration;
            moveUp=true;
            rb.velocity=Vector2.zero;
            anim.SetTrigger("earthquakeAttackStopping");
            Debug.Log("EARTHQUAKEATTACKSTOPPING");
        }

        if (collision.CompareTag("Player")){
            if (collision.transform.position.x <= transform.position.x) playerMove.RegisterHitKnockback(KBForce, true);
            else playerMove.RegisterHitKnockback(KBForce, false);
            playerMove.TakeDamage(damage);
        }
    }

    void SpawnScythers(){
        GameObject temp;
        temp = Instantiate(scytherPrefab) as GameObject;
        temp.transform.position=transform.parent.transform.Find("ScytherSpawnPoint1").position;
        temp = Instantiate(scytherPrefab) as GameObject;
        temp.transform.position=transform.parent.transform.Find("ScytherSpawnPoint2").position;
        temp = Instantiate(scytherPrefab) as GameObject;
        temp.transform.position=transform.parent.transform.Find("ScytherSpawnPoint3").position;
    }

    public void Activate(){
        activated=true;
    }

}