using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MichaelWolfGames.CC2D
{
    public class GroundControlState : CC2D_ControllerMoveState
    {
        protected override Vector2 MovementUpdate(float deltaTime)
        {
            return Controller.Rigidbody.velocity;
        }
    }
}