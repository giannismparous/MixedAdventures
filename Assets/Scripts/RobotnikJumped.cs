using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotnikJumped : MonoBehaviour
{

    private float hitTimer;
    private RobotnikMove robotnikMove;
    [SerializeField] private float hitTimerValue;

    void Start(){
        robotnikMove=transform.parent.transform.GetComponent<RobotnikMove>();
        hitTimer=0;
    }

    void Update(){
        hitTimer-=Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.GetComponent<CheckEnemyHead>())
        {
            if (hitTimer<=0){
                robotnikMove.Hit();
                hitTimer=hitTimerValue;
            }
        }
    }
}
