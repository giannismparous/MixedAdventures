using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerKoopaShellJumped : MonoBehaviour
{

    [SerializeField] private HammerKoopaShellMove hammerKoopaShellMove;

    void Awake(){
        hammerKoopaShellMove=transform.parent.transform.Find("HammerKoopaShellBody").GetComponent<HammerKoopaShellMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CheckEnemyHead>())
        {
            hammerKoopaShellMove.SetMoveSpeed(0);
        }
    }
}
