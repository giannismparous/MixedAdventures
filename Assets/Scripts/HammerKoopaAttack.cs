using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HammerKoopaAttack : MonoBehaviour
{

    private PlayerMove playerMove;
    private float distance;
    [SerializeField] private float distanceValue;
    [SerializeField] private HammerKoopaMove hammerKoopaMove;

    void Start() {
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
    }

    void Update()
    {

        distance = (float)Math.Sqrt(Math.Pow(playerMove.transform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(playerMove.transform.position.y - transform.parent.transform.position.y, 2));

        if (distance < distanceValue)
        {
            if (playerMove.transform.position.x< transform.parent.transform.position.x)hammerKoopaMove.SetDirX(-1);
            else hammerKoopaMove.SetDirX(1);
            hammerKoopaMove.Attack();
            
        }
        else {
            hammerKoopaMove.StopAttack();
        }
    }
}
