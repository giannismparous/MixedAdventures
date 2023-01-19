using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrapController : MonoBehaviour
{

    private Animator anim;
    private Collider2D flameCollider;
    private float inactiveTimer;
    private float activeTimer;
    private bool active;
    [SerializeField] private float inactiveTimerValue;
    [SerializeField] private float activeTimerValue;
    
    void Start()
    {
        anim=GetComponent<Animator>();
        flameCollider=transform.Find("FlameCollider").GetComponent<Collider2D>();
    }

    void Update()
    {
        if (active){
            if (activeTimer>=0)activeTimer-=Time.deltaTime;
            else {
                active=false;
                inactiveTimer=inactiveTimerValue;
                flameCollider.enabled=false;
                anim.SetBool("active",false);
            }
        }
        else {
            if (inactiveTimer>=0)inactiveTimer-=Time.deltaTime;
            else {
                active=true;
                activeTimer=activeTimerValue;
                flameCollider.enabled=true;
                anim.SetBool("active",true);
            }
        }
    }
}
