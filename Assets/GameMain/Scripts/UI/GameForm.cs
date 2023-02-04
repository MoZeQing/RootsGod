
using System;
using ET;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class GameForm : UGuiForm
    {
        [SerializeField] private int mCurGameState = 0;
        [SerializeField] private float mTargetGameState = 0;
        [SerializeField] private int mTargetTime = 30;
        [SerializeField] private Text mBloodText = null;
        [SerializeField] private Text mStateText = null;
        [SerializeField] private Image mCountProgress = null;

        private float m_CurTime = 0;
        private int m_IncomePerSecond = 0;
        private int m_TargetBlood = 0;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(AddCostEventArgs.EventId,AddCost);
            GameEntry.Event.Subscribe(AddIncomeEventArgs.EventId,AddIncome);
            mCurGameState = 0;
            mStateText.text = mCurGameState + "阶段";
            SetGameState();
            UpdateTask().Coroutine();
        }
        
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(AddCostEventArgs.EventId,AddCost);
            GameEntry.Event.Unsubscribe(AddIncomeEventArgs.EventId,AddIncome);
        }

        private void Update()
        {
            UpdateUI();
            CheckStateOver();
        }

        private void UpdateUI()
        {
            mBloodText.text = GameEntry.Utils.Blood + " / " + m_TargetBlood;
            m_CurTime += Time.deltaTime;
            mCountProgress.fillAmount = m_CurTime / mTargetTime;
        }

        private void CheckStateOver()
        {
            if (mCountProgress.fillAmount < 1)
                return;
            if (GameEntry.Utils.Blood < m_TargetBlood)
            {
                //游戏失败
                return;
            }

            m_CurTime = 0;
            mCurGameState++;
            SetGameState();
        }

        private async ETTask UpdateTask()
        {
            while (enabled)
            {
                await GameEntry.Timer.WaitSeconds(1.0f);
                if (GameEntry.Procedure.CurrentProcedure.ToString() != UCS.ProcedureMain)
                {
                    break;
                }
                GameEntry.Utils.Blood += m_IncomePerSecond;
            }
        }

        private void SetGameState()
        {
            IDataTable<DRGameState> dtGameState = GameEntry.DataTable.GetDataTable<DRGameState>();
            DRGameState drGameState = dtGameState.GetDataRow(mCurGameState + 1);
            if (drGameState == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", mCurGameState.ToString());
                return;
            }
            mTargetGameState = dtGameState.Count;
            m_TargetBlood = drGameState.Cost;
            mStateText.text = mCurGameState + "阶段";
        }

        private void AddCost(object sender,GameEventArgs e)
        {
            AddCostEventArgs ne = (AddCostEventArgs)e;
            m_TargetBlood += ne.Cost;
        }
        
        private void AddIncome(object sender,GameEventArgs e)
        {
            AddIncomeEventArgs ne = (AddIncomeEventArgs)e;
            m_IncomePerSecond += ne.Income;
        }
    }
}
