using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EZCameraShake;

public class FlybotMove : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = false;
    private bool isAttacking;
    private Vector3 localScale;
    private PlayerMove playerMove;
    private System.Random rand;
    private Transform explosionPositionTransform;
    [SerializeField] private float dirX;
    [SerializeField] private float moveSpeedValue;
    [SerializeField] private float fallSpeed;
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
        localScale = transform.parent.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isAttacking = false;
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        rand = new System.Random();
        if ((rand.Next() % 2) ==0)explosionPositionTransform = transform.parent.transform.Find("ExplosionPosition1").transform;
        else explosionPositionTransform = transform.parent.transform.Find("ExplosionPosition2").transform;
    }


    void FixedUpdate()
    {
        
        if (!isAttacking)rb.velocity = new Vector2(dirX * moveSpeedValue, rb.velocity.y);
        else rb.velocity = new Vector2(0, -fallSpeed);

        if (!isAttacking && transform.position.x <= playerMove.transform.position.x + 0.1 && transform.position.x >= playerMove.transform.position.x - 0.1) {
            isAttacking = true;
            anim.SetTrigger("kamikaze");
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

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") || other.CompareTag("Obstacle") || other.CompareTag("Ground"))Explode();

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
                obj.transform.parent.GetComponent<Rigidbody2D>().AddForce(direction * explosionForce);
            }
        }
        
        float distance = (float)Math.Sqrt(Math.Pow(playerMove.transform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(playerMove.transform.position.y - transform.parent.transform.position.y, 2));
        //CameraShaker.Instance.ShakeOnce(explosionMagnitude, explosionRoughness, explosionFadeInTime, explosionFadeOutTime);
        GameObject explosionEffectInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity); //meaning no rotation
        Destroy(explosionEffectInstance, 10);
        Destroy(transform.parent.gameObject);
    }

}