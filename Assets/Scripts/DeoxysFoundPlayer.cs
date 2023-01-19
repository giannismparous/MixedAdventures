using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeoxysFoundPlayer : MonoBehaviour
{
    private DeoxysMove deoxysMove;
    private bool activated;

    void Start()
    {
        deoxysMove = transform.parent.transform.Find("DeoxysBody").GetComponent<DeoxysMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && !activated){
            deoxysMove.Activate();
            GameObject.Find("BGMusic").transform.GetComponent<AudioSource>().Stop();
            GameObject.Find("BossMusic").transform.GetComponent<AudioSource>().Play();
            activated=true;
        }

    }

}
