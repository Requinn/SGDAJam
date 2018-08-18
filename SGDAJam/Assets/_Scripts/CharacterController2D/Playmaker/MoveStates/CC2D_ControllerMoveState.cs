using System;
using UnityEngine;

namespace MichaelWolfGames.CC2D
{
    public abstract class CC2D_ControllerMoveState : CC2D_ControllerStateBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            SetUpdateFunction(MovementUpdate);
        }

        protected virtual void SetUpdateFunction(Func<float, Vector2> updateFunc)
        {
            if (Controller)
            {
                Controller.SetUpdateMovementFunc(updateFunc);
            }
        }
        protected abstract Vector2 MovementUpdate(float deltaTime);
    }
}