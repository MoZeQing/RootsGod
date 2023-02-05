using System;
using System.Collections.Generic;
using ET;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityEngine;
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
            
            // 还原游戏速度
            GameEntry.Base.ResetNormalGameSpeed();
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(2);
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", 2.ToString());
                return;
            }
            Debug.Log("Start Load Scene");
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), Constant.AssetPriority.SceneAsset, this);
            BgmTask().Coroutine();
            GameEntry.Sound.PlaySound(10017);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(ChangeProcedureStateEventArgs.EventId,ChangeState);
            GameEntry.Sound.StopAllLoadedSounds();
            GameEntry.Sound.StopAllLoadingSounds();
            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.UI.CloseAllLoadingUIForms();
            GameEntry.Entity.HideAllLoadedEntities();
            GameEntry.Entity.HideAllLoadingEntities();
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

        private async ETTask BgmTask()
        {
            while (true)
            {
                if (GameEntry.Procedure.CurrentProcedure.ToString() != UCS.ProcedureMain)
                    return;
                GameEntry.Sound.PlayMusic(1);
                await GameEntry.Timer.WaitSeconds(111);
                if (GameEntry.Procedure.CurrentProcedure.ToString() != UCS.ProcedureMain)
                    return;
                GameEntry.Sound.PlayMusic(2);
                await GameEntry.Timer.WaitSeconds(70);
                if (GameEntry.Procedure.CurrentProcedure.ToString() != UCS.ProcedureMain)
                    return;
                GameEntry.Sound.PlayMusic(3);
                await GameEntry.Timer.WaitSeconds(74);
            }
        }
    }
}
