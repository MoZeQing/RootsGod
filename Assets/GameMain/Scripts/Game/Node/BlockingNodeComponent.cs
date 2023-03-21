using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class BlockingNodeComponent : BaseNodeComponent, IPointerDownHandler
    {
        [SerializeField] private int mCostValue = 0;
        [SerializeField] private GameObject mFrame = null;
        private ComponentData m_Data = null;
        private NodeData m_NodeData = null;
        private List<BaseNodeComponent> m_ParentNodes = new List<BaseNodeComponent>();
        private SpriteRenderer m_SpriteRenderer = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Data = userData as ComponentData;
            m_NodeData = m_Data.NodeData;
            GameEntry.Entity.AttachEntity(this, m_NodeData.Id);
            m_SpriteRenderer = transform.GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sprite = GameEntry.Utils.sprites[7];
            m_NodeData.NodeType = NodeType.BlockingNode;
            m_NodeData.NodeState = NodeState.Active;
            m_NodeData.Select = false;

            mFrame = transform.Find("NodeFrame").gameObject;
            mFrame.SetActive(m_NodeData.Select);
        }
        //private void Start()
        //{
        //    m_NodeData = transform.GetComponent<NodeData>();
        //    m_NodeData.NodeType = NodeType.BlockingNode;
        //    m_NodeData.NodeState = NodeState.Active;
        //    m_NodeData.Select = false;
        //    mFrame.SetActive(m_NodeData.Select);
        //    m_NodeData.Costable = false;
        //    m_NodeData.Movable = false;
        //    m_NodeData.Connectable = false;
        //    m_NodeData.Total = 0;
        //    m_NodeData.Income = 0;
        //    m_NodeData.CostPersecond = 1;
        //}

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            GameEntry.Event.Subscribe(SetSelectEventArgs.EventId, SetSelect);
            GameEntry.Event.Subscribe(AddParentNodeEventArgs.EventId, AddParent);
            GameEntry.Event.Subscribe(AddChildNodeEventArgs.EventId, AddChild);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            GameEntry.Event.Unsubscribe(SetSelectEventArgs.EventId, SetSelect);
            GameEntry.Event.Unsubscribe(AddParentNodeEventArgs.EventId, AddParent);
            GameEntry.Event.Unsubscribe(AddChildNodeEventArgs.EventId, AddChild);
        }
        //private void OnEnable()
        //{
        //    GameEntry.Event.Subscribe(SetSelectEventArgs.EventId,SetSelect);
        //    GameEntry.Event.Subscribe(AddParentNodeEventArgs.EventId,AddParent);
        //    GameEntry.Event.Subscribe(AddChildNodeEventArgs.EventId,AddChild);
        //}

        //private void OnDisable()
        //{
        //    GameEntry.Event.Unsubscribe(SetSelectEventArgs.EventId,SetSelect);
        //    GameEntry.Event.Unsubscribe(AddParentNodeEventArgs.EventId,AddParent);
        //    GameEntry.Event.Unsubscribe(AddChildNodeEventArgs.EventId,AddChild);
        //}

        //private void Update()
        //{
            
        //}

        private void AddParent(object sender, GameEventArgs e)
        {
            AddParentNodeEventArgs ne = (AddParentNodeEventArgs)e;
            if (ne.NodeData != m_NodeData)
                return;
            for (int i = 0; i < m_NodeData.ParentNodes.Count; i++)
            {
                if (m_NodeData.ChildNodes.Contains(m_NodeData.ParentNodes[i]))
                    return;
                if (m_NodeData.ParentNodes[i].NodeType == NodeType.ClearNode)
                {
                    m_NodeData.NodeState = NodeState.InActive;
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
                if (m_NodeData.ChildNodes[i].NodeType == NodeType.ClearNode)
                {
                    m_NodeData.NodeState = NodeState.InActive;
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