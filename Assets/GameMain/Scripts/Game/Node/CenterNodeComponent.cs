using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using GameMain;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class CenterNodeComponent : BaseNodeComponent, IPointerDownHandler
    {
        [SerializeField] private int mCostValue = 0;
        private NodeData m_NodeData = null;

        private void Start()
        {
            m_NodeData = transform.GetComponent<NodeData>();
            m_NodeData.NodeType = NodeType.CenterNode;
            m_NodeData.NodeState = NodeState.InActive;
            m_NodeData.Select = false;
            m_NodeData.Costable = false;
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

        private void SetSelect(object sender, GameEventArgs e)
        {
            SetSelectEventArgs ne = (SetSelectEventArgs)e;
            m_NodeData.Select = ne.Select;
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
                }
            }
            else
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    GameEntry.Sound.PlaySound(10010);
                    var lineData = new LineData(GameEntry.Entity.GenerateSerialId(),10000,transform);
                    GameEntry.Entity.ShowLine(lineData);
                }
            }
        }
    }
}