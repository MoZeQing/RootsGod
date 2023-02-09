using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class Level2NodeComponent : BaseNodeComponent, IPointerDownHandler
    {
        [SerializeField] private int mTotal = 0;
        [SerializeField] private int mIncome = 0;
        [SerializeField] private int mCostPerSecond = 0;
        [SerializeField] private GameObject mFrame = null;
        [SerializeField] private GameObject mProgress = null;
        [SerializeField] private float mLerpTime = 0.5f;
        private NodeData m_NodeData = null;
        private List<BaseNodeComponent> m_ParentNodes = new List<BaseNodeComponent>();
        private bool m_IsAdd = false;
        private SpriteRenderer m_SpriteRenderer = null;
        private Rigidbody2D m_Rigidbody2D = null;
        private Color32 m_Color32 = new Color32(176, 176, 176, 255);
        private void Start()
        {
            m_Rigidbody2D = transform.GetComponent<Rigidbody2D>();
            m_SpriteRenderer = transform.GetComponent<SpriteRenderer>();
            m_SpriteRenderer.color = Color.red;
            m_NodeData = transform.GetComponent<NodeData>();
            m_NodeData.NodeType = NodeType.Level2Node;
            m_NodeData.NodeState = NodeState.InActive;
            m_NodeData.Select = false;
            mFrame.SetActive(m_NodeData.Select);
            m_NodeData.Costable = false;
            m_NodeData.Movable = false;
            m_NodeData.Connectable = true;
            m_NodeData.Total = mTotal;
            m_NodeData.Income = mIncome;
            m_NodeData.CostPersecond = mCostPerSecond;
            m_IsAdd = false;
        }
        
        private void OnEnable()
        {
            GameEntry.Event.Subscribe(SetSelectEventArgs.EventId,SetSelect);
            GameEntry.Event.Subscribe(SetRigidbodyTypeEventArgs.EventId,SetRigidType);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(SetSelectEventArgs.EventId,SetSelect);
            GameEntry.Event.Unsubscribe(SetRigidbodyTypeEventArgs.EventId,SetRigidType);
        }

        private void Update()
        {
            if (m_IsAdd)
            {
                return;
            }
            if (m_NodeData.Total <= 0)
            {
                m_NodeData.Total = 0;
                m_SpriteRenderer.DOColor(m_Color32,mLerpTime);
                m_NodeData.NodeState = NodeState.InActive;
                GameEntry.Event.FireNow(this,AddIncomeEventArgs.Create(-(2 - m_NodeData.Income) * 2));
                m_IsAdd = true;
                return;
            }
            m_NodeData.Total -= m_NodeData.CostPersecond * Time.deltaTime;
            
            if (m_NodeData.Income < 2)
            {
                m_NodeData.Total -= (2 - m_NodeData.Income) * Time.deltaTime;
            }
            mProgress.transform.SetLocalScaleX(1-(1 - m_NodeData.Total / mTotal));
            // if (m_NodeData.NodeState != NodeState.Active)
            //     return;
            //
            // if (!m_IsAdd)
            // {
            //     m_NodeData.Income = 2;
            //     m_IsAdd = true;
            // }
            // if (m_NodeData.Total < m_NodeData.Income)
            // {
            //     //GameEntry.Event.FireNow(this,AddIncomeEventArgs.Create(-m_NodeData.Income));
            //     return;
            // }
            // m_NodeData.Total -= m_NodeData.Income * Time.deltaTime;
        }

        private void SetRigidType(object sender,GameEventArgs e)
        {
            SetRigidbodyTypeEventArgs ne = (SetRigidbodyTypeEventArgs)e;
            m_Rigidbody2D.bodyType = ne.Type;
        }
        
        private void SetSelect(object sender, GameEventArgs e)
        {
            SetSelectEventArgs ne = (SetSelectEventArgs)e;
            m_NodeData.Select = ne.Select;
            mFrame.SetActive(m_NodeData.Select);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            switch (m_NodeData.NodeState)
            {
                case NodeState.Unknown:
                    break;
                case NodeState.InActive:
                    break;
                case NodeState.Active:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (!m_NodeData.Select)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    GameEntry.Event.FireNow(this,SetSelectEventArgs.Create(false));
                    m_NodeData.Select = true;
                    mFrame.SetActive(m_NodeData.Select);
                }
            }
            else
            {
                if (!GameEntry.Utils.LinePairs.ContainsKey(transform))
                    return;
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    if (GameEntry.Utils.dragLine)
                        return;
                    GameEntry.Sound.PlaySound(10010);
                    var lineData = new LineData(GameEntry.Entity.GenerateSerialId(),10000,transform);
                    GameEntry.Entity.ShowLine(lineData);
                }
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Line line = null;
            if (!other.TryGetComponent(out line))
                return;
            if (m_NodeData.NodeState == NodeState.InActive)
                return;
            GameEntry.Event.FireNow(this,LineVaildEventArgs.Create(false));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Line line = null;
            if (!other.TryGetComponent(out line))
                return;
            if (m_NodeData.NodeState == NodeState.InActive)
                return;
            GameEntry.Event.FireNow(this,LineVaildEventArgs.Create(true));
        }
    }
}