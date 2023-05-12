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
        private ComponentData m_Data = null;
        private NodeData m_NodeData = null;
        private SpriteRenderer m_SpriteRenderer = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Data = userData as ComponentData;
            m_NodeData = m_Data.NodeData;
            GameEntry.Entity.AttachEntity(this, m_NodeData.Id);
            m_SpriteRenderer = transform.GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sprite = GameEntry.Utils.sprites[8];
            m_NodeData.NodeType = NodeType.CenterNode;
            m_NodeData.NodeState = NodeState.InActive;
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            GameEntry.Event.Subscribe(SetSelectEventArgs.EventId, SetSelect);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            GameEntry.Event.Unsubscribe(SetSelectEventArgs.EventId, SetSelect);
        }
        //private void OnEnable()
        //{
        //    GameEntry.Event.Subscribe(SetSelectEventArgs.EventId,SetSelect);
        //}

        //private void OnDisable()
        //{
        //    GameEntry.Event.Unsubscribe(SetSelectEventArgs.EventId,SetSelect);
        //}

        private void SetSelect(object sender, GameEventArgs e)
        {
            SetSelectEventArgs ne = (SetSelectEventArgs)e;
            m_NodeData.Select = ne.Select;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            switch (m_NodeData.NodeState)
            {
                case NodeState.Undefined:
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
                    if (GameEntry.Utils.dragLine)
                        return;
                    GameEntry.Sound.PlaySound(10010);
                    var lineData = new LineData(GameEntry.Entity.GenerateSerialId(),10000,transform);
                    GameEntry.Entity.ShowLine(lineData);
                }
            }
        }
    }
}