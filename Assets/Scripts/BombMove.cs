using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EZCameraShake;

public class BombMove : MonoBehaviour
{

    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float colliderTimer;
    private float moveSpeed;
    private bool triggered;
    private float triggeredTimer;
    private Animator anim;
    private bool exploding;
    private float explodingTimer;
    private Transform explosionPositionTransform;
    [SerializeField] private float dirX;
    [SerializeField] private float moveSpeedValue;
    [SerializeField] private float moveSpeedTriggeredValue;
    [SerializeField] private float triggeredTimerValue;
    [SerializeField] private float explodingTimerValue;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;
    [SerializeField] private LayerMask explosionLayer;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private int explosionDamage1;
    [SerializeField] private int explosionDamage2;
    [SerializeField] private int explosionDamage3;

    //CameraShaker
    [SerializeField] private float explosionMagnitude;
    [SerializeField] private float explosionRoughness;
    [SerializeField] private float explosionFadeInTime;
    [SerializeField] private float explosionFadeOutTime;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        moveSpeed = moveSpeedValue;
        colliderTimer = 0;
        triggered = false;
        exploding = false;
        anim = GetComponent<Animator>();
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        explosionPositionTransform = transform.parent.Find("ExplosionPosition").transform;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (colliderTimer<=0 && (collision.CompareTag("Ground") || collision.CompareTag("Enemy")))
        {
            colliderTimer = 0.3f;
            dirX *= -1f;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        colliderTimer -= Time.deltaTime;
        if (!exploding && triggered && triggeredTimer > 0) triggeredTimer -= Time.deltaTime;
        else if (!exploding && triggered && triggeredTimer <= 0)
        {
            moveSpeed = 0;
            anim.SetTrigger("exploding");
            exploding = true;
            explodingTimer = explodingTimerValue;
        }
        else if (exploding) {
            explodingTimer -= Time.deltaTime;
            if (explodingTimer <= 0) {
                Explode();
            }
            
        }

    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {

        if (!triggered || exploding)
        {
            if (dirX > 0)
                facingRight = true;
            else if (dirX < 0)
                facingRight = false;
        }
        else {
            if (dirX > 0)
                facingRight = false;
            else if (dirX < 0)
                facingRight = true;
        }

        if (((facingRight) && (parentLocalScale.x < 0)) || ((!facingRight) && (parentLocalScale.x > 0)))
            parentLocalScale.x *= -1;

        transform.parent.localScale = parentLocalScale;
    }

    void Explode() {
        Collider2D[] explosionAffectedObjects = Physics2D.OverlapCircleAll(transform.position,explosionRadius,explosionLayer);
        foreach (Collider2D obj in explosionAffectedObjects) {

            Vector2 direction = obj.transform.position - explosionPositionTransform.position;
            if (obj.CompareTag("Player"))
            {
                obj.gameObject.GetComponent<PlayerMove>().RegisterExplosionKnockback(direction, 1000);
                float distance = (float)Math.Sqrt(Math.Pow(playerMove.transform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(playerMove.transform.position.y - transform.parent.transform.position.y, 2));
                if (distance <= explosionRadius / 3) playerMove.TakeDamage(explosionDamage1);
                else if (distance <= explosionRadius / 2) playerMove.TakeDamage(explosionDamage2);
                else if (distance <= explosionRadius) playerMove.TakeDamage(explosionDamage3);
            }
            else if (obj.CompareTag("Brick"))
            {
                //obj.transform.parent.GetComponent<Rigidbody2D>().AddForce(direction*explosionForce);
                Destroy(obj.gameObject);
            }
        }
        //for mario
        //CameraShaker.Instance.ShakeOnce(explosionMagnitude, explosionRoughness, explosionFadeInTime,explosionFadeOutTime);
        GameObject explosionEffectInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity); //meaning no rotation
        Destroy(explosionEffectInstance, 10);
        Destroy(transform.parent.gameObject);
    }

    void onDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public void Trigger() {
        triggered = true;
        moveSpeed = moveSpeedTriggeredValue;
        triggeredTimer = triggeredTimerValue;
        anim.SetTrigger("triggered");
        if (moveSpeedValue==0)anim.SetTrigger("jumped");
    }

    public void Jumped() {
        moveSpeed = 0;
        anim.SetTrigger("jumped");
    }

   
}