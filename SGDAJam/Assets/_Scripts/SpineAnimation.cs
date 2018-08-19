using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class SpineAnimation : StateMachineBehaviour
{
    public string animationName;
    public float speed = 1f;
    public bool loop = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SkeletonAnimation anim = animator.GetComponent<SkeletonAnimation>();
        anim.state.SetAnimation(0, animationName, loop).timeScale = speed;
    }
}
