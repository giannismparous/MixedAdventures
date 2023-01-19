using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyoonJumped : MonoBehaviour
{

    private BuyoonMove buyoonMove;

    void Start() {
        buyoonMove = transform.parent.GetComponent<BuyoonMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CheckEnemyHead>())
        {
            if (buyoonMove.IsExtended())buyoonMove.Lower();
            buyoonMove.ResetExtendTimer();
        }
    }
}
