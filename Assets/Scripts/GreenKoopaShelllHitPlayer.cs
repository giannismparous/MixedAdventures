using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKoopaShelllHitPlayer : MonoBehaviour
{
    private PlayerMove playerMove;
    private Rigidbody2D otherRB;
    private GreenKoopaShellMove greenKoopaShellMove;
    [SerializeField] private float KBForce;

    void Start()
    {
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        greenKoopaShellMove=transform.GetComponent<GreenKoopaShellMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && greenKoopaShellMove.IsMoving())
        {
            if (other.transform.position.x <= transform.position.x) playerMove.RegisterHitKnockback(KBForce, true);
            else playerMove.RegisterHitKnockback(KBForce, false);
            playerMove.TakeDamage(10);
        }
    }
}
