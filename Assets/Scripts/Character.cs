using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator animator;
    internal void AnimateIdle()
    {
        animator.CrossFade("Idle",0.1f);
    }
    internal void AnimateWalk()
    {
        animator.CrossFade("Sneak",0.1f);
    }
    internal void AnimateRun()
    {
        animator.CrossFade("Run",0.1f);
    }
    internal void AnimateAttack()
    {
        animator.CrossFade("Attack",0.1f);
    }
    internal void AnimateWin()
    {
        animator.CrossFade("Win",0.1f);
    }
    internal void AnimateDie()
    {
        
    }
}
