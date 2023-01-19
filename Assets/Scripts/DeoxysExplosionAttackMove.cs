using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeoxysExplosionAttackMove : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private PlayerMove playerMove;
    private bool moving;
    private System.Random rand;
    private Transform explosionPositionTransform;
    [SerializeField] private int explosionDamage;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;
    [SerializeField] private LayerMask explosionLayer;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float aimTimer;
    [SerializeField] private float explosionTimer;
    //CameraShaker
    [SerializeField] private float explosionMagnitude;
    [SerializeField] private float explosionRoughness;
    [SerializeField] private float explosionFadeInTime;
    [SerializeField] private float explosionFadeOutTime;
    

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        moving=true;
        rand = new System.Random();
        if ((rand.Next() % 2) ==0)explosionPositionTransform = transform.Find("ExplosionPosition1").transform;
        else explosionPositionTransform = transform.Find("ExplosionPosition2").transform;
    }

    void FixedUpdate()
    {
       


        if (moving){
            transform.position=playerMove.transform.position;
            if (aimTimer<=0){
                moving=false;
                anim.SetTrigger("aimed");
            }
            else aimTimer-=Time.deltaTime;
        }
        else {
            if (explosionTimer<=0)Explode();
            else explosionTimer-=Time.deltaTime;
        }
        


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
        
        float distance = (float)Math.Sqrt(Math.Pow(playerMove.transform.position.x - transform.position.x, 2) + Math.Pow(playerMove.transform.position.y - transform.position.y, 2));
        //CameraShaker.Instance.ShakeOnce(explosionMagnitude, explosionRoughness, explosionFadeInTime, explosionFadeOutTime);
        GameObject explosionEffectInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity); //meaning no rotation
        Destroy(explosionEffectInstance, 10);
        Destroy(transform.gameObject);
    }
}