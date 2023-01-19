using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    private float dirX;
    private bool facingRight;
    private Vector3 localScale;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private CharizardMove charizardMove;

    void Start() {
        localScale = transform.localScale;
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;
        if (current_animation.Equals("flamethrower_animation") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) {
            charizardMove.StopFlamethrower();
            Destroy(transform.gameObject);
        }
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        if (dirX > 0)
            facingRight = true;
        else if (dirX < 0)
            facingRight = false;

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    public void SetCharizardMove(CharizardMove charizardMove) {
        this.charizardMove = charizardMove;
    }

    public void SetDirX(float dirX) {
        this.dirX = dirX;
    }
}
