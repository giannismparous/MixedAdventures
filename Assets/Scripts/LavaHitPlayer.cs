using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaHitPlayer : MonoBehaviour
{
    private PlayerMove playerMove;
    [SerializeField] private int damage;

    void Start()
    {
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            playerMove.TakeDamage(1000);
        }
        if (other.CompareTag("Enemy"))
        {
            if (other.transform.gameObject.transform.parent!=null)Destroy(other.transform.gameObject.transform.parent.transform.gameObject);
            else Destroy(other.transform.gameObject);
            
        }

    }
}
