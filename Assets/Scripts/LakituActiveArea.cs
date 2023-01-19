using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LakituActiveArea : MonoBehaviour
{
    private LakituMove lakituMove;

    void Start()
    {
        lakituMove = transform.parent.transform.Find("LakituBody").GetComponent<LakituMove>();
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player"))lakituMove.Active();

    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))lakituMove.Inactive();

    }

}
