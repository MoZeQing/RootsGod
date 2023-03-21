
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
        [SerializeField] private Text mCardCostBloodText = null;
        [SerializeField] private Text mLineCostText = null;
        [SerializeField] private Image mCountProgress = null;
        [SerializeField] private GameObject mGuideImage = null;
        [SerializeField] private Text mTipsText = null;

        [Header("GameOver")] 
        [SerializeField] private GameObject mDie = null;
        private float m_CurTime = 0;
        private int m_IncomePerSecond = 0;
        private int m_TargetBlood = 0;
        private bool m_GameOver = false;
        private int m_BuyCount = 0;
        private RectTransform m_LineCostBg = null;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(AddCostEventArgs.EventId,AddCost);
            GameEntry.Event.Subscribe(AddIncomeEventArgs.EventId,AddIncome);
            GameEntry.Event.Subscribe(ShowTipsEventArgs.EventId,ShowTips);
            GameEntry.Utils.Blood = GameEntry.Utils.startBlood;
            mTargetGameState = 15;
            m_GameOver = false;
            mCurGameState = 0;
            m_IncomePerSecond = 0;
            m_TargetBlood = 0;
            m_CurTime = 0;
            mDie.SetActive(false);
            mStateText.text = mCurGameState.ToString("00");
            mCountProgress.fillAmount = 0;
            m_BuyCount = 0;
            mCardCostBloodText.text = GameEntry.Utils.cardCost[m_BuyCount].ToString();
            m_LineCostBg = mLineCostText.transform.parent.gameObject.GetComponent<RectTransform>();
            m_LineCostBg.gameObject.SetActive(false);
            SetGameState();
            UpdateTask().Coroutine();
        }
        
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(AddCostEventArgs.EventId,AddCost);
            GameEntry.Event.Unsubscribe(AddIncomeEventArgs.EventId,AddIncome);
            GameEntry.Event.Unsubscribe(ShowTipsEventArgs.EventId,ShowTips);
        }

        private void Update()
        {
            if (m_GameOver)
                return;
            UpdateUI();
            LineCostFollow();
            CheckStateOver();
        }

        private void LineCostFollow()
        {
            var vec = new Vector3(0.5f *Screen.width, 0.5f * Screen.height);
            m_LineCostBg.anchoredPosition =Input.mousePosition - vec;
            //Debug.Log(Input.mousePosition);
            mLineCostText.text = GameEntry.Utils.lineCost.ToString();
            if (GameEntry.Utils.Blood >= GameEntry.Utils.lineCost)
            {
                mLineCostText.color = Color.white;
            }
            else
            {
                mLineCostText.color = Color.black;
            }
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
        
        public void OnGuideButtonClick()
        {
            mGuideImage.SetActive(!mGuideImage.activeSelf);
        }

        public void OnBuyButtonClick()
        {
            var cost = GameEntry.Utils.cardCost[m_BuyCount];
            if (GameEntry.Utils.Blood < cost)
                return;
            GameEntry.Utils.Blood -= GameEntry.Utils.cardCost[m_BuyCount];
            m_BuyCount++;
            if (m_BuyCount > GameEntry.Utils.cardCost.Length - 1)
                m_BuyCount--;
            mCardCostBloodText.text = GameEntry.Utils.cardCost[m_BuyCount].ToString();

            //Scripta 存某个名称物体，遍历所有子节点，存位置 画个Save按钮，按一下执行


            //GameEntry.Utils.ShowCardPackage();
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
                Invoke(nameof(OnQuitButtonClick),1.6f + 2f);
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
            GameEntry.Utils.ShowCardPackage();
            m_BuyCount = 0;
            mCardCostBloodText.text = GameEntry.Utils.cardCost[m_BuyCount].ToString();
            mTargetGameState = dtGameState.Count;
            m_TargetBlood = drGameState.Cost;
            GameEntry.Utils.depth1 = drGameState.PoolDepth1;
            GameEntry.Utils.depth2 = drGameState.PoolDepth2;
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

        private void ShowTips(object sender, GameEventArgs e)
        {
            ShowTipsEventArgs ne = (ShowTipsEventArgs)e;
            switch (ne.TipType)
            {
                case 0:
                    m_LineCostBg.gameObject.SetActive(ne.Active);
                    break;
                case 1:
                    mTipsText.gameObject.SetActive(ne.Active);
                    break;
            }
            
        }
    }
}
