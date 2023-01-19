using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GengarFoundPlayer : MonoBehaviour
{
    private Rigidbody2D otherRB;
    private GengarMove gengarMove;

    void Start()
    {
        gengarMove = transform.parent.transform.Find("GengarBody").GetComponent<GengarMove>();
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (!gengarMove.IsUsingFacade() & !gengarMove.IsSpitting() && other.CompareTag("Player"))
        {
            gengarMove.ActivateFacade();
        }

    }

}
