using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EZCameraShake;

public class CondorBomberMove : MonoBehaviour
{

    private float moveSpeed;
    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private bool isAttacking;
    private float condorBomberBulletTimer;
    private Transform explosionPositionTransform;
    private PlayerMove playerMove;
    [SerializeField] private float dirX;
    [SerializeField] private float moveSpeedValue;
    [SerializeField] private float condorBomberBulletTimerValue;
    [SerializeField] private GameObject condorBomberBulletPrefab;
    [SerializeField] private int explosionDamage;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;
    [SerializeField] private LayerMask explosionLayer;
    [SerializeField] private GameObject explosionEffect;

    //CameraShaker
    [SerializeField] private float explosionMagnitude;
    [SerializeField] private float explosionRoughness;
    [SerializeField] private float explosionFadeInTime;
    [SerializeField] private float explosionFadeOutTime;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        moveSpeed = moveSpeedValue;
        condorBomberBulletTimer = condorBomberBulletTimerValue;
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        moveSpeed = 0;
        explosionPositionTransform = transform.parent.transform.Find("ExplosionPosition").transform;
    }


    void FixedUpdate()
    {

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (isAttacking) {
            if (condorBomberBulletTimer <= 0)
            {
                Rigidbody2D temp1 = transform.parent.GetComponent<Rigidbody2D>();
                GameObject temp2 = Instantiate(condorBomberBulletPrefab) as GameObject;
                temp2.transform.position = new Vector2(temp1.position.x, temp1.position.y);
                temp2.GetComponent<CondorBomberBulletMove>().SetDirX(dirX);
                condorBomberBulletTimer = condorBomberBulletTimerValue;
            }
            else condorBomberBulletTimer -= Time.deltaTime;

            if (transform.position.x >= playerMove.transform.position.x) dirX = -1;
            else dirX = 1;
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

    public void InitiateAttack() {
        isAttacking = true;
        moveSpeed = moveSpeedValue;
    }

    public void StopAttack()
    {
        isAttacking = false;
        moveSpeed = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("Ground")) Explode();

    }

    void Explode()
    {
        Collider2D[] explosionAffectedObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionLayer);
        foreach (Collider2D obj in explosionAffectedObjects)
        {
            Vector2 direction = obj.transform.position - explosionPositionTransform.position;
            if (obj.CompareTag("Player"))
            {
                playerMove.TakeDamage(explosionDamage);
                obj.gameObject.GetComponent<PlayerMove>().RegisterExplosionKnockback(direction, 1000);
            }
            else
            {
                if (obj.transform.parent != null && obj.transform.parent.GetComponent<Rigidbody2D>()!=null) obj.transform.parent.GetComponent<Rigidbody2D>().AddForce(direction * explosionForce);
            }
        }
        //for mario
        float distance = (float)Math.Sqrt(Math.Pow(playerMove.transform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(playerMove.transform.position.y - transform.parent.transform.position.y, 2));
        //CameraShaker.Instance.ShakeOnce(explosionMagnitude, explosionRoughness, explosionFadeInTime, explosionFadeOutTime);
        GameObject explosionEffectInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity); //meaning no rotation
        Destroy(explosionEffectInstance, 10);
        Destroy(transform.parent.gameObject);
    }
}