using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombJumped : MonoBehaviour
{
    [SerializeField] private BombMove bombMove;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CheckEnemyHead>())
        {
            bombMove.Jumped();
        }
    }
}
