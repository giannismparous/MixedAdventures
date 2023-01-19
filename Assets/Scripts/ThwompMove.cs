using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThwompMove : MonoBehaviour
{

    private Animator anim;
    private int currentWaypointIndex;
    private bool triggered;
    private bool recovering;
    private float recoveringTimer;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float recoveringTimerValue;
    
    void Start()
    {
        anim=GetComponent<Animator>();
        currentWaypointIndex=0;
        triggered=false;
        recovering=false;
    }

    void Update()
    {
        
        if ((triggered || recovering) && Vector2.Distance(waypoints[currentWaypointIndex].transform.position,transform.position)<=0.1f){
            
            if (triggered && currentWaypointIndex!=0){
                recovering=true;
                recoveringTimer=recoveringTimerValue;
            }
            else if (recovering && currentWaypointIndex!=1){
                recovering=false;
                anim.SetTrigger("recovered");
            }
            currentWaypointIndex=(currentWaypointIndex+1)%waypoints.Length;
        }

        if (triggered && recovering && recoveringTimer<=0){
            triggered=false;
            anim.SetTrigger("recovering");
        }
        else if (triggered && recovering && recoveringTimer>0)recoveringTimer-=Time.deltaTime;

        if ((triggered && !recovering) || (recovering && !triggered))transform.position=Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position,Time.deltaTime*moveSpeed);

    }

    public bool IsRecovering(){
        return recovering;
    }

    public bool IsTriggered(){
        return triggered;
    }

    public void Trigger(){
        triggered=true;
        anim.SetTrigger("triggered");
    }

}
