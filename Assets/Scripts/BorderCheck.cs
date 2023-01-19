using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderCheck : MonoBehaviour
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
            playerMove.TakeDamage(damage);
        }

        if (other.CompareTag("Enemy"))
        {
            if (other.transform.parent!=null)Destroy(other.transform.parent.transform.gameObject);
            else Destroy(other.gameObject);
        }

    }
}
