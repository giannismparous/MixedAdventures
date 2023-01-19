using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointFlagController : MonoBehaviour
{

    private Animator anim;
    private bool triggered;
    private AudioSource audioSource;

    void Start(){
        anim=GetComponent<Animator>();
        audioSource=GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && !triggered)
        {
            audioSource.Play();
            anim.SetTrigger("checked");
            GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().CheckPointReached();
            triggered=true;
        }

    }
}
