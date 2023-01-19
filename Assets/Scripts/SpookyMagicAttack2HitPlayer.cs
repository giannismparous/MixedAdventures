using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyMagicAttack2HitPlayer : MonoBehaviour
{
    private PlayerMove playerMove;
    private float destructionTimer=10;
    [SerializeField] private int damage;
    [SerializeField] private float KBForce;

    void Start()
    {
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
    }

    void Update(){
        destructionTimer-=Time.deltaTime;
        if (destructionTimer<=0)Destroy(transform.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (other.transform.position.x <= transform.position.x) playerMove.RegisterHitKnockback(KBForce, true);
            else playerMove.RegisterHitKnockback(KBForce, false);
            playerMove.TakeDamage(damage);
            Destroy(transform.gameObject);
        }
        if (other.CompareTag("Ground"))Destroy(transform.gameObject);
    }
}
