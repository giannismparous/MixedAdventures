using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastoiseAttack : MonoBehaviour
{
    private BlastoiseMove blastoiseMove;

    void Start()
    {
        blastoiseMove = transform.parent.transform.Find("BlastoiseBody").GetComponent<BlastoiseMove>();
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            blastoiseMove.InitiateAttack();
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            blastoiseMove.StopAttack();
        }
    }
}
