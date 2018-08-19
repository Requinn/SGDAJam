using HutongGames.PlayMaker;
using UnityEngine;

namespace MichaelWolfGames.CC2D
{
    [ActionCategory("CC2D")]
    public class DashSate : CC2D_ControllerMoveState
    {
        public FsmFloat DashDistance = 3.5f;
        public FsmFloat DashDuration = 0.2f;

        private Vector2 velocity;
        private float timer = 0f;

        public override void Reset()
        {
            base.Reset();
            velocity = Vector2.zero;
            timer = 0f;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Controller.LimitSpeed = false;
            timer = 0f;
            //we are dashing downwards for this attack
            velocity = Vector2.down * (DashDistance.Value / DashDuration.Value);
            //velocity = new Vector2(((Controller.IsFacingRight) ? 1 : -1) * (DashDistance.Value / DashDuration.Value), 0f);
        }

        public override void OnExit()
        {
            base.OnExit();
            Controller.LimitSpeed = true;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            timer += Time.deltaTime;
            if (timer > DashDuration.Value)
            {
                Finish();
            }
        }

        protected override Vector2 MovementUpdate(float deltaTime)
        {
            return velocity;
        }
    }
}