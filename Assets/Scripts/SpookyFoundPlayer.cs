using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyFoundPlayer : MonoBehaviour
{
    private SpookyMove spookyMove;
    private bool activated;

    void Start()
    {
        spookyMove = transform.parent.transform.Find("SpookyBody").GetComponent<SpookyMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && !activated){
            spookyMove.Activate();
            GameObject.Find("BGMusic").transform.GetComponent<AudioSource>().Stop();
            GameObject.Find("BossMusic").transform.GetComponent<AudioSource>().Play();
            activated=true;
        }

    }

}
