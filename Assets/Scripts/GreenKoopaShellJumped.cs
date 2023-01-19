using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKoopaShellJumped : MonoBehaviour
{

    private GreenKoopaShellMove greenKoopaShellMove;

    void Start(){
        greenKoopaShellMove=transform.parent.transform.Find("GreenKoopaShellBody").GetComponent<GreenKoopaShellMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CheckEnemyHead>())
        {
            greenKoopaShellMove.SetMoveSpeed(0);
        }
    }
}
