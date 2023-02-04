using System;
using System.Collections.Generic;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public enum State
    {
        Undefined,
        Game,
        Next,
        Back
    }
    public class ProcedureMain : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get { return false; }
        }
        private State m_State = State.Undefined;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(ChangeProcedureStateEventArgs.EventId,ChangeState);
            m_State = State.Game;
            GameEntry.UI.OpenUIForm(UIFormId.GameForm);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(ChangeProcedureStateEventArgs.EventId,ChangeState);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            switch(m_State)
            {
                case State.Undefined:
                    break;
                case State.Game:
                    break;
                case State.Next:
                    break;
                case State.Back:
                    ChangeState<ProcedureMenu>(procedureOwner);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ChangeState(object sender,GameEventArgs e)
        {
            ChangeProcedureStateEventArgs ne = (ChangeProcedureStateEventArgs)e;
            m_State = ne.State;
        }
    }
}
