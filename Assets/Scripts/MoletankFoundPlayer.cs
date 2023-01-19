using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoletankFoundPlayer : MonoBehaviour
{
    private MoletankMove moletankMove;

    void Start()
    {
        moletankMove = transform.parent.transform.Find("MoletankBody").GetComponent<MoletankMove>();
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (!moletankMove.isMovingTowardsPlayer() && other.CompareTag("Player"))
        {
            moletankMove.FoundPlayer();
        }

    }

}
