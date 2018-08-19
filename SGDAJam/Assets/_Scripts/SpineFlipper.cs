using System.Collections;
using System.Collections.Generic;
using MichaelWolfGames;
using MichaelWolfGames.CC2D;
using Spine.Unity;
using UnityEngine;

public class SpineFlipper : SubscriberBase<CharacterController2D>
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;

    protected override void Awake()
    {
        base.Awake();
        if (!skeletonAnimation) skeletonAnimation = this.GetComponent<SkeletonAnimation>();
    }

    protected override void SubscribeEvents()
    {
        SubscribableObject.OnChangeFacingDirection += FlipCharacter;
    }

    protected override void UnsubscribeEvents()
    {
        SubscribableObject.OnChangeFacingDirection -= FlipCharacter;
    }

    private void FlipCharacter(bool isFacingRight)
    {
        skeletonAnimation.skeleton.flipX = !isFacingRight;
    }

}
