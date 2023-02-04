using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class Level1NodeComponent : BaseNodeComponent, IPointerDownHandler
    {
        [SerializeField] private int mCostValue = 0;
        [SerializeField] private GameObject mFrame = null;
        private NodeData m_NodeData = null;
        private List<BaseNodeComponent> m_ParentNodes = new List<BaseNodeComponent>();
        private int m_LineID = 0;
        public override List<BaseNodeComponent> ParentNodes
        {
            get
            {
                return m_ParentNodes;
            }
        }
        private List<BaseNodeComponent> m_ChildNodes = new List<BaseNodeComponent>();

        public override List<BaseNodeComponent> ChildNodes
        {
            get
            {
                return m_ChildNodes;
            }
        }

        private void Start()
        {
            m_NodeData = transform.GetComponent<NodeData>();
            m_NodeData.NodeType = NodeType.Level1Node;
            m_NodeData.NodeState = NodeState.InActive;
            m_NodeData.Select = false;
            mFrame.SetActive(m_NodeData.Select);
            m_NodeData.Cost = false;
            m_NodeData.Movable = false;
            m_NodeData.Connectable = true;
        }
        
        private void OnEnable()
        {
            GameEntry.Event.Subscribe(SetSelectEventArgs.EventId,SetSelect);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(SetSelectEventArgs.EventId,SetSelect);
        }

        private void Update()
        {
            
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
                    m_LineID = lineData.Id;
                    GameEntry.Entity.ShowLine(lineData);
                }
            }
        }
    }
}