using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiglettHitPlayer : MonoBehaviour
{
    private PlayerMove playerMove;
    private DiglettMove diglettMove;
    [SerializeField] private float KBForce;

    void Start()
    {
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        diglettMove =transform.parent.transform.Find("DiglettBody").GetComponent<DiglettMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && !diglettMove.IsUnderground())
        {
            if (other.transform.position.x <= transform.position.x) playerMove.RegisterHitKnockback(KBForce, true);
            else playerMove.RegisterHitKnockback(KBForce, false);
            playerMove.TakeDamage(10);
        }

    }
}
