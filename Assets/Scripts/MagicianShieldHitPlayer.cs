using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianShieldHitPlayer : MonoBehaviour
{
    private Rigidbody2D otherRB;
    private MagicianMovesController magicianMovesController;
    private PlayerMove playerMove;
    [SerializeField] private float KBForce;

    void Start()
    {
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        magicianMovesController = transform.parent.transform.Find("MagicianBody").GetComponent<MagicianMovesController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && magicianMovesController.ShieldIsActivated())
        {
            if (other.transform.position.x <= transform.position.x) playerMove.RegisterHitKnockback(KBForce, true);
            else playerMove.RegisterHitKnockback(KBForce, false);
            playerMove.TakeDamage(10);
        }

    }
}
