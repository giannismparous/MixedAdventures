using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BombAttack : MonoBehaviour
{

    public PlayerMove playerMove;
    public float distanceValue;
    public BombMove bombMove;
    private float distance;
    private bool triggered;

    void Start() {
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        triggered = false;
    }

    void Update() { 
    

        if (!triggered)
        {
            distance = (float) Math.Sqrt(Math.Pow(playerMove.transform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(playerMove.transform.position.y - transform.parent.transform.position.y, 2));
            if (distance < distanceValue)
            {
                bombMove.Trigger();
                triggered = true;
            }
        }
    }
}
