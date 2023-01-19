using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotnikFoundPlayer : MonoBehaviour
{
    private RobotnikMove robotnikMove;
    private bool activated;

    void Start()
    {
        robotnikMove = transform.parent.transform.Find("RobotnikBody").GetComponent<RobotnikMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && !activated){
            robotnikMove.Activate();
            GameObject.Find("BGMusic").transform.GetComponent<AudioSource>().Stop();
            GameObject.Find("BossMusic").transform.GetComponent<AudioSource>().Play();
            activated=true;
        }

    }

}
