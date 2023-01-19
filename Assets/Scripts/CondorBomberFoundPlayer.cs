using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondorBomberFoundPlayer : MonoBehaviour
{
    private CondorBomberMove condorBomberMove;

    void Awake()
    {
        condorBomberMove = transform.parent.transform.Find("CondorBomberBody").GetComponent<CondorBomberMove>();
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            condorBomberMove.InitiateAttack();
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            condorBomberMove.StopAttack();
        }
    }
}
