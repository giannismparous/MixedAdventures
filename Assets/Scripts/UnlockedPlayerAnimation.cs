using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedPlayerAnimation : MonoBehaviour
{
    private Animator anim;

    void Start(){
        anim=GetComponent<Animator>();
        anim.SetBool("isRunning",true);
    }

}
