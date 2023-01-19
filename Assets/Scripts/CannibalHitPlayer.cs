using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannibalHitPlayer : MonoBehaviour
{
    private Rigidbody2D otherRB;
    [SerializeField] private CannibalMove cannibalMove;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private float KBForce;

    void Start()
    {
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("Player")) {
            cannibalMove.SetTimer(3);
            if (other.transform.position.x <= transform.position.x) playerMove.RegisterHitKnockback(KBForce, true);
            else playerMove.RegisterHitKnockback(KBForce, false);
            playerMove.TakeDamage(10);
        }

    }
}
