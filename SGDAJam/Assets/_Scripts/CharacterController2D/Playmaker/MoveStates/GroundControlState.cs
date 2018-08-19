using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

namespace MichaelWolfGames.CC2D
{
    public class GroundControlState : CC2D_ControllerMoveState
    {
        #region Fields
        //[Header("Movement")] 
        public float _moveSpeed = 7.5f;
        public float _horzAcceleration = 50f;
        public float _horzDecelerationFactor = 0.9f;
        //[Header("Jumping")]
        public float _jumpSpeed = 10f;
        public float _fallSpeed = 10f;
        public float _cayoteDuration = 0.09f;

        public FsmEvent OnJump;
        public FsmEvent OnEnterAir;
        public FsmEvent OnExitAir;
        public FsmEvent OnStartSlide;


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

        public override void OnExit()
        {
            base.OnExit();
            //StopJump();
        }

        protected override Vector2 MovementUpdate(float deltaTime)
        {
            float h = Input.GetAxis("Horizontal");
            float v = 0f;
            //h *= _moveSpeed;

            //Vector2 resultVelocity = UpdateMove(deltaTime, h);
            //float x = resultVelocity.x;
            //float y = resultVelocity.y;
            if (Mathf.Abs(h) > 0.01f)
            {
                float horz = Controller.Rigidbody.velocity.x + (deltaTime * (h * _horzAcceleration));
                h = Mathf.Clamp(horz, -_moveSpeed, _moveSpeed);
            }
            else // No Input - Decelerate
            {
                if (Mathf.Abs(Controller.Rigidbody.velocity.x) > 0.01f)
                {
                    //h = Controller.Rigidbody.velocity.x * _horzDecelerationFactor;
                }
                else
                {
                    h = 0f;
                    //y = 0f;
                }
            }
            //resultVelocity = new Vector2(x, y);

            Vector2 resultVelocity = UpdateMove(deltaTime, h);

            if (Mathf.Abs(h) <= 0.01f)
            {
                if (Mathf.Abs(Controller.Rigidbody.velocity.x) > 0.01f)
                {
                    resultVelocity = new Vector2(Controller.Rigidbody.velocity.x * _horzDecelerationFactor, resultVelocity.y);
                }
            }

            if ((Controller.IsGrounded || cayoteTimeActive)) //&& !jumpOnCooldown
            {
                if (Input.GetButtonDown("Jump"))
                {
                    StartJump();
                    resultVelocity = Controller.Rigidbody.velocity;
                }
            }

            return resultVelocity;
        }

        #region Movement Logic

        protected virtual Vector2 UpdateMove(float deltaTime, float horz)
        {
            float vert = 0f;
            if (!Controller.IsGrounded && !cayoteTimeActive)
            {
                // Is In Air transition
                if (isInAir == false)
                {
                    Fsm.Event(OnEnterAir);
                    isInAir = true;
                }
                //horz = Controller.Rigidbody.velocity.x;
                vert = Controller.Rigidbody.velocity.y;
            }
            else // Is Grounded
            {
                if (isInAir == true)
                {
                    //Land Event?
                    isInAir = false;
                }
                vert = 0f;
                // Stopping at Walls & Steep Slopes
                if (Mathf.Abs(horz) > 0.001f)
                {
                    bool validMove = Controller.CheckForWalkableSlope(horz);
                    if (!validMove)
                    {
                        horz = 0f;
                        Fsm.Event(OnStartSlide);
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


                if (cayoteTimeActive)
                {
                    if (!Controller.IsGrounded)
                    {
                        vert = Controller.Rigidbody.velocity.y;
                    }
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
                        vert = Controller.Rigidbody.velocity.y;
                    }
                }
            }
            return new Vector2(horz, vert);
        }

        private void StartJump()
        {
            //Debug.Log("JUMP!");
            //Debug.DrawLine(Controller.GroundCheckPos, Controller.GroundCheckPos + Controller.CurrentGroundNormal *10f, Color.magenta, 0.25f);
            //Vector2 jumpVector = Controller.CurrentGroundNormal * _jumpSpeed;

            //Controller.Rigidbody.AddForce(jumpVector, ForceMode2D.Impulse);
            cayoteTimer = 0f;
            cayoteTimeActive = false;

            //isInAir = true;
            Fsm.Event(OnJump);
        }

        #endregion
    }
}