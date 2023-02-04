
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
        [SerializeField] private int mBlood = 0;
        [SerializeField] private int mCurGameState = 0;
        [SerializeField] private float mTargetGameState = 0;
        [SerializeField] private int mTargetTime = 30;
        [SerializeField] private Text mBloodText = null;
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
            mBloodText.text = mBlood + " / " + m_TargetBlood;
            m_CurTime += Time.deltaTime;
            mCountProgress.fillAmount = m_CurTime / mTargetTime;
        }

        private void CheckStateOver()
        {
            if (mBlood < m_TargetBlood)
            {
                //游戏失败
                return;
            }
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
                mBlood += m_IncomePerSecond;
            }
        }

        private void SetGameState()
        {
            IDataTable<DRGameState> dtGameState = GameEntry.DataTable.GetDataTable<DRGameState>();
            DRGameState drGameState = dtGameState.GetDataRow(mCurGameState);
            if (drGameState == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", mCurGameState.ToString());
                return;
            }
            mTargetGameState = dtGameState.Count;
            m_TargetBlood = drGameState.Cost;
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
