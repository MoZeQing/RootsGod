
using System;
using ET;
using GameFramework.DataTable;
using GameFramework.Event;
using Unity.Mathematics;
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
        [SerializeField] private Text mTargetBloodText = null;
        [SerializeField] private Image mCountProgress = null;

        [Header("GameOver")] 
        [SerializeField] private GameObject mDie = null;
        private float m_CurTime = 0;
        private int m_IncomePerSecond = 0;
        private int m_TargetBlood = 0;
        private bool m_GameOver = false;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(AddCostEventArgs.EventId,AddCost);
            GameEntry.Event.Subscribe(AddIncomeEventArgs.EventId,AddIncome);
            GameEntry.Utils.Blood = 100;
            mTargetGameState = 15;
            m_GameOver = false;
            mCurGameState = 0;
            m_IncomePerSecond = 0;
            m_TargetBlood = 0;
            m_CurTime = 0;
            mDie.SetActive(false);
            mStateText.text = mCurGameState.ToString("00");
            mCountProgress.fillAmount = 0;
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
            if (m_GameOver)
                return;
            UpdateUI();
            CheckStateOver();
        }
        
        public void OnQuitButtonClick()
        {
            //UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
            GameEntry.Event.FireNow(this,ChangeProcedureStateEventArgs.Create(State.Back));
        }

        public void OnPauseButtonClick()
        {
            GameEntry.Base.GameSpeed = GameEntry.Base.GameSpeed == 0 ? 1 : 0;
        }

        private void UpdateUI()
        {
            mBloodText.text = GameEntry.Utils.Blood.ToString();
            m_CurTime += Time.deltaTime;
            mCountProgress.fillAmount = m_CurTime / mTargetTime;
        }

        private void CheckStateOver()
        {
            if (mCountProgress.fillAmount < 1)
                return;
            if (GameEntry.Utils.Blood < m_TargetBlood || mCurGameState == 12)
            {
                m_GameOver = true;
                GameEntry.Sound.PlaySound(10019);
                mDie.SetActive(true);
                Invoke(nameof(OnQuitButtonClick),1.6f + 20f);
                return;
            }

            GameEntry.Utils.Blood -= m_TargetBlood;
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

                GameEntry.Sound.PlaySound(10011);
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

            GameEntry.Sound.PlaySound(10018);
            Instantiate(GameEntry.Utils.nodes[0], new Vector3(2, 0, 10.5f), quaternion.Euler(0, 0, 0));
            mTargetGameState = dtGameState.Count;
            m_TargetBlood = drGameState.Cost;
            GameEntry.Utils.depth = drGameState.PoolDepth;
            mTargetBloodText.text = m_TargetBlood.ToString();
            mStateText.text = mCurGameState.ToString("00");
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
