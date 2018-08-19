using HutongGames.PlayMaker;
using UnityEngine;

namespace MichaelWolfGames.CC2D
{
    public class InAirState : CC2D_ControllerMoveState
    {
        #region Fields
        public float _moveSpeed = 7.5f;
        public float _horzAcceleration = 50f;
        public float _horzDecelerationFactor = 0.9f;

        public FsmEvent OnExitAir;

        #endregion

        protected override Vector2 MovementUpdate(float deltaTime)
        {
            float h = Input.GetAxis("Horizontal");
            float v = 0f;
            //h *= _moveSpeed;

            if (Mathf.Abs(h) > 0.1f)
            {
                float horz = Controller.Rigidbody.velocity.x + (deltaTime * (h * _horzAcceleration));
                h = Mathf.Clamp(horz, -_moveSpeed, _moveSpeed);
            }
            else
            {
                if (Mathf.Abs(Controller.Rigidbody.velocity.x) > 0.01f)
                {
                    h = Controller.Rigidbody.velocity.x * _horzDecelerationFactor;
                }
            }

            Vector2 resultVelocity = new Vector2(h, Controller.Rigidbody.velocity.y);
            if (Controller.IsGrounded)
            {
                Fsm.Event(OnExitAir);
            }
            return resultVelocity;
        }
    }
}