using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CupMove : MonoBehaviour
{

    private float dirX;
    private float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 localScale;
    private float destructionTimer;
    private bool goingUp;
    [SerializeField] private float moveSpeedValue;
    [SerializeField] private float destructionTimerValue;

    void Start()
    {
        localScale = transform.transform.localScale;
        rb = transform.GetComponent<Rigidbody2D>();
        moveSpeed = moveSpeedValue;
        destructionTimer = destructionTimerValue;
    }

    void FixedUpdate()
    {
        if (goingUp)rb.velocity = new Vector2(0, moveSpeed);
        else rb.velocity = new Vector2(dirX * moveSpeed, 0);

        destructionTimer -= Time.deltaTime;
        if (destructionTimer<=0) Destroy(transform.gameObject);
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

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    public void SetDirX(float dirX) { 
        this.dirX=dirX;
    }

    public void GoUp(){
        goingUp=true;
        if (dirX>0)transform.rotation=Quaternion.Euler(0,0,90);
        else transform.rotation=Quaternion.Euler(0,0,-90);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Enemy"))
        {
            if (other.gameObject.transform.parent!=null)Destroy(other.gameObject.transform.parent.transform.gameObject);
            else Destroy(other.gameObject);
            Destroy(transform.gameObject);
        }

        if (other.CompareTag("Ground"))
        {
            Destroy(transform.gameObject);
        }

        if (other.CompareTag("Deoxys")){
            other.transform.GetComponent<DeoxysMove>().Hit(10);
            Destroy(transform.gameObject);
        }

    }

}