using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitsosMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float moveSpeed;
    private float dirX;
    private bool facingRight = true;
    private Vector3 localScale;
    private bool doubleJumped;
    [SerializeField] private GameObject bananaPrefab;

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

        if (Input.GetKey(KeyCode.LeftShift)) anim.SetBool("isRunning", true);
        else anim.SetBool("isRunning", false);

        if (rb.velocity.y>0) anim.SetBool("isJumping", true);
        else if (rb.velocity.y == 0) anim.SetBool("isJumping", false);

        if (Input.GetKey(KeyCode.Z))
        {
            anim.SetBool("isRubbingButt", true);
        }
        else anim.SetBool("isRubbingButt", false);

        if (Input.GetKey(KeyCode.X))
        {
            anim.SetBool("isFlexing", true);
        }
        else anim.SetBool("isFlexing", false);

        if (Input.GetKey(KeyCode.C))
        {
            anim.SetBool("isWorkingOut", true);
        }
        else anim.SetBool("isWorkingOut", false);

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, rb.velocity.y);
    }

    private void LateUpdate()
    {
        if (dirX < 0)
            facingRight = true;
        else if (dirX > 0)
            facingRight = false;


        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.parent.transform.localScale = localScale;
    }

}
