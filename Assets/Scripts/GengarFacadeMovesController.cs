using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GengarFacadeMovesController : MonoBehaviour
{
    private int dirX;
    private Rigidbody2D rb;
    private Vector3 localScale;
    private bool facingRight;
    private PlayerMove playerMove;
    private float destroyTimer=10;

    void Start() {
        rb = transform.GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        Physics2D.IgnoreCollision(player.transform.Find("Feet").GetComponent<BoxCollider2D>(), transform.Find("Feet").GetComponent<BoxCollider2D>(), true);
    }

    void FixedUpdate() {
        if (playerMove.transform.parent.transform.position.x < rb.transform.position.x) facingRight = false;
        else facingRight = true;

        destroyTimer-=Time.deltaTime;
        if (destroyTimer<=0)Destroy(transform.gameObject);
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

        if (((!facingRight) && (localScale.x < 0)) || ((facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(transform.gameObject);
        }
        if (collision.CompareTag("Ground"))
        {
            Destroy(transform.gameObject);
        }
    }
}
