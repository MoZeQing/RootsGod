using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameFramework;
using GameFramework.Event;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class LeafNode : BaseNodeComponent, IPointerDownHandler
    {
        [SerializeField] private int mCostValue = 0;
        [SerializeField] private GameObject mFrame = null;
        [SerializeField] private GameObject mProgress = null;
        [SerializeField] private GameObject mBoomProgress = null;
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

            mProgress = transform.Find("jingdutiao1").gameObject;
            mProgress.SetActive(m_NodeData.Select);

            mBoomProgress = transform.Find("boomjingdutiao1").gameObject;
            mBoomProgress.SetActive(m_NodeData.Select);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        private void OnEnable()
        {
            GameEntry.Event.Subscribe(SetSelectEventArgs.EventId, SetSelect);
            GameEntry.Event.Subscribe(AddParentNodeEventArgs.EventId, AddParent);
            GameEntry.Event.Subscribe(AddChildNodeEventArgs.EventId, AddChild);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(SetSelectEventArgs.EventId, SetSelect);
            GameEntry.Event.Unsubscribe(AddParentNodeEventArgs.EventId, AddParent);
            GameEntry.Event.Unsubscribe(AddChildNodeEventArgs.EventId, AddChild);
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
                    GameEntry.Event.FireNow(this, SetSelectEventArgs.Create(false));
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
                    var lineData = new LineData(GameEntry.Entity.GenerateSerialId(), 10000, transform);
                    GameEntry.Entity.ShowLine(lineData);
                }
            }
        }

        public override void RemoveNode()
        {
            foreach (NodeData parent in m_NodeData.ParentNodes)
            {
                if (GameEntry.Utils.Lines.ContainsKey(new ConnectPair(GameEntry.Entity.GetEntity(parent.Id).transform, transform.parent)))
                {
                    GameEntry.Entity.GetEntity(GameEntry.Utils.Lines[new ConnectPair(GameEntry.Entity.GetEntity(parent.Id).transform, transform.parent)].Id).gameObject.SetActive(false);
                }
                parent.ChildNodes.Remove(m_NodeData);
            }
            foreach (NodeData child in m_NodeData.ChildNodes)
            {
                if (GameEntry.Utils.Lines.ContainsKey(new ConnectPair(transform.parent, GameEntry.Entity.GetEntity(child.Id).transform)))
                {
                    GameEntry.Entity.GetEntity(GameEntry.Utils.Lines[new ConnectPair(transform.parent, GameEntry.Entity.GetEntity(child.Id).transform)].Id).gameObject.SetActive(false);
                }
                child.ParentNodes.Remove(m_NodeData);
                Node node = GameEntry.Entity.GetEntity(child.Id).GetComponent<Node>();
                node.Component.SetConnect(false);
            }
            this.gameObject.SetActive(false);
        }

        public override void SetConnect(bool flag)
        {
            foreach (NodeData parent in m_NodeData.ParentNodes)
            {
                if (GameEntry.Entity.GetEntity(parent.Id).GetComponent<Node>().Component.IsConnect)
                {
                    flag = true;
                    break;
                }
            }
            IsConnect = flag;
            foreach (NodeData child in m_NodeData.ChildNodes)
            {
                GameEntry.Entity.GetEntity(child.Id).GetComponent<Node>().Component.SetConnect(flag);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Line line = null;
            if (!other.TryGetComponent(out line))
                return;
            if (m_NodeData.NodeState == NodeState.InActive)
                return;
            GameEntry.Event.FireNow(this, LineVaildEventArgs.Create(false));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Line line = null;
            if (!other.TryGetComponent(out line))
                return;
            if (m_NodeData.NodeState == NodeState.InActive)
                return;
            GameEntry.Event.FireNow(this, LineVaildEventArgs.Create(true));
        }
    }
}