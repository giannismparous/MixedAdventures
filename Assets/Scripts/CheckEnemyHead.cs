using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyHead : MonoBehaviour
{

    private PlayerMove playerMove;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = this.gameObject.transform.parent.GetComponent<Rigidbody2D>();
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyHead"))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * 400f);
            playerMove.ResetDoubleJumped();
        }
    }
}
