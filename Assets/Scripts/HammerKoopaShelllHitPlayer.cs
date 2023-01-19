using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerKoopaShelllHitPlayer : MonoBehaviour
{
    private PlayerMove playerMove;
    private HammerKoopaShellMove hammerKoopaShellMove;
    [SerializeField] private float KBForce;

    void Start()
    {
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        hammerKoopaShellMove=transform.GetComponent<HammerKoopaShellMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && hammerKoopaShellMove.IsMoving())
        {
            if (other.transform.position.x <= transform.position.x) playerMove.RegisterHitKnockback(KBForce, true);
            else playerMove.RegisterHitKnockback(KBForce, false);
            playerMove.TakeDamage(10);
        }
    }
}
