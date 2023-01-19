using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosionDestruction : MonoBehaviour
{
   
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    [SerializeField] private AudioSource explosionSound;

    void Start(){
        anim = GetComponent<Animator>();
        explosionSound.Play();
    }

    void Update()
    {
        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;

        if (current_animation.Equals("explosion_animation") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)Destroy(transform.gameObject);
    }
}
