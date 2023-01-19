using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearDestruct : MonoBehaviour
{

    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;

    void Start(){
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;

        if (current_animation.Equals("disappear_animation") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)Destroy(transform.gameObject);
    }
}
