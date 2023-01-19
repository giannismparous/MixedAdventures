using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlimeGhostMove : MonoBehaviour
{

    private float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float colliderTimer;
    private Vector2 gravityMode1;
    private Vector2 gravityMode2;
    private Vector2 gravityMode3;
    private Vector2 gravityMode4;
    private GameObject feet;
    [SerializeField] private float dirX;
    [SerializeField] private Vector2 gravityDirection;
    [SerializeField] private float gravityStrength;
    [SerializeField] private float gravityScale;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        moveSpeed = 3f;
        colliderTimer = 0;
        gravityMode1 = new Vector2(0,-1);
        gravityMode2 = new Vector2(-1, 0);
        gravityMode3 = new Vector2(0, 1);
        gravityMode4 = new Vector2(1, 0);
        feet = transform.parent.transform.Find("Feet").transform.gameObject;
        if (gravityDirection.Equals(gravityMode2))
        {
            transform.localRotation = Quaternion.Euler(0, 0, -90);
            feet.transform.localRotation= Quaternion.Euler(0, 0, -90);
        }
        else if (gravityDirection.Equals(gravityMode3))
        {
            transform.localRotation = Quaternion.Euler(0, 0, -180);
            feet.transform.localRotation = Quaternion.Euler(0, 0, -180);
        }
        else if (gravityDirection.Equals(gravityMode4))
        {
            transform.localRotation = Quaternion.Euler(0, 0, -270);
            feet.transform.localRotation = Quaternion.Euler(0, 0, -270);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (colliderTimer<=0 && (collision.CompareTag("Obstacle") || collision.CompareTag("Enemy")))
        {
            colliderTimer = 0.2f;
            dirX *= -1f;
        }

        //doesn't work if collided with enemy, if dirX=-1 there should be more change gravity colliders and the quaternion angles should be opposite (?), also local scale should be opposite but this may require quaternion angle first be set to zero and then reversed back
        if (collision.CompareTag("GravityChange")) {

            if (gravityDirection.Equals(gravityMode1))
            {
                gravityDirection = gravityMode2;
                transform.localRotation = Quaternion.Euler(0, 0, -90);
                feet.transform.localRotation= Quaternion.Euler(0, 0, -90);
            }
            else if (gravityDirection.Equals(gravityMode2))
            {
                gravityDirection = gravityMode3;
                transform.localRotation = Quaternion.Euler(0, 0, -180);
                feet.transform.localRotation = Quaternion.Euler(0, 0, -180);
            }
            else if (gravityDirection.Equals(gravityMode3))
            {
                gravityDirection = gravityMode4;
                transform.localRotation = Quaternion.Euler(0, 0, -270);
                feet.transform.localRotation = Quaternion.Euler(0, 0, -270);
            }
            else if (gravityDirection.Equals(gravityMode4))
            {
                gravityDirection = gravityMode1;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                feet.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            rb.velocity = Vector2.zero;

        }
    }

    void FixedUpdate()
    {

        // Optionally, you could normalize the gravity direction to make sure it always has length 1
        // Handling gravity by itself
        rb.AddForce(gravityDirection * gravityStrength * gravityScale, ForceMode2D.Force);

        if (gravityDirection[0]==0)rb.velocity = new Vector2(gravityDirection[1] * (-1) * moveSpeed, rb.velocity.y);
        else if (gravityDirection[1]==0) rb.velocity = new Vector2(rb.velocity.x, gravityDirection[0] * moveSpeed);


        colliderTimer -= Time.deltaTime;
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        if (dirX > 0)
            facingRight = true;
        else if (dirX < 0)
            facingRight = false;

        if (((facingRight) && (parentLocalScale.x < 0)) || ((!facingRight) && (parentLocalScale.x > 0)))
            parentLocalScale.x *= -1;

        transform.parent.localScale = parentLocalScale;
    }

    public void SetDirX(float dirX) { 
        this.dirX=dirX;
    }

}