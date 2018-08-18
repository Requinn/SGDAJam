using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace MichaelWolfGames.CC2D
{
    public abstract class CC2D_ControllerStateBase : FsmStateAction
    {
        [RequiredField]
        [CheckForComponent(typeof(CharacterController2D))]
        [HutongGames.PlayMaker.Tooltip("The GameObject with the CharacterController2D.")]
        public FsmOwnerDefault ControllerObject;

        protected CharacterController2D Controller;

        public override void Reset()
        {
            ControllerObject = null;
            Controller = null;

        }

        public override void Awake()
        {
            base.Awake();
            var go = Fsm.GetOwnerDefaultTarget(ControllerObject);
            if (go)
            {
                Controller = go.GetComponent<CharacterController2D>();
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (!Controller)
            {
                var go = Fsm.GetOwnerDefaultTarget(ControllerObject);
                if (go)
                {
                    Controller = go.GetComponent<CharacterController2D>();
                }
            }
            if (!Controller)
            {
                Debug.LogError("[CC2D_ControlStateBase]: CC2D Not Found!");
                Finish();
                return;
            }
        }
    }
}