using System;
using UnityEngine;

namespace MichaelWolfGames.CC2D
{
    public class CC2D_CharacterFlipper : SubscriberBase<CharacterController2D>
    {
        [SerializeField] private Transform flippingTransform;
        private Vector3 _originalScale;

        protected override void Awake()
        {
            base.Awake();
            if(!flippingTransform) flippingTransform = this.transform;
            _originalScale = flippingTransform.localScale;
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
            flippingTransform.localScale = new Vector3(_originalScale.x * ((isFacingRight)? 1 : -1), 
                _originalScale.y, _originalScale.z);
        }
    }
}