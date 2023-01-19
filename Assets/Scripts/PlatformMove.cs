using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{

    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float moveSpeed;
    private int currentWaypointIndex;
    
    void Start()
    {
        currentWaypointIndex=0;
    }

    void Update()
    {
        
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position,transform.position)<=0.1f)currentWaypointIndex=(currentWaypointIndex+1)%waypoints.Length;

        transform.position=Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position,Time.deltaTime*moveSpeed);

    }

    void OnTriggerEnter2D(Collider2D collision){

        if (collision.CompareTag("CheckForGround"))collision.gameObject.transform.parent.transform.SetParent(transform);

    }

    void OnTriggerExit2D(Collider2D collision){

        if (collision.CompareTag("CheckForGround"))collision.gameObject.transform.parent.transform.SetParent(null);

    }
}
