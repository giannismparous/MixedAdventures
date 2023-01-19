using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerKoopaShellMove : MonoBehaviour
{

    private float dirX;
    private float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float colliderTimer;
    private Animator anim;

    void Start()
    {   
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        dirX = 1f;
        moveSpeed = 0;
        colliderTimer = 0;
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (colliderTimer <= 0  && (collision.CompareTag("Player") || collision.CompareTag("Enemy")))
        {
            colliderTimer = 0.2f;
            moveSpeed = 8f;
            if (transform.position.x > collision.transform.position.x)
            {
                
                dirX = 1f;
            }
            else
            {
                dirX = -1f;
            }
            Debug.Log(collision.gameObject.name);
        }
        if (colliderTimer <= 0 && (collision.CompareTag("Ground")))
        {
            dirX *= -1f;
            colliderTimer = 0.2f;
        }
        
    }


    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        colliderTimer -= Time.deltaTime;

        if (Mathf.Abs(rb.velocity.x)>0) anim.SetBool("isSpinning", true);
        else anim.SetBool("isSpinning", false);
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

    public void SetMoveSpeed(float moveSpeed) {
        this.moveSpeed = moveSpeed;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public bool IsMoving(){
        return rb.velocity.x!=0;
    }

    public void InvertDirX(){
        dirX=-dirX;
    }

}
