using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrampolineController : MonoBehaviour
{
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    [SerializeField] private float upwardVelocity;

    void Start(){
        anim=GetComponent<Animator>();
    }

    void Update(){
        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;

        if (current_animation.Equals("trampoline_active") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) anim.SetBool("isActive", false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("CheckForGround"))
        {
            other.gameObject.transform.parent.GetComponent<Rigidbody2D>().velocity=new Vector2(other.gameObject.transform.parent.GetComponent<Rigidbody2D>().velocity.x,upwardVelocity);
            //other.gameObject.transform.parent.GetComponent<Rigidbody2D>().AddForce(Vector2.up * upwardForce);
            anim.SetBool("isActive", true);
        }

    }

}