using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterfreeFoundPlayer : MonoBehaviour
{
    private ButterfreeMove butterfreeMove;

    void Start()
    {
        butterfreeMove = transform.parent.transform.Find("ButterfreeBody").GetComponent<ButterfreeMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (!butterfreeMove.AttackIsActivated() && other.CompareTag("Player"))
        {
            butterfreeMove.Attack();
        }

    }

}
