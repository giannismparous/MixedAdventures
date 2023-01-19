using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class RobotnikMove : MonoBehaviour
{
    private int hp;
    private float moveSpeed;
    private Rigidbody2D rb;
    private Vector3 localScale;
    private float shootTimer;
    private Animator anim;
    private bool isShootingAttacking;
    private bool isFallingAttacking;
    private float moveCounter;
    private bool moveRight;
    private bool moveDown;
    private Vector3 nextPos;
    private float parabola;
    private float distance;
    private float shootTimerValue;
    private float pauseTimer;
    private int attackType;
    private Vector2 downPosition;
    private Vector2 buzzbomberPosition1;
    private Vector2 buzzbomberPosition2; 
    private Vector2 buzzbomberPosition3;
    private Transform shootPositionTransform;
    private float disappearBodyColliderTimer;
    private Collider2D bodyCollider;
    //parabola
    private float arcHeight;
    private Vector2 leftPosition;
    private float progress;
    private Vector2 rightPosition;
    private float stepSize;
    private PlayerMove playerMove;
    private bool activated;
    private bool loadingNextLevel;
    private bool dead;
    [SerializeField] private float moveSpeedValue1;
    [SerializeField] private float moveSpeedValue2;
    [SerializeField] private float moveSpeedValue3;
    [SerializeField] private int movementsBeforeAttack;
    [SerializeField] private float pauseDuration;
    [SerializeField] private float shootTimerValue1;
    [SerializeField] private float shootTimerValue2;
    [SerializeField] private float shootTimerValue3;
    [SerializeField]private float disappearBodyColliderTimerValue;
    [SerializeField] private GameObject buzzbomberPrefab;
    [SerializeField] private GameObject condorBomberBulletPrefab;
    [SerializeField] private PlayerUnlocked playerUnlocked;
    [SerializeField] private float nextLevelTimer;
    [SerializeField] private AudioClip getHitSound;

    void Start()
    {

        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isShootingAttacking = false;
        isFallingAttacking = false;
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        moveCounter = 0;
        moveRight = true;
        pauseTimer = 0;
        leftPosition=transform.parent.Find("Waypoint1").transform.position;
        rightPosition=transform.parent.Find("Waypoint2").transform.position;
        downPosition=transform.parent.Find("Waypoint3").transform.position;
        buzzbomberPosition1=transform.parent.Find("BuzzbomberSpawnPoint1").transform.position;
        buzzbomberPosition2=transform.parent.Find("BuzzbomberSpawnPoint2").transform.position;
        buzzbomberPosition3=transform.parent.Find("BuzzbomberSpawnPoint3").transform.position;
        shootPositionTransform=transform.Find("ShootPosition").transform;
        attackType=1;
        hp=3;
        moveSpeed = moveSpeedValue1;
        bodyCollider=transform.Find("RobotnikBodyCollider").GetComponent<Collider2D>();
        shootTimerValue=shootTimerValue1;
    }

    void Update() {

        if (isShootingAttacking)
        {
            anim.SetBool("isShootingAttacking", true);
            anim.SetBool("isFallingAttacking", false);
        }
        else if (isFallingAttacking)
        {
            anim.SetBool("isShootingAttacking", false);
            anim.SetBool("isFallingAttacking", true);
        }
        else
        {
            anim.SetBool("isShootingAttacking", false);
            anim.SetBool("isFallingAttacking", false);
        }

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

            if (disappearBodyColliderTimer<=0)bodyCollider.enabled=true;
            else disappearBodyColliderTimer-=Time.deltaTime;

            if (pauseTimer <= 0){

                if (!isFallingAttacking && !isShootingAttacking){
                    if (moveRight) rb.transform.position = Vector2.MoveTowards(rb.transform.position, rightPosition, moveSpeed * Time.deltaTime);
                    else rb.transform.position = Vector2.MoveTowards(rb.transform.position, leftPosition, moveSpeed * Time.deltaTime);
                }

                if ((transform.position.Equals(leftPosition) && !moveRight) || (transform.position.Equals(rightPosition) && moveRight))
                {
                    moveCounter++;
                    if (moveCounter >= movementsBeforeAttack)
                    {
                        moveCounter = 0;
                        if (attackType==0){
                            isFallingAttacking = true;
                            distance = Vector3.Distance(leftPosition, rightPosition);
                            // This is one divided by the total flight duration, to help convert it to 0-1 progress.
                            stepSize = moveSpeed / distance;
                            progress = 0;
                            arcHeight = transform.position.y - playerMove.transform.position.y;
                            attackType=1;
                        }
                        else if (attackType==1){
                            isShootingAttacking = true;
                            moveDown=true;
                            shootTimer=shootTimerValue;
                            attackType=0;
                        }
                        
                    }
                    moveRight = !moveRight;
                    pauseTimer =pauseDuration;
                }

                if (isFallingAttacking && !isShootingAttacking)
                {
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
                        moveRight=!moveRight;
                        isFallingAttacking = false;
                        pauseTimer=pauseDuration;
                    }
                }

                if (isShootingAttacking && !isFallingAttacking) {

                    if (moveDown) rb.velocity=new Vector2(0,-moveSpeed);
                    else rb.velocity=new Vector2(0,moveSpeed);
                    if (transform.position.y<=downPosition.y+0.1f && transform.position.y>=downPosition.y-0.1f)moveDown=false;
                    else if (moveDown==false && transform.position.y<=leftPosition.y+0.1f && transform.position.y>=leftPosition.y-0.1f){
                        isShootingAttacking=false;
                        rb.velocity=Vector2.zero;
                        pauseTimer=pauseDuration;
                    }

                    if (shootTimer<=0)Shoot();
                    else shootTimer-=Time.deltaTime;
                }
            }
            else pauseTimer -= Time.deltaTime;
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

        if (isFallingAttacking || isShootingAttacking)localScale.x*=-1;
        transform.localScale = localScale;
    }

    void SpawnBuzzbombers()
    {
        GameObject temp;
        temp = Instantiate(buzzbomberPrefab) as GameObject;
        temp.transform.position = buzzbomberPosition1;
        temp.transform.Find("BuzzbomberBody").GetComponent<BuzzbomberController>().SetDistanceValue(100);
        temp = Instantiate(buzzbomberPrefab) as GameObject;
        temp.transform.position = buzzbomberPosition2;
        temp.transform.Find("BuzzbomberBody").GetComponent<BuzzbomberController>().SetDistanceValue(100);
        if (hp<=2){
            temp = Instantiate(buzzbomberPrefab) as GameObject;
            temp.transform.position = buzzbomberPosition3;
            temp.transform.Find("BuzzbomberBody").GetComponent<BuzzbomberController>().SetDistanceValue(100);
        }
    }

    void Shoot()
    {
        GameObject temp;
        temp = Instantiate(condorBomberBulletPrefab) as GameObject;
        temp.transform.position=shootPositionTransform.position;
        if (moveRight)temp.GetComponent<CondorBomberBulletMove>().SetDirX(1);
        else temp.GetComponent<CondorBomberBulletMove>().SetDirX(-1);
        shootTimer=shootTimerValue;
    }

    public void Hit(){
        hp--;
        if (hp<=0 && !dead){
            if (GetComponent<AudioSource>()!=null)GetComponent<AudioSource>().Play();
            int levelIndex=CurrentGameData.currentGameData.currentLevelIndex-3;
            if (levelIndex!=15 && levelIndex+1>int.Parse(CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.reachedLevel))CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.reachedLevel=(levelIndex+1).ToString();
            if (levelIndex==7 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedPacMan=="n"){
                CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedPacMan="y";
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
            shootTimerValue=shootTimerValue3;
        }
        else if (hp==2){
            moveSpeed=moveSpeedValue2;
            shootTimerValue=shootTimerValue2;
        }
        bodyCollider.enabled=false;
        disappearBodyColliderTimer=disappearBodyColliderTimerValue;
        if (!dead){
            AudioSource.PlayClipAtPoint(getHitSound, GameObject.Find("Camera").transform.position,1);
            SpawnBuzzbombers();
        }
    }

    public void Activate(){
        activated=true;
    }
}