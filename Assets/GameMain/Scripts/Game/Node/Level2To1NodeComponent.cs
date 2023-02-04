using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class Level2To1NodeComponent : BaseNodeComponent, IPointerDownHandler
    {
        [SerializeField] private int mCostValue = 0;
        [SerializeField] private GameObject mFrame = null;
        private NodeData m_NodeData = null;
        private List<BaseNodeComponent> m_ParentNodes = new List<BaseNodeComponent>();
        private bool m_IsAdd = false;
        private int m_curPower = 0;
        private int m_MaxPower = 3;
        private int m_PrePower = 0;
        private int m_ConnectLevel2Num = 0;
        private int m_PreConnect = 0;
        private void Start()
        {
            m_NodeData = transform.GetComponent<NodeData>();
            m_NodeData.NodeType = NodeType.Level2To1Node;
            m_NodeData.NodeState = NodeState.InActive;
            m_NodeData.Select = false;
            mFrame.SetActive(m_NodeData.Select);
            m_NodeData.Costable = false;
            m_NodeData.Movable = false;
            m_NodeData.Connectable = true;
            m_NodeData.Total = 100;
            m_NodeData.Income = 0;
            m_NodeData.CostPersecond = 1;
            m_IsAdd = false;
        }
        
        private void OnEnable()
        {
            GameEntry.Event.Subscribe(SetSelectEventArgs.EventId,SetSelect);
            GameEntry.Event.Subscribe(AddParentNodeEventArgs.EventId,AddParent);
            GameEntry.Event.Subscribe(AddChildNodeEventArgs.EventId,AddChild);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(SetSelectEventArgs.EventId,SetSelect);
            GameEntry.Event.Unsubscribe(AddParentNodeEventArgs.EventId,AddParent);
            GameEntry.Event.Unsubscribe(AddChildNodeEventArgs.EventId,AddChild);
        }

        private void Update()
        {
            if (m_NodeData.Total <= 0)
            {
                m_NodeData.Total = 0;
                m_NodeData.NodeState = NodeState.InActive;
                return;
            }
            m_NodeData.Total -= m_NodeData.CostPersecond * Time.deltaTime;
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

        private void AddParent(object sender, GameEventArgs e)
        {
            AddParentNodeEventArgs ne = (AddParentNodeEventArgs)e;
            if (ne.NodeData != m_NodeData)
                return;
            for (int i = 0; i < m_NodeData.ParentNodes.Count; i++)
            {
                if (m_NodeData.ChildNodes.Contains(m_NodeData.ParentNodes[i]))
                    return;
                if (m_NodeData.ParentNodes[i].NodeType == NodeType.Level2Node)
                {
                    if (m_NodeData.ParentNodes[i].Income == 0)
                        continue;
                    m_ConnectLevel2Num++;
                    var power = m_curPower + m_ConnectLevel2Num * 2;
                    if (power > m_MaxPower)
                    {
                        var usedPower = power % 2;
                        m_NodeData.ParentNodes[i].Income -= usedPower;
                        m_curPower = m_MaxPower;
                    }
                    else
                    {
                        m_curPower = power;
                        m_NodeData.ParentNodes[i].Income = 0;
                    }
                    Debug.Log(m_curPower);
                    Debug.Log(m_curPower * 2 - m_PrePower * 2);
                    GameEntry.Event.FireNow(this,AddIncomeEventArgs.Create(m_curPower * 2 - m_PrePower * 2));
                    m_PrePower = m_curPower;
                }
            }
            
        }

        private void AddChild(object sender, GameEventArgs e)
        {
            AddChildNodeEventArgs ne = (AddChildNodeEventArgs)e;
            if (ne.NodeData != m_NodeData)
                return;
            for (int i = 0; i < m_NodeData.ChildNodes.Count; i++)
            {
                if (m_NodeData.ParentNodes.Contains(m_NodeData.ChildNodes[i]))
                    return;
                if (m_NodeData.ChildNodes[i].NodeType == NodeType.Level2Node)
                {
                    if (m_NodeData.ChildNodes[i].Income == 0)
                        continue;
                    m_ConnectLevel2Num++;
                    var power = m_curPower + m_ConnectLevel2Num * 2;
                    if (power > m_MaxPower)
                    {
                        var usedPower = power % 2;
                        m_NodeData.ChildNodes[i].Income -= usedPower;
                        m_curPower = m_MaxPower;
                    }
                    else
                    {
                        m_curPower = power;
                        m_NodeData.ChildNodes[i].Income = 0;
                    }
                    GameEntry.Event.FireNow(this,AddIncomeEventArgs.Create(m_curPower * 2 - m_PrePower * 2));
                    m_PrePower = m_curPower;
                }
            }
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
                    var lineData = new LineData(GameEntry.Entity.GenerateSerialId(),10000,transform);
                    GameEntry.Entity.ShowLine(lineData);
                }
            }
        }
    }
}