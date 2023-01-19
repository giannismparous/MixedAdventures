using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpookyMagicAttack1Move : MonoBehaviour
{

    private Rigidbody2D rb;
    private Vector3 localScale;
    private Animator anim;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private float moveSpeedValue;
    [SerializeField] private float destroyTimer;

    void Start()
    {
        localScale = transform.localScale;
        rb = transform.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        localScale.x *= -1;
        transform.localScale=localScale;
    }

    void FixedUpdate()
    {
        if (destroyTimer<=0)Destroy(gameObject);
        else destroyTimer-=Time.deltaTime;
        rb.transform.position = Vector2.MoveTowards(rb.transform.position, playerMove.transform.position, moveSpeedValue * Time.deltaTime);

    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        transform.rotation=Quaternion.Euler(0,0,(float)Math.Atan2( playerMove.transform.position.y - transform.position.y, playerMove.transform.position.x - transform.position.x ) * (float)( 180 / Math.PI ));
    }

}