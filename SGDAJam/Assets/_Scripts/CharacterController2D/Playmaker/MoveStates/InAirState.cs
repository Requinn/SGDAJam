using HutongGames.PlayMaker;
using UnityEngine;

namespace MichaelWolfGames.CC2D
{
    public class InAirState : CC2D_ControllerMoveState
    {
        #region Fields
        public float _moveSpeed = 7.5f;

        public FsmEvent OnExitAir;

        #endregion

        protected override Vector2 MovementUpdate(float deltaTime)
        {
            float h = Input.GetAxis("Horizontal");
            float v = 0f;
            h *= _moveSpeed;

            Vector2 resultVelocity = new Vector2(h, Controller.Rigidbody.velocity.y);
            if (Controller.IsGrounded)
            {
                Fsm.Event(OnExitAir);
            }
            return resultVelocity;
        }
    }
}