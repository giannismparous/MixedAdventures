using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagnetonElectricBallController : MonoBehaviour
{

    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;
        if (current_animation.Equals("magneton_electric_ball_animation") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) Destroy(transform.gameObject);
    }
}