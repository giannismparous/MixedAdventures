using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformController : MonoBehaviour
{

    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private bool falling;
    private bool triggered;
    [SerializeField] private float fallSpeedValue;
    
    void Start()
    {
        anim=GetComponent<Animator>();
    }

    void FixedUpdate(){

        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;
        
        if (falling)transform.position=Vector2.MoveTowards(transform.position,new Vector2(transform.position.x,-100),Time.deltaTime*fallSpeedValue);

        if (!falling && current_animation.Equals("falling_tile_triggered") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
            falling=true;
        }

    }

    void OnTriggerStay2D(Collider2D collision){

        if (collision.CompareTag("CheckForGround") && !triggered){
            collision.gameObject.transform.parent.transform.SetParent(transform);
            if (!falling)anim.SetTrigger("trigger");
            triggered=true;
        }

    }

    void OnTriggerExit2D(Collider2D collision){

        if (collision.CompareTag("CheckForGround") && triggered){
            collision.gameObject.transform.parent.transform.SetParent(null);
            if (!falling)anim.SetTrigger("untrigger");
            triggered=false;
        }

    }
}
