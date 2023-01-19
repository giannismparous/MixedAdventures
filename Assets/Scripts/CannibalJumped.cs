using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannibalJumped : MonoBehaviour
{
    public CannibalMove cannibalMove;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CheckEnemyHead>())
        {
            cannibalMove.SetTimer(3);
        }
    }
}
