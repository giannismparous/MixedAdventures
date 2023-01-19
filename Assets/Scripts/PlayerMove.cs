using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerMove : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private float moveSpeed;
    private float dirX;
    private bool facingRight = true;
    private Vector3 localScale;
    private bool doubleJumped;
    private float hitKBForce;
    private float hitKBCounter;
    private float explosionKBCounter;
    private bool hitKBFromRight;
    private bool stunned;
    private float stunTimer;
    private Collider2D playerFeetCollider;
    private PlayerCheckForHill playerCheckForHill;
    private Vector3 currentRotation;
    private bool updatedRotation;
    private Text coinCounterText;
    private int coinsNum;
    private bool inWater;
    private float eatGhostsTimer;
    private float jumpHighTimer;
    private float moveFastTimer;
    private int shieldLife;
    private GameObject sonicShield;
    private float shootTimer;
    private bool dead;
    private bool escape;
    private GameObject healthbarCanvas;
    [SerializeField]private AudioSource popSound;
    [SerializeField]private AudioSource chasingGhostsSound;
    [SerializeField] private float moveSpeedValue;
    [SerializeField] private float sprintValue;
    [SerializeField] private float explosionKBDuration;
    [SerializeField] private float hitKBDuration;
    [SerializeField] private float jumpForce;
    [SerializeField] HealthBar healthbar;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private int maxHealth;
    [SerializeField] private float eatGhostsTimerValue;
    [SerializeField] private float moveFasterByValue;
    [SerializeField] private float jumpHigherByValue;
    [SerializeField] private float jumpHighTimerValue;
    [SerializeField] private float moveFastTimerValue;
    [SerializeField] private GameObject marioFireBallPrefab;
    [SerializeField] private GameObject pikachuElectroBallPrefab;
    [SerializeField] private GameObject sonicShieldPrefab;
    [SerializeField] private float shootTimerValue;
    [SerializeField] private AudioClip fireballSound;
    [SerializeField] private AudioClip electroballSound;
    [SerializeField] private GameObject bananaPrefab;
    [SerializeField] private GameObject cupPrefab;
    [SerializeField] private bool mitsos;
    [SerializeField] private bool antigoni;

    //powerups
    private bool canFireBall;
    private bool canJumpHigh;
    private bool canShield;
    private bool canMoveFast;
    private bool canEatGhosts;
    private bool canElectroBall;

    //disappear
    private GameObject disappearPrefab;

     private void Start()
        {
            rb = transform.parent.GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            localScale = transform.localScale;
            moveSpeed = moveSpeedValue;
            playerFeetCollider=transform.parent.transform.Find("Feet").GetComponent<Collider2D>();
            doubleJumped=false;
            playerCheckForHill=transform.parent.transform.Find("CheckForGround").GetComponent<PlayerCheckForHill>();
            currentRotation=new Vector3(0,0,0);
            updatedRotation=true;
            coinCounterText=GameObject.Find("Canvas").transform.Find("CoinsCounter").GetComponent<Text>();
            coinsNum=int.Parse(CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins);
            inWater=false;
            ResetPowerUps();
            UpdateCoinCounter();
            disappearPrefab = (GameObject)Resources.Load("DisappearAnimation");
            healthbarCanvas=transform.parent.transform.Find("Canvas").gameObject;
		}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log(other.gameObject.name+" collider with player.");
        }

        if (other.CompareTag("Coin"))
        {
            coinsNum++;
            UpdateCoinCounter();
        }
    }

    private void Update(){

        if (!escape){

            if (!dead){
                        
                if (!updatedRotation){
                    RotatePlayer(currentRotation);
                }
            //Debug
                // if (Input.GetKeyDown(KeyCode.DownArrow)) {
                //     TakeDamage(20);
                // }
                // if (Input.GetKeyDown(KeyCode.UpArrow))
                // {
                //     Heal(20);
                // }

                if (!stunned){
                dirX = Input.GetAxisRaw("Horizontal");

                if (Input.GetKey(KeyCode.LeftShift)) {
                    //anim.SetBool("isSprinting", true);
                    moveSpeed=sprintValue;
                }
                else {
                    //anim.SetBool("isSprinting", false);
                    moveSpeed=moveSpeedValue;
                }
                

                if (Input.GetKeyDown(KeyCode.Space) && (IsGrounded() || !doubleJumped || inWater)){
                    if (rb.velocity.y>=0.01f || rb.velocity.y<=-0.01f){
                        if (!inWater)doubleJumped=true;
                        rb.velocity=Vector2.zero;
                    }
                    rb.AddForce(Vector2.up * jumpForce);
                }
                
                if (playerCheckForHill.IsOnHill() && !Input.GetKeyDown(KeyCode.Space)){
                    rb.velocity=new Vector2(rb.velocity.x,-0.1f);
                }

                if (Mathf.Abs(dirX) >= 0.01f && IsGrounded())
                    anim.SetBool("isRunning", true);
                else
                    anim.SetBool("isRunning", false);

                if (IsGrounded())
                {
                    anim.SetBool("isJumping", false);
                    anim.SetBool("isFalling", false);
                    anim.SetBool("isDoubleJumping", false);
                    doubleJumped = false;
                }

                if (!IsGrounded()){
                    if (rb.velocity.y >= 0.01f && !doubleJumped)
                    {

                        anim.SetBool("isDoubleJumping", false);
                        anim.SetBool("isJumping", true);
                        if (inWater)rb.gravityScale=1;
                    }
                    else if (rb.velocity.y >= 0.01f && doubleJumped)
                    {
                        anim.SetBool("isJumping", false);
                        anim.SetBool("isDoubleJumping", true);
                        if (inWater)rb.gravityScale=1;
                    }
                    else if (rb.velocity.y <= -0.01f){
                        anim.SetBool("isFalling", true);
                        anim.SetBool("isDoubleJumping", false);
                        anim.SetBool("isJumping", false);
                        if (inWater)rb.gravityScale=0.2f;
                    }
                }

                if (Input.GetKeyDown(KeyCode.X) && shootTimer<=0){
                    if (canFireBall){
                        FireBallShoot();
                        shootTimer=shootTimerValue;
                    }
                    else if (canElectroBall){
                        ElectroBallShoot();
                        shootTimer=shootTimerValue;
                    }
                    else if (mitsos){
                        BananaShoot();
                        shootTimer=shootTimerValue;
                    }
                    else if (antigoni){
                        CupShoot();
                        shootTimer=shootTimerValue;
                    }
                }

                if (shootTimer>0)shootTimer-=Time.deltaTime;
            }
            else {
                anim.speed = 0;
                moveSpeed = 0;
                stunTimer -= Time.deltaTime;
                if (stunTimer <= 0)
                {
                    stunned = false;
                    moveSpeed = moveSpeedValue;
                    anim.speed = 1;
                    anim.SetBool("isFalling", false);
                    anim.SetBool("isDoubleJumping", false);
                    anim.SetBool("isJumping", false);
                    anim.SetBool("isRunning", false);
                    anim.SetTrigger("unstunned");
                }
            }

            }
        }
    }    

    private void FixedUpdate(){

        if (!dead){
            if (hitKBCounter <= 0 && explosionKBCounter<=0) rb.velocity = new Vector2(dirX*moveSpeed, rb.velocity.y);//to getrawaxis mhdenizei to rb.velocity.x
            if (hitKBCounter>0){
                
                if (hitKBFromRight) {
                    rb.velocity = new Vector2(-hitKBForce*3, hitKBForce);
                }
                else {
                    rb.velocity = new Vector2(hitKBForce*3, hitKBForce);
                }
                hitKBCounter -= Time.deltaTime;
            }
            if (explosionKBCounter > 0) explosionKBCounter -= Time.deltaTime;
        }
        

            if (!escape){

                if (CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.usingCheats=="y")doubleJumped=false;

                if (!dead){

                    if (canEatGhosts){
                        if (eatGhostsTimer<=0)canEatGhosts=false;
                        else eatGhostsTimer-=Time.deltaTime;
                    }

                    if (canMoveFast){
                        if (moveFastTimer<=0){
                            canMoveFast=false;
                            moveSpeed=moveSpeed-moveFasterByValue;
                            moveSpeedValue=moveSpeedValue-moveFasterByValue;
                            sprintValue=sprintValue-moveFasterByValue;
                        }
                        else moveFastTimer-=Time.deltaTime;
                    }

                    if (canJumpHigh){
                        if (jumpHighTimer<=0){
                            canJumpHigh=false;
                            jumpForce=jumpForce-jumpHigherByValue;
                        }
                        else {
                            jumpHighTimer-=Time.deltaTime;
                        }
                    }

                    if (canShield){
                        sonicShield.transform.position=transform.position;
                    }
                }
                else {

                    rb.velocity=Vector2.zero;

                    animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
                    current_animation = animatorinfo[0].clip.name;

                    if (current_animation.Equals("player_dead") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
                        anim.SetBool("isRunning",false);
//                        anim.SetBool("isSprinting",false);
                        anim.SetBool("isJumping", false);
                        anim.SetBool("isFalling", false);
                        anim.SetBool("isDoubleJumping", false);
                        anim.SetTrigger("respawn");
                        dead=false;
                        healthbarCanvas.SetActive(true);
                        GameManager.gameManager.RestartLevel();
                        shieldLife=0;
                        stunned=false;
                        stunTimer=0;
                        anim.speed=1;
                        hitKBCounter=0;
                        hitKBForce = 0;
                        hitKBFromRight = false;
                    }
                }
            }

            if (mitsos && rb.velocity.x<=0.001f && rb.velocity.x>=-0.001f && rb.velocity.y<=0.001f && rb.velocity.y>=-0.001f && IsGrounded() && !stunned){
                if (Input.GetKey(KeyCode.B))
                {
                    anim.SetBool("isHitting", true);
                }
                else anim.SetBool("isHitting", false);

                if (Input.GetKey(KeyCode.V))
                {
                    anim.SetBool("isFlexing", true);
                }
                else anim.SetBool("isFlexing", false);

                if (Input.GetKey(KeyCode.C))
                {
                    anim.SetBool("isWorkingOut", true);
                }
                else anim.SetBool("isWorkingOut", false);
            }
            else if (mitsos){
                anim.SetBool("isHitting", false);
                anim.SetBool("isFlexing", false);
                anim.SetBool("isWorkingOut", false);
            }
        }

    private void LateUpdate()
        {
        if (dirX > 0)
            if (mitsos)facingRight = false;
            else facingRight = true;
        else if(dirX < 0)
            if (mitsos)facingRight = true;
            else facingRight=false;


        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
                localScale.x *= -1;

            transform.localScale = localScale;
        }

    public void TakeDamage(int damage)
    {
        if (CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.usingCheats=="n"){
            if (!dead){
                if (canShield && !(damage>=maxHealth)){
                    shieldLife--;
                    if (shieldLife<=0){
                        canShield=false;
                        Destroy(sonicShield.gameObject);
                    }
                }
                else {
                    GameManager.gameManager.playerHealth.DamageUnit(damage);
                    healthbar.SetHealth(GameManager.gameManager.playerHealth.Health);
                    if (GameManager.gameManager.playerHealth.Health <= 0) {
                        popSound.Play();
                        anim.SetTrigger("dead");
                        dead=true;
                        healthbarCanvas.SetActive(false);
                        GameObject temp= Instantiate(disappearPrefab) as GameObject;
                        temp.transform.position =transform.position;
                        canShield=false;
                        if (sonicShield!=null)Destroy(sonicShield.gameObject);
                        shieldLife=0;
                        stunned=false;
                        stunTimer=0;
                        anim.speed=1;
                        hitKBCounter=0;
                        hitKBForce = 0;
                        hitKBFromRight = false;
                    }
                }
            }
        }
    }

    public void Heal(int healing)
    {
        if (healing>=maxHealth)healing=maxHealth;
        GameManager.gameManager.playerHealth.HealUnit(healing);
        healthbar.SetHealth(GameManager.gameManager.playerHealth.Health);
    }

    public void RegisterHitKnockback(float hitKBForce, bool hitKBFromRight) {
        hitKBCounter=hitKBDuration;
        this.hitKBForce = hitKBForce;
        this.hitKBFromRight = hitKBFromRight;
    }

    public void RegisterExplosionKnockback(Vector2 direction, float explosionKBForce)
    {

        if (!dead){
            rb.AddForce(direction * explosionKBForce);
            explosionKBCounter = explosionKBDuration;
        }
    }

    public float GetDirX() {
        return dirX;
    }

    public bool GetFacingRight() {
        return facingRight;
    }

    public void Stun(float stunTimerValue) {
        if (!dead){
            stunned = true;
            stunTimer = stunTimerValue;
        }
    }

    public bool IsStunned(){
        return stunned;
    }

    public bool IsGrounded(){
        return Physics2D.BoxCast(playerFeetCollider.bounds.center,playerFeetCollider.bounds.size,0f,Vector2.down,.25f,jumpableGround);
    }

    private void RotatePlayer(Vector3 rotation){
        transform.Rotate(rotation);
        transform.parent.Find("Feet").transform.Rotate(rotation);
        updatedRotation=true;
    }


    public void SetCurrentRotation(Vector3 rotation){
        currentRotation=rotation;
        updatedRotation=false;
    }

    void UpdateCoinCounter(){
        coinCounterText.text="X "+ coinsNum;
    }

    public bool isMoving(){
        return dirX!=0;
    }

    public bool isSprinting(){
        return dirX!=0 && moveSpeed==sprintValue && !stunned;
    }

    public void ResetDoubleJumped(){
        doubleJumped=false;
    }

    public void Earthquake(float force){
        if (!dead){
            rb.velocity=new Vector2(rb.velocity.x,force);
            Stun(2);
        }
    }

    public void InWater(){
        if (!inWater){
            rb.velocity=new Vector2(rb.velocity.x,rb.velocity.y/5);
            doubleJumped=false;
        }
        inWater=true;
    }

    public void OutOfWater(){
        inWater=false;
        rb.gravityScale=1;
    }

    public void ResetPowerUps(){
        canFireBall=false;
        canJumpHigh=false;
        canShield=false;
        canMoveFast=false;
        canEatGhosts=false;
        canElectroBall=false;
        shieldLife=0;
        eatGhostsTimer=0;
        moveFastTimer=0;
        jumpHighTimer=0;
    }

    private void FireBallShoot(){
        GameObject temp = Instantiate(marioFireBallPrefab) as GameObject;
        temp.transform.position = transform.position;
        if (facingRight)temp.GetComponent<MarioFireBallMove>().SetDirX(1);
        else temp.GetComponent<MarioFireBallMove>().SetDirX(-1);
        if (Input.GetKey(KeyCode.UpArrow))temp.GetComponent<MarioFireBallMove>().GoUp();
        AudioSource.PlayClipAtPoint(fireballSound, GameObject.Find("Camera").transform.position,1);
    }


    private void ElectroBallShoot(){
        GameObject temp = Instantiate(pikachuElectroBallPrefab) as GameObject;
        temp.transform.position = transform.position;
        if (facingRight)temp.GetComponent<PikachuElectroBallMove>().SetDirX(1);
        else temp.GetComponent<PikachuElectroBallMove>().SetDirX(-1);
        if (Input.GetKey(KeyCode.UpArrow))temp.GetComponent<PikachuElectroBallMove>().GoUp();
        AudioSource.PlayClipAtPoint(electroballSound, GameObject.Find("Camera").transform.position,1);
    }

    private void BananaShoot(){
        GameObject temp = Instantiate(bananaPrefab) as GameObject;
        temp.transform.position = transform.position;
        if (facingRight)temp.GetComponent<BananaMove>().SetDirX(-1);
        else temp.GetComponent<BananaMove>().SetDirX(1);
        if (Input.GetKey(KeyCode.UpArrow))temp.GetComponent<BananaMove>().GoUp();
        temp.GetComponent<BananaMove>().MitsosBanana();
        AudioSource.PlayClipAtPoint(fireballSound, GameObject.Find("Camera").transform.position,1);
    }

    private void CupShoot(){
        GameObject temp = Instantiate(cupPrefab) as GameObject;
        temp.transform.position = transform.position;
        if (facingRight)temp.GetComponent<CupMove>().SetDirX(1);
        else temp.GetComponent<CupMove>().SetDirX(-1);
        if (Input.GetKey(KeyCode.UpArrow))temp.GetComponent<CupMove>().GoUp();
        AudioSource.PlayClipAtPoint(fireballSound, GameObject.Find("Camera").transform.position,1);
    }

    public void FireBall(){
        canFireBall=true;
        shootTimer=0;
    }

    public void ElectroBall(){
        canElectroBall=true;
        shootTimer=0;
    }

    public void EatGhosts(){
        canEatGhosts=true;
        eatGhostsTimer=eatGhostsTimerValue;
        chasingGhostsSound.Stop();
        chasingGhostsSound.Play();
    }

    public void MoveFast(){
        if (!canMoveFast){
            moveSpeed=moveSpeed+moveFasterByValue;
            moveSpeedValue=moveSpeedValue+moveFasterByValue;
            sprintValue=sprintValue+moveFasterByValue;
        }
        moveFastTimer=moveFastTimerValue;
        canMoveFast=true;
    }

    public void JumpHigh(){
        if (!canJumpHigh)jumpForce=jumpForce+jumpHigherByValue;
        jumpHighTimer=jumpHighTimerValue;
        canJumpHigh=true;
    }

    public void Shield(){
        if(!canShield){
            sonicShield = Instantiate(sonicShieldPrefab) as GameObject;
            sonicShield.GetComponent<CircleCollider2D>().enabled=false;
            sonicShield.GetComponent<BoxCollider2D>().enabled=false;
            sonicShield.GetComponent<Rigidbody2D>().gravityScale=0;
        }
        shieldLife=3;
        canShield=true;
    }

    public bool ScaringGhosts(){
        return canEatGhosts;
    }

    public void ResetEatGhosts(){
        eatGhostsTimer=0;
        canEatGhosts=false;
        chasingGhostsSound.Stop();
    }

    public int GetCoins(){
        return coinsNum;
    }

    public void AddCoin(){
        coinsNum++;
        UpdateCoinCounter();
    }

    public void EscapePressed(){
        escape=true;
    }

    public void EscapeNotPressed(){
        escape=false;
    }

}