using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JajaMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float moveSpeed;
    private float dirX;
    private bool facingRight = true;
    private Vector3 localScale;
    private bool doubleJumped;

    private void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localScale = transform.parent.transform.localScale;
        moveSpeed = 5f;
    }

    private void Update()
    {

        dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if (Input.GetButtonDown("Jump") && rb.velocity.y == 0)
            rb.AddForce(Vector2.up * 400f);

        if (rb.velocity.x>=0.001f || rb.velocity.x<=-0.001f) anim.SetBool("isRunning", true);
        else anim.SetBool("isRunning", false);

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, rb.velocity.y);
    }

    private void LateUpdate()
    {
        if (dirX < 0)
            facingRight = false;
        else if (dirX > 0)
            facingRight = true;


        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.parent.transform.localScale = localScale;
    }

}
