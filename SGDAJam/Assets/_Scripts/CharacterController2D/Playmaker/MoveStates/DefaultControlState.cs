using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

namespace MichaelWolfGames.CC2D
{
    [ActionCategory("CC2D")]
    public class DefaultControlState : CC2D_ControllerMoveState
    {
        #region Fields
        //[Header("Movement")] 
        public float _moveSpeed = 7.5f;
        //[Header("Jumping")]
        public float _jumpSpeed = 10f;
        public float _fallSpeed = 10f;
        public float _minJumpDuration = 0.1f;
        public float _maxJumpDuration = 1.2f;
        public float _apexDuration = 0f;
        public float _hangTime = 0.1f;
        public float _cayoteDuration = 0.09f;
        public FsmAnimationCurve _apexCurve;

        public float _jumpCooldown = 0.1f;


        #endregion
        #region Protected Member Variables

        private bool isJumping = false;
        private float jumpTimer = 0f;
        private bool isOnApex = false;

        private float cayoteTimer = 0f;
        private bool cayoteTimeActive = false;

        // Not yet implemented.
        private float jumpCooldownTimer = 0f; // Jump as an ability with a cooldown?
        private bool jumpOnCooldown = false;

        #endregion

        public override void Reset()
        {
            base.Reset();
            isJumping = false;
            jumpTimer = 0f;
            isOnApex = false;
            cayoteTimer = 0f;
            cayoteTimeActive = false;

            jumpCooldownTimer = 0f;
            jumpOnCooldown = false;
        }

        public override void OnExit()
        {
            base.OnExit();
            StopJump();
        }

        protected override Vector2 MovementUpdate(float deltaTime)
        {
            float h = Input.GetAxis("Horizontal");
            float v = 0f; //Input.GetAxis("Vertical");
            h *= _moveSpeed;

            Vector2 resultVelocity;
            if (!isJumping)
            {
                resultVelocity = UpdateMove(deltaTime, h);

                if ((Controller.IsGrounded ||  cayoteTimeActive) && !jumpOnCooldown)
                {
                    if (Input.GetButtonDown("Jump") )
                    {
                        StartJump();
                    }
                }
            }
            else
            {
                bool jumpPressed = Input.GetButton("Jump");
                resultVelocity = UpdateJump(deltaTime, h, v, jumpPressed);
            }
            return resultVelocity;
        }

        #region Movement Logic

        protected virtual Vector2 UpdateMove(float deltaTime, float horz)
        {
            float vert = 0f;
            if (!Controller.IsGrounded)
            {
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
                    else
                    {
                        vert = -_fallSpeed;
                    }
                }
            }
            else // Is Grounded
            {
                vert = 0f;
                // Stopping at Walls & Steep Slopes
                if (Mathf.Abs(horz) > 0.001f)
                {
                    bool validMove = Controller.CheckForWalkableSlope(horz);
                    if (!validMove)
                    {
                        horz = 0f;
                    }
                }
                // Moving Up/Down Slopes.
                if (Mathf.Abs(horz) > 0.001f)
                {
                    if (Controller.CurrentSlopeAngle < Controller.SlopeLimit && Mathf.Abs(Controller.CurrentSlopeAngle) > 1f)
                    {
                        var orthNorm = Vector2.Perpendicular(Controller.CurrentGroundNormal);
                        var orthRatio = orthNorm.y / orthNorm.x;
                        var move = new Vector2(horz, horz * orthRatio);
                        horz = move.x;
                        vert = move.y;
                    }
                }
            }
            return new Vector2(horz, vert);
        }

        private void StartJump()
        {
            isJumping = true;
            jumpTimer = 0f;
            cayoteTimeActive = false;
            cayoteTimer = 0f;
            isOnApex = false;
        }

        private void StopJump()
        {
            isJumping = false;
            jumpTimer = 0f;
            cayoteTimeActive = false;
            cayoteTimer = 0f;
            isOnApex = false;
        }

        protected virtual Vector2 UpdateJump(float deltaTime, float horz, float vert, bool jumpPressed)
        {
            if (isJumping)
            {
                vert = _jumpSpeed;

                jumpTimer += deltaTime;
                if (jumpTimer > _minJumpDuration)
                {
                    if ((jumpPressed && !Controller.IsGrounded && !Controller.IsOnCeiling) || isOnApex)
                    {
                        if (jumpTimer > _maxJumpDuration)
                        {
                            isOnApex = true;

                            var lerp = ((jumpTimer - _maxJumpDuration) / (_apexDuration));
                            //var coslerp = 1f - Mathf.Cos(lerp * Mathf.PI * 0.5f);
                            //var sinlerp = Mathf.Sin(lerp * Mathf.PI * 0.5f);
                            //vert = Mathf.Lerp(_jumpSpeed, -_fallSpeed, lerp);
                            lerp = _apexCurve.curve.Evaluate(lerp);
                            vert = Mathf.Lerp(-_fallSpeed, _jumpSpeed, lerp);
                            if (jumpTimer > _maxJumpDuration + _apexDuration)
                            {
                                StopJump();
                            }
                        }
                    }
                    else
                    {
                        StopJump();
                    }
                }
            }
            return new Vector2(horz, vert);
        }
        #endregion
    }
}