using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrapHitPlayer : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float KBForce;
    private PlayerMove playerMove;

    void Start()
    {
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (other.transform.position.x <= transform.position.x) playerMove.RegisterHitKnockback(KBForce, true);
            else playerMove.RegisterHitKnockback(KBForce, false);
            playerMove.TakeDamage(damage);
        }

    }
}
