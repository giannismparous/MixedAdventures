using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharizardFoundPlayer : MonoBehaviour
{
    private Rigidbody2D otherRB;
    private CharizardMove charizardMove;

    void Start()
    {
        charizardMove = transform.parent.transform.Find("CharizardBody").GetComponent<CharizardMove>();
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (!charizardMove.isUsingFlamethrower() & !charizardMove.isMovingTowardsPlayer() && other.CompareTag("Player"))
        {
            charizardMove.FoundPlayer();
        }

    }

}
