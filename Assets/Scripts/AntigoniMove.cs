using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AntigoniMove : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private float moveSpeed;
    private float dirX;
    private bool facingRight = true;
    private Vector3 localScale;
    private bool doubleJumped;
    [SerializeField] HealthBar healthbar;
    private float hitKBForce;
    private float hitKBCounter;
    private float explosionKBCounter;
    public float explosionKBDuration;
    public float hitKBDuration;
    private bool hitKBFromRight;
    private bool stunned;
    private float stunTimer;
    public float moveSpeedValue;
    public GameObject cupPrefab;


    private void Start()
        {
            rb = transform.parent.GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            localScale = transform.localScale;
            moveSpeed = moveSpeedValue;
		}

    private void Update()
        {
        //Debug
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                TakeDamage(20);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Heal(20);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                GameObject temp = Instantiate(cupPrefab) as GameObject;
                temp.transform.position = new Vector2(transform.position.x,transform.position.y);
                if(facingRight)temp.transform.GetComponent<CupMove>().SetDirX(1);
                else temp.transform.GetComponent<CupMove>().SetDirX(-1);
        }

        if (!stunned)
        {
            dirX = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump") && rb.velocity.y == 0)
                rb.AddForce(Vector2.up * 400f);

            if (Mathf.Abs(dirX) > 0 && rb.velocity.y == 0)
                anim.SetBool("isRunning", true);
            else
                anim.SetBool("isRunning", false);

            if (rb.velocity.y == 0)
            {

                //anim.SetBool("isJumping", false);
                //anim.SetBool("isFalling", false);
                doubleJumped = false;
            }

            if (rb.velocity.y > 0 && !doubleJumped)
            {

                if (Input.GetButtonDown("Jump"))
                {
                    //anim.SetBool("isDoubleJumping", true);
                    rb.AddForce(Vector2.up * 100f);
                    //anim.SetBool("isJumping", false);
                    doubleJumped = true;
                }
                //else anim.SetBool("isJumping", true);

            }

            if (rb.velocity.y > 0 && doubleJumped)
            {
                //anim.SetBool("isDoubleJumping", true);
            }

            if (rb.velocity.y < 0)

            {
                if (Input.GetButtonDown("Jump") && !doubleJumped)
                {
                    //anim.SetBool("isDoubleJumping", true);
                    rb.velocity = Vector2.zero;
                    rb.AddForce(Vector2.up * 200f);
                    //anim.SetBool("isFalling", false);
                    doubleJumped = true;
                }
                else
                {
                    //anim.SetBool("isJumping", false);
                    //anim.SetBool("isDoubleJumping", false);
                    //anim.SetBool("isFalling", true);
                }

            }

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
            }
        }

           
        }    

    private void FixedUpdate()
        {
        
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

    private void LateUpdate()
        {
        if (dirX > 0)
            facingRight = true;
        else if(dirX < 0)
            facingRight = false;


        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
                localScale.x *= -1;

            transform.localScale = localScale;
        }

    public void TakeDamage(int damage)
    {
        GameManager.gameManager.playerHealth.DamageUnit(damage);
        healthbar.SetHealth(GameManager.gameManager.playerHealth.Health);
        if (GameManager.gameManager.playerHealth.Health <= 0) GameManager.gameManager.RestartLevel();
    }

    public void Heal(int healing)
    {
        GameManager.gameManager.playerHealth.HealUnit(10);
        healthbar.SetHealth(GameManager.gameManager.playerHealth.Health);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("!");
        }

    }

    public void RegisterHitKnockback(float hitKBForce, bool hitKBFromRight) {
        hitKBCounter=hitKBDuration;
        this.hitKBForce = hitKBForce;
        this.hitKBFromRight = hitKBFromRight;
    }

    public void RegisterExplosionKnockback(Vector2 direction, float explosionKBForce)
    {

        rb.AddForce(direction * explosionKBForce);
        explosionKBCounter = explosionKBDuration;
    }

    public float GetDirX() {
        return dirX;
    }

    public bool GetFacingRight() {
        return facingRight;
    }

    public void Stun(float stunTimerValue) {
        stunned = true;
        stunTimer = stunTimerValue;
    }


}