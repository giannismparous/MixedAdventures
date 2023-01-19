using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBladeHitPlayer : MonoBehaviour
{
    private PlayerMove playerMove;
    [SerializeField] private int damage;
    [SerializeField] private float KBForce;

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
         if (other.CompareTag("Enemy"))
        {
            if (other.transform.gameObject.transform.parent!=null)Destroy(other.transform.gameObject.transform.parent.transform.gameObject);
            else Destroy(other.transform.gameObject);
            
        }

    }
}
