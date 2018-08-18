using HutongGames.PlayMaker;
using UnityEngine;

namespace MichaelWolfGames.CC2D
{
    public class JumpingState : CC2D_ControllerMoveState
    {
        #region Fields

        public float _moveSpeed = 7.5f;
        public float _jumpSpeed = 10f;

        public FsmEvent OnFinishJump;

        #endregion

        private bool _isJumping = false;

        public override void OnEnter()
        {
            base.OnEnter();

            Debug.Log("JUMP!");
            Debug.DrawLine(Controller.GroundCheckPos, Controller.GroundCheckPos + Controller.CurrentGroundNormal * 10f, Color.magenta, 0.25f);
            Vector2 jumpVector = Controller.CurrentGroundNormal * _jumpSpeed;

            Controller.Rigidbody.AddForce(jumpVector, ForceMode2D.Impulse);
            _isJumping = true;
        }

        protected override Vector2 MovementUpdate(float deltaTime)
        {
            float h = Input.GetAxis("Horizontal");
            float v = 0f;
            h *= _moveSpeed;

            Vector2 resultVelocity = new Vector2(h, Controller.Rigidbody.velocity.y);
            if(_isJumping)
            {
                bool jumpPressed = Input.GetButton("Jump") || Input.GetButtonDown("Jump");
                if (!jumpPressed)
                {
                    float cancelSpeed = Mathf.Min(resultVelocity.y, 0.5f);
                    resultVelocity = new Vector2(resultVelocity.x, 0f);
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