using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedKoopaShellJumped : MonoBehaviour
{

    [SerializeField] private RedKoopaShellMove redKoopaShellMove;

    void Start(){
        redKoopaShellMove=transform.parent.transform.Find("RedKoopaShellBody").GetComponent<RedKoopaShellMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CheckEnemyHead>())
        {
            redKoopaShellMove.SetMoveSpeed(0);
        }
    }
}
