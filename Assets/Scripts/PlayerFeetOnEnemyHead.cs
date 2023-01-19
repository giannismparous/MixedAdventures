using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeetOnEnemyHead : MonoBehaviour
{

    private PlayerMove playerMove;

    void Start(){
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
    }
     void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("EnemyHead"))
        {
            playerMove.ResetDoubleJumped();
        }

    }
}
