using HutongGames.PlayMaker;
using UnityEngine;

namespace MichaelWolfGames.CC2D
{
    public class JumpingState : CC2D_ControllerMoveState
    {
        #region Fields

        public float _moveSpeed = 7.5f;
        public float _horzAcceleration = 50f;
        public float _horzDecelerationFactor = 0.9f;
        public float _jumpSpeed = 10f;

        public FsmEvent OnFinishJump;

        #endregion

        private bool _isJumping = false;

        public override void OnEnter()
        {
            base.OnEnter();

            Debug.DrawLine(Controller.GroundCheckPos, Controller.GroundCheckPos + Controller.CurrentGroundNormal * 10f, Color.magenta, 0.25f);

            Vector2 jumpVector = new Vector2(0f, _jumpSpeed);

            //Vector2 jumpNormal = (Controller.IsGrounded) ? Controller.CurrentGroundNormal : Vector2.up;
            //Vector2 jumpVector = jumpNormal * _jumpSpeed;

            //float jumpRatio = ((Mathf.Abs(jumpNormal.y) > 0) ? jumpNormal.x / jumpNormal.y : 1f);
            //Vector2 jumpVector = new Vector2(_jumpSpeed * jumpRatio, _jumpSpeed);// jumpNormal * _jumpSpeed * jumpRatio;

            Controller.Rigidbody.velocity = new Vector2(Controller.Rigidbody.velocity.x, 0f);
            Controller.Rigidbody.AddForce(jumpVector, ForceMode2D.Impulse);
            _isJumping = true;
        }

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
            if(_isJumping)
            {
                bool jumpPressed = Input.GetButton("Jump") || Input.GetButtonDown("Jump");
                if (!jumpPressed)
                {
                    float cancelSpeed = Mathf.Min(resultVelocity.y, 0.5f);
                    resultVelocity = new Vector2(resultVelocity.x, cancelSpeed);
                }
                if (!jumpPressed || (Controller.IsGrounded|| Controller.Rigidbody.velocity.y <= 0f))
                {
                    Fsm.Event(OnFinishJump);
                    _isJumping = false;
                }
            }
            return resultVelocity;
        }
    }
}