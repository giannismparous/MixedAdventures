using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnorlaxHitPlayer : MonoBehaviour
{
    private PlayerMove playerMove;
    private SnorlaxMove snorlaxMove;
    [SerializeField] private float KBForce;

    void Start()
    {
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        snorlaxMove = transform.parent.Find("SnorlaxBody").GetComponent<SnorlaxMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (other.transform.position.x <= transform.position.x) playerMove.RegisterHitKnockback(KBForce, true);
            else playerMove.RegisterHitKnockback(KBForce, false);
            playerMove.TakeDamage(10);
            if (!snorlaxMove.IsAwake()) snorlaxMove.WakeUp();
        }

    }
}
