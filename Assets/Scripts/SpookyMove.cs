using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SpookyMove : MonoBehaviour
{
    private int hp;
    private float moveSpeed;
    private Rigidbody2D rb;
    private Vector3 localScale;
    private float shootTimer;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private bool isMagicAttacking1;
    private bool isMagicAttacking2;
    private bool isMeleeAttacking;
    private int meleeAttackCounter;
    private int magicAttack1Counter;
    private int magicAttack2Counter;
    private bool moveRight;
    private bool moveUp;
    private Vector3 nextPos;
    private float magicAttack1Timer;
    private float magicAttack2Timer;
    private float pauseTimer;
    private float previousPauseTimer;
    private Vector2 upPosition;
    private Transform magicAttack1PositionTransform;
    private float disappearBodyColliderTimer;
    private Collider2D bodyCollider;
    private Vector2 leftPosition;
    private Vector2 rightPosition;
    private PlayerMove playerMove;
    private List<Transform> magicAttack2PointsFirst;
    private List<Transform> magicAttack2PointsSecond;
    private float hitTimer;
    private float pacManPowerUpTimer;
    private GameObject pacManPowerUp;
    private bool activated;
    private bool loadingNextLevel;
    private bool dead;
    [SerializeField] private float moveSpeedValue1;
    [SerializeField] private float moveSpeedValue2;
    [SerializeField] private float moveSpeedValue3;
    [SerializeField] private int meleeAttackCounterValue;
    [SerializeField] private float pauseDuration;
    [SerializeField] private float magicAttack1TimerValue;
    [SerializeField] private float magicAttack2TimerValue;
    [SerializeField]private float disappearBodyColliderTimerValue;
    [SerializeField] private GameObject spookyMagicAttack1Prefab;
    [SerializeField] private GameObject spookyMagicAttack2Prefab;
    [SerializeField] private float hitTimerValue;
    [SerializeField] private int magicAttack1CounterValue;
    [SerializeField] private GameObject pacManPowerUpPrefab;
    [SerializeField] private float pacManPowerUpTimerValue;
    [SerializeField] private int damage;
    [SerializeField] private float KBForce;
    [SerializeField] private PlayerUnlocked playerUnlocked;
    [SerializeField] private float nextLevelTimer;
    [SerializeField] private AudioClip getHitSound;

    void Start()
    {

        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isMagicAttacking1 = false;
        isMagicAttacking2 = false;
        isMeleeAttacking = true;
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        meleeAttackCounter = 0;
        moveRight = true;
        pauseTimer = 0;
        leftPosition=transform.parent.Find("Waypoint1").transform.position;
        rightPosition=transform.parent.Find("Waypoint2").transform.position;
        upPosition=transform.parent.Find("Waypoint3").transform.position;
        magicAttack1PositionTransform=transform.Find("SpookyMagicAttack1Point").transform;
        hp=3;
        moveSpeed = moveSpeedValue1;
        bodyCollider=transform.Find("SpookyBodyCollider").GetComponent<Collider2D>();
        pauseTimer=pauseDuration;
        magicAttack2PointsFirst = new List<Transform>();
        magicAttack2PointsSecond = new List<Transform>();
        for (int i=1;i<14;i++){
            magicAttack2PointsFirst.Add(transform.parent.transform.Find("SpookyMagicAttack2Point"+i.ToString()).transform);
        }
        for (int i=14;i<28;i++){
            magicAttack2PointsSecond.Add(transform.parent.transform.Find("SpookyMagicAttack2Point"+i.ToString()).transform);
        }
        pacManPowerUpTimer=pacManPowerUpTimerValue;
    }

    void FixedUpdate()
    {       


        if (loadingNextLevel){
            if (nextLevelTimer<=0){
                SceneManager.LoadScene(1);
                Destroy(transform.parent.transform.gameObject);
            }
            else nextLevelTimer-=Time.deltaTime;
        }
        else {
            if (activated){
                if (pacManPowerUpTimer<=0){
                    if (pacManPowerUp==null){
                        pacManPowerUp=Instantiate(pacManPowerUpPrefab) as GameObject;
                        pacManPowerUp.transform.position = transform.position;
                    }
                    pacManPowerUpTimer=pacManPowerUpTimerValue;
                }
                else pacManPowerUpTimer-=Time.deltaTime;

                if (disappearBodyColliderTimer<=0)bodyCollider.enabled=true;
                else disappearBodyColliderTimer-=Time.deltaTime;

                if (pauseTimer <= 0){

                    if (previousPauseTimer>0 && isMeleeAttacking)anim.SetTrigger("melee");

                    if (isMeleeAttacking){

                        if (moveRight) rb.transform.position = Vector2.MoveTowards(rb.transform.position, rightPosition, moveSpeed * Time.deltaTime);
                        else rb.transform.position = Vector2.MoveTowards(rb.transform.position, leftPosition, moveSpeed * Time.deltaTime);

                        if ((transform.position.Equals(leftPosition) && !moveRight) || (transform.position.Equals(rightPosition) && moveRight)){
                            meleeAttackCounter++;
                            if (meleeAttackCounter >= meleeAttackCounterValue)
                            {
                                meleeAttackCounter = 0;
                                moveUp=true;
                                isMeleeAttacking=false;
                                magicAttack1Timer=magicAttack1TimerValue;
                                magicAttack2Timer=magicAttack2TimerValue;
                            }
                            moveRight = !moveRight;
                            pauseTimer =pauseDuration;
                            anim.SetTrigger("stop_melee");
                        }
                    }

                    if (!isMeleeAttacking && moveUp){
                        rb.transform.position = Vector2.MoveTowards(rb.transform.position, upPosition, moveSpeed * Time.deltaTime);
                        if (Vector2.Distance(rb.transform.position,upPosition)<=0.1f){
                            moveUp=false;
                            if (hp==1){
                                isMagicAttacking2=true;
                                magicAttack2Counter=0;
                                magicAttack1Timer=magicAttack1TimerValue;
                            }
                            else if (hp>1){
                                isMagicAttacking1=true;
                                magicAttack1Counter=0;
                                magicAttack2Timer=magicAttack2TimerValue;
                            }
                            pauseTimer =pauseDuration;
                        }
                    }

                    if (isMagicAttacking1)
                    {
                        anim.SetBool("isMagicAttacking",true);
                        if (magicAttack1Timer<=0){
                            MagicAttack1();
                            magicAttack1Timer=magicAttack1TimerValue;
                            magicAttack1Counter++;
                            if (magicAttack1Counter>=magicAttack1CounterValue){
                                isMagicAttacking1=false;
                                isMeleeAttacking=true;
                                pauseTimer =pauseDuration;
                                anim.SetBool("isMagicAttacking",false);
                                magicAttack1Counter=0;
                            }
                        }
                        else magicAttack1Timer-=Time.deltaTime;
                    }

                    if (isMagicAttacking2)
                    {
                        anim.SetBool("isMagicAttacking",true);
                        if (magicAttack2Timer<=0){
                            magicAttack2Timer=magicAttack2TimerValue;
                            magicAttack2Counter++;
                            if (magicAttack2Counter==1)MagicAttack2First();
                            else if (magicAttack2Counter==2){
                                MagicAttack2Second();
                                isMagicAttacking2=false;
                                isMagicAttacking1=true;
                                magicAttack1Timer=magicAttack1TimerValue;
                                pauseTimer =pauseDuration;
                                anim.SetBool("isMagicAttacking",false);
                            }
                        }
                        else magicAttack2Timer-=Time.deltaTime;
                    }

                }

                previousPauseTimer=pauseTimer;
                pauseTimer -= Time.deltaTime;
                hitTimer-=Time.deltaTime;

                animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
                current_animation = animatorinfo[0].clip.name;

                if (current_animation.Equals("spooky_melee_attack_stop") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)anim.SetTrigger("stop_moving");
            }
        }
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {

        if (((!moveRight) && (localScale.x < 0)) || ((moveRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    void MagicAttack1()
    {
        GameObject temp;
        temp = Instantiate(spookyMagicAttack1Prefab) as GameObject;
        temp.transform.position = magicAttack1PositionTransform.position;
    }

    void MagicAttack2First()
    {
        GameObject temp;
        foreach (Transform point in magicAttack2PointsFirst) {
            temp = Instantiate(spookyMagicAttack2Prefab) as GameObject;
            temp.transform.position = point.position;
        }
        
    }

    void MagicAttack2Second()
    {
        GameObject temp;
        foreach (Transform point in magicAttack2PointsSecond) {
            temp = Instantiate(spookyMagicAttack2Prefab) as GameObject;
            temp.transform.position = point.position;
        }
        
    }

    public void Hit(){

        if (!loadingNextLevel){
            hp--;
            if (hp<=0 && !dead){
                if (GetComponent<AudioSource>()!=null)GetComponent<AudioSource>().Play();
                int levelIndex=CurrentGameData.currentGameData.currentLevelIndex-3;
                if (levelIndex!=15 && levelIndex+1>int.Parse(CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.reachedLevel))CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.reachedLevel=(levelIndex+1).ToString();
                if (levelIndex==11 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedPikachu=="n"){
                    CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedPikachu="y";
                    CurrentGameData.currentGameData.newPlayerIndex=3;
                    playerUnlocked.ActivateAnimation();
                }
                transform.GetComponent<SpriteRenderer>().color=new Color(0,0,0,0);
                loadingNextLevel=true;
                GameObject.Find("BossMusic").transform.GetComponent<AudioSource>().Stop();
                CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins=playerMove.GetCoins().ToString();
                dead=true;
                CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.SaveToJSON();
            }
            else if (hp==1){
                moveSpeed=moveSpeedValue3;
            }
            else if (hp==2){
                moveSpeed=moveSpeedValue2;
            }
            pauseDuration=pauseDuration-1;
            magicAttack1CounterValue++;
            bodyCollider.enabled=false;
            disappearBodyColliderTimer=disappearBodyColliderTimerValue;
            if (!dead)AudioSource.PlayClipAtPoint(getHitSound, GameObject.Find("Camera").transform.position,1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            if (playerMove.ScaringGhosts()){
                if (hitTimer<=0){
                    Hit();
                    hitTimer=hitTimerValue;
                    playerMove.ResetEatGhosts();
                }
            }
            else {
                if (collision.transform.position.x <= transform.position.x) playerMove.RegisterHitKnockback(KBForce, true);
                else playerMove.RegisterHitKnockback(KBForce, false);
                playerMove.TakeDamage(damage);
            }
        }
    }

    public void Activate(){
        activated=true;
    }

}