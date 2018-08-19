using HutongGames.PlayMaker;
using UnityEngine;

namespace MichaelWolfGames.CC2D
{
    public class SlidingState : CC2D_ControllerMoveState
    {
        #region Fields
        public float _cayoteDuration = 0.09f;

        public FsmEvent OnJump;
        public FsmEvent OnEnterAir;
        public FsmEvent OnStopSlide;

        #endregion
        #region Protected Member Variables

        private bool isInAir = false;

        private float cayoteTimer = 0f;
        private bool cayoteTimeActive = false;

        #endregion

        public override void Reset()
        {
            base.Reset();
            cayoteTimer = 0f;
            cayoteTimeActive = false;
        }

        protected override Vector2 MovementUpdate(float deltaTime)
        {
            if ((Controller.IsGrounded || cayoteTimeActive)) 
            {
                if (Input.GetButtonDown("Jump"))
                {
                    StartJump();
                }

                if (cayoteTimeActive)
                {
                    cayoteTimer += deltaTime;
                    if (cayoteTimer > _cayoteDuration)
                    {
                        cayoteTimeActive = false;
                        cayoteTimer = 0f;
                    }
                }
                else
                {
                    if (Controller.WasGrounded)
                    {
                        cayoteTimeActive = true;
                        cayoteTimer = 0f;
                    }
                }
            }
            else
            {
                if (Controller.IsGrounded)
                {
                    Fsm.Event(OnStopSlide);
                }
                else if(!cayoteTimeActive)
                {
                    Fsm.Event(OnEnterAir);
                }
            }

            return Controller.Rigidbody.velocity;
        }

        private void StartJump()
        {
            cayoteTimer = 0f;
            cayoteTimeActive = false;
            Fsm.Event(OnJump);
        }

    }
}