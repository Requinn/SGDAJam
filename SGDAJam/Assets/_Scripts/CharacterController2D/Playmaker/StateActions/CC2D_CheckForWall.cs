using HutongGames.PlayMaker;

namespace MichaelWolfGames.CC2D
{
    public class CC2D_CheckForWall : CC2D_ControllerStateBase
    {
        public FsmEvent OnDetectWall;

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Controller.CheckForWall(Controller.Rigidbody.velocity.x))
            {
                Fsm.Event(OnDetectWall);
            }
        }
    }
}